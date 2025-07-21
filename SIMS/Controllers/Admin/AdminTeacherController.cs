using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Helpers;
using SIMS.Models;
using SIMS.ViewModels;

namespace SIMS.Controllers.Admin
{
    public class AdminTeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminTeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            const int PageSize = 10;

            var totalItems = _context.Teachers.Count();

            var teachers = _context.Teachers
                .OrderByDescending(t => t.TeacherId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.TeacherId,
                    t.FirstName,
                    t.LastName,
                    Email = t.User.Email,
                    Action = ""
                })
                .ToList();

            var columns = new List<TableColumn>
            {
                new TableColumn { Header = "Teacher ID", PropertyName = "TeacherId" },
                new TableColumn { Header = "First Name", PropertyName = "FirstName" },
                new TableColumn { Header = "Last Name", PropertyName = "LastName" },
                 new TableColumn { Header = "Email", PropertyName = "Email" },
                new TableColumn { Header = "Action", PropertyName = "Action" }
            };

            ViewData["Teachers"] = teachers;
            ViewData["Columns"] = columns;
            ViewData["Pagination"] = new PaginationModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Action = "Index",
                Controller = "AdminTeacher",
                RouteValues = new Dictionary<string, string>()
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher(string FirstName, string LastName)
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                return BadRequest("Missing required fields");

            FirstName = char.ToUpper(FirstName[0]) + FirstName.Substring(1).ToLower();
            LastName = char.ToUpper(LastName[0]) + LastName.Substring(1).ToLower();

            string baseEmail = $"{FirstName}.{LastName}".ToLower();
            string email = $"{baseEmail}@sims.com";
            int suffix = 1;

            while (_context.Users.Any(u => u.Email == email))
            {
                email = $"{baseEmail}{suffix}@sims.com";
                suffix++;
            }

            string password = "123456";
            string hashedPassword = PasswordHelper.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.Now
            };
            _context.Users.Add(user);

            _context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = Guid.Parse("00000000-0000-0000-0000-000000000002") // Teacher role
            });

            var teacher = new Models.Teacher
            {
                UserId = user.Id,
                FirstName = FirstName,
                LastName = LastName,
                TeacherId = GenerateNextTeacherId()
            };
            _context.Teachers.Add(teacher);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private string GenerateNextTeacherId()
        {
            var lastTeacher = _context.Teachers
                .OrderByDescending(t => t.TeacherId)
                .FirstOrDefault();

            int lastNumber = 0;

            if (lastTeacher != null && !string.IsNullOrEmpty(lastTeacher.TeacherId))
            {
                string numberPart = lastTeacher.TeacherId.Substring(1);
                int.TryParse(numberPart, out lastNumber);
            }

            int newNumber = lastNumber + 1;
            return $"T{newNumber.ToString("D4")}";
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string teacherId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            if (teacher == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacher.UserId);
            var roles = _context.UserRoles.Where(r => r.UserId == teacher.UserId);

            _context.UserRoles.RemoveRange(roles);
            if (user != null) _context.Users.Remove(user);
            _context.Teachers.Remove(teacher);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeacher(
            string OriginalTeacherId,
            string FirstName,
            string LastName,
            string OriginalFirstName,
            string OriginalLastName)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherId == OriginalTeacherId);
            if (teacher == null)
                return NotFound("Teacher not found.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacher.UserId);
            if (user == null)
                return NotFound("Linked user not found.");

            FirstName = char.ToUpper(FirstName[0]) + FirstName.Substring(1).ToLower();
            LastName = char.ToUpper(LastName[0]) + LastName.Substring(1).ToLower();

            teacher.FirstName = FirstName;
            teacher.LastName = LastName;

            if (!string.Equals(OriginalFirstName, FirstName, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(OriginalLastName, LastName, StringComparison.OrdinalIgnoreCase))
            {
                string baseEmail = $"{FirstName}.{LastName}".ToLower();
                string email = $"{baseEmail}@sims.com";
                int suffix = 1;

                while (_context.Users.Any(u => u.Email == email && u.Id != user.Id))
                {
                    email = $"{baseEmail}{suffix}@sims.com";
                    suffix++;
                }

                user.Email = email;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
