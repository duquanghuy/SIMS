using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Helpers;
using SIMS.Models;
using SIMS.ViewModels;

namespace SIMS.Controllers.Admin
{
    public class AdminAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Guid AdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public AdminAdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchEmail = "")
        {
            var query = _context.Users
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == AdminRoleId));

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchEmail))
            {
                query = query.Where(u => u.Email.Contains(searchEmail));
            }

            var admins = query
                .Select(u => new
                {
                    u.Id, // ✅ This is required for Action detection
                    u.Email,
                    u.CreatedAt,
                    Action = "" // this key just triggers rendering
                })
                .ToList();

            ViewData["Admins"] = admins;

            ViewData["Columns"] = new List<TableColumn>
                {
                    new TableColumn { Header = "Email", PropertyName = "Email" },
                    new TableColumn { Header = "Created At", PropertyName = "CreatedAt" },
                    new TableColumn { Header = "Action", PropertyName = "Action" }
                };

            // Pass search value to view
            ViewData["SearchEmail"] = searchEmail;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAdmin(string Email, string Password)
        {
            if (_context.Users.Any(u => u.Email == Email))
                return Conflict("Admin with this email already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = Email.ToLower(),
                PasswordHash = PasswordHelper.HashPassword(Password),
                CreatedAt = DateTime.Now
            };
            _context.Users.Add(user);

            _context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = AdminRoleId
            });

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(Guid userId, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found.");

            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found.");

            var roles = _context.UserRoles.Where(r => r.UserId == user.Id);
            _context.UserRoles.RemoveRange(roles);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
