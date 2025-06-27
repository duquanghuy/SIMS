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
        public async Task<IActionResult> ChangePassword(StudentProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please complete all password fields correctly.";
                return RedirectToAction("Index");
            }

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            // Kiểm tra mật khẩu hiện tại
            if (!PasswordHelper.VerifyPassword(model.CurrentPassword!, user.PasswordHash))
            {
                TempData["ErrorMessage"] = "Current password is incorrect.";
                return RedirectToAction("Index");
            }

            // Cập nhật mật khẩu mới
            user.PasswordHash = PasswordHelper.HashPassword(model.NewPassword!);
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
