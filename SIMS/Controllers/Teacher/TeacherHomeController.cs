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
    public class TeacherHomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TeacherHomeController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
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
                .Include(c => c.ClassSchedules)
                .Include(c => c.Enrollments)
                .ToListAsync();

            // Get today's schedules for these classes
            var today = DateTime.Today.DayOfWeek.ToString();
            var schedules = await _context.ClassSchedules
                .Include(cs => cs.Class).ThenInclude(c => c.Subject)
                .Where(cs => classes.Select(c => c.ClassId).Contains(cs.ClassId) && cs.DayOfWeek == today)
                .ToListAsync();

            var vm = new TeacherHomeViewModel();

            // Today's classes
            foreach (var sched in schedules)
            {
                vm.TodaysClasses.Add(new TeacherTodaysClassInfo
                {
                    ClassCode = sched.Class?.Subject?.Code,
                    ClassName = sched.Class?.Subject?.Title,
                    TimeSlot = sched.TimeSlot
                });
            }

            // All classes table
            foreach (var cls in classes)
            {
                var enrollments = cls.Enrollments;
                var numericScores = enrollments
                    .Select(e => double.TryParse(e.Grade, out var score) ? (double?)score : null)
                    .Where(s => s.HasValue)
                    .Select(s => s.Value)
                    .ToList();
                vm.AllClasses.Add(new TeacherClassInfo
                {
                    ClassCode = cls.Subject?.Code,
                    ClassName = cls.Subject?.Title,
                    StudentCount = enrollments.Count,
                    AverageScore = numericScores.Any() ? numericScores.Average() : 0.0
                });
            }

            // Overall average score
            var allScores = vm.AllClasses.SelectMany(c => c.StudentCount > 0 ? new[] { c.AverageScore } : Array.Empty<double>()).ToList();
            vm.OverallAverageScore = allScores.Any() ? allScores.Average() : 0.0;

            return View(vm);
        }
    }
}
