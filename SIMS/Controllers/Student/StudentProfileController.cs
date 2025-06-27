using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Helpers;
using SIMS.Models;
using SIMS.Models.ViewModels;
using System.Security.Claims;

namespace SIMS.Controllers.Student
{
    public class StudentProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound();

            var model = new StudentProfileViewModel
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                EnrollmentDate = student.EnrollmentDate,
                Address = student.Address,
                PhoneNumber = student.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(StudentProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound();

            student.DateOfBirth = model.DateOfBirth;
            student.Address = model.Address;
            student.PhoneNumber = model.PhoneNumber;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string NewPassword, string ConfirmPassword)
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 8 || NewPassword != ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match or are invalid.";
                return RedirectToAction("Index");
            }

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            // ✅ Dùng PasswordHelper để hash đúng kiểu
            user.PasswordHash = PasswordHelper.HashPassword(NewPassword);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction("Index");
        }

        private Guid? GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdStr, out Guid userId) ? userId : null;
        }
    }
}
