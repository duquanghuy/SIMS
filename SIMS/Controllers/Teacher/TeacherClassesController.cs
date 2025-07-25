using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SIMS.Controllers.Teacher
{
    public class TeacherClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TeacherClassesController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(int? classId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            // Find the teacher
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null) return NotFound();

            // Get all classes assigned to this teacher
            var classes = await _context.Classes
                .Where(c => c.TeacherId == teacher.TeacherId)
                .Include(c => c.Subject)
                .ToListAsync();

            var vm = new TeacherClassesViewModel();
            vm.Classes = classes.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.ClassId.ToString(),
                Text = c.Subject.Code
            }).ToList();
            vm.SelectedClassId = classId;

            if (classId.HasValue)
            {
                // Get all students in the selected class
                var enrollments = await _context.Enrollments
                    .Where(e => e.ClassId == classId.Value)
                    .Include(e => e.Student)
                        .ThenInclude(s => s.User)
                    .ToListAsync();

                foreach (var enrollment in enrollments)
                {
                    var student = enrollment.Student;
                    var user = student.User;
                    vm.Students.Add(new StudentInClassInfo
                    {
                        StudentCode = student.StudentNumber.ToString(),
                        Name = student.FirstName + " " + student.LastName,
                        Email = user?.Email ?? "N/A",
                        Phone = string.IsNullOrEmpty(student.PhoneNumber) ? "N/A" : student.PhoneNumber,
                        Score = enrollment.Grade ?? "N/A"
                    });
                }

                // Calculate average score
                var numericScores = enrollments
                    .Select(e => double.TryParse(e.Grade, out var score) ? (double?)score : null)
                    .Where(s => s.HasValue)
                    .Select(s => s.Value)
                    .ToList();
                ViewBag.AverageScore = numericScores.Any() ? numericScores.Average() : 0.0;
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateScore(int classId, string studentCode, string score)
        {
            // Find the enrollment
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.ClassId == classId && e.Student.StudentNumber.ToString() == studentCode);
            if (enrollment == null)
            {
                TempData["Error"] = "Enrollment not found.";
                return RedirectToAction("Index", new { classId });
            }

            // Validate score
            if (!int.TryParse(score, out var numericScore) || numericScore < 0 || numericScore > 100)
            {
                TempData["Error"] = "Score must be a number between 0 and 100.";
                return RedirectToAction("Index", new { classId });
            }

            enrollment.Grade = numericScore.ToString();
            await _context.SaveChangesAsync();
            TempData["Success"] = "Score updated.";
            return RedirectToAction("Index", new { classId });
        }
    }
}
