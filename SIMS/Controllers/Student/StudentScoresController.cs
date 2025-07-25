using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SIMS.Controllers.Student
{
    public class StudentScoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentScoresController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            // Find the student
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound();

            // Get all enrollments for this student
            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == student.StudentId)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Subject)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .ToListAsync();

            var vm = new StudentScoreViewModel();
            foreach (var enrollment in enrollments)
            {
                var cls = enrollment.Class;
                var teacher = cls?.Teacher;
                var teacherUser = teacher?.User;
                vm.Scores.Add(new StudentScoreRow
                {
                    ClassName = cls?.Subject?.Code + " - " + cls?.Subject?.Title,
                    TeacherName = teacher?.FirstName + " " + teacher?.LastName,
                    TeacherEmail = teacherUser?.Email ?? "N/A",
                    Score = enrollment.Grade ?? "N/A"
                });
            }

            return View(vm);
        }
    }
}
