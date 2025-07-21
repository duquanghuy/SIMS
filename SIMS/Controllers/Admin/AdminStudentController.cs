using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Helpers;
using SIMS.Models;

using SIMS.ViewModels;

namespace SIMS.Controllers.Admin
{
    public class AdminStudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminStudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var totalItems = _context.Students.Count();

            var students = _context.Students
                .OrderBy(s => s.StudentNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new
                {
                    s.StudentId,
                    s.StudentNumber,
                    s.FirstName,
                    s.LastName,
                    s.DateOfBirth,
                    EnrollmentDate = s.EnrollmentDate.Date,
                    Action = ""
                })
                .ToList();

            var columns = new List<TableColumn>
            {
                new TableColumn { Header = "Student ID", PropertyName = "StudentId" },
                new TableColumn { Header = "First Name", PropertyName = "FirstName" },
                new TableColumn { Header = "Last Name", PropertyName = "LastName" },
                new TableColumn { Header = "Enrollment Date", PropertyName = "EnrollmentDate" },
                new TableColumn { Header = "Action", PropertyName = "Action" }
            };

            ViewData["Students"] = students;
            ViewData["Columns"] = columns;
            ViewData["Pagination"] = new PaginationModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Action = "Index",
                Controller = "AdminStudent",
                RouteValues = new Dictionary<string, string>()
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(string FirstName, string LastName, DateTime EnrollmentDate)
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
                RoleId = Guid.Parse("00000000-0000-0000-0000-000000000003") // Student role
            });

            int nextStudentNumber = (_context.Students.Max(s => (int?)s.StudentNumber) ?? 0) + 1;
            string studentId = $"S{nextStudentNumber:D4}";

            var student = new Models.Student
            {
                StudentId = studentId,
                StudentNumber = nextStudentNumber,
                UserId = user.Id,
                FirstName = FirstName,
                LastName = LastName,
                EnrollmentDate = EnrollmentDate,
                DateOfBirth = DateTime.MinValue,
                Address = "",
                PhoneNumber = ""
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStudent(
            string StudentId,
            string FirstName,
            string LastName,
            DateTime EnrollmentDate,
            string OriginalFirstName,
            string OriginalLastName)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == StudentId);
            if (student == null)
                return NotFound("Student not found.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == student.UserId);
            if (user == null)
                return NotFound("Linked user not found.");

            FirstName = char.ToUpper(FirstName[0]) + FirstName.Substring(1).ToLower();
            LastName = char.ToUpper(LastName[0]) + LastName.Substring(1).ToLower();

            student.FirstName = FirstName;
            student.LastName = LastName;
            student.EnrollmentDate = EnrollmentDate;

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

        [HttpGet]
        public async Task<IActionResult> Delete(string studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == student.UserId);
            var roles = _context.UserRoles.Where(r => r.UserId == student.UserId);

            _context.UserRoles.RemoveRange(roles);
            if (user != null) _context.Users.Remove(user);
            _context.Students.Remove(student);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ✅ API: Get full student details for modal view
        [HttpGet]
        public async Task<IActionResult> GetStudentDetails(string StudentId)
        {
            var student = await _context.Students
                .Where(s => s.StudentId == StudentId)
                .Select(s => new
                {
                    s.StudentId,
                    s.FirstName,
                    s.LastName,
                    s.DateOfBirth,
                    s.EnrollmentDate,
                    s.PhoneNumber,
                    s.Address,
                    s.User.Email
                })
                .FirstOrDefaultAsync();

            if (student == null)
                return NotFound();

            return Json(student);
        }
    }
}
