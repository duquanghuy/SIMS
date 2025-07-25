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
    public class StudentHomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentHomeController(ApplicationDbContext context) => _context = context;

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

            // Get all class IDs
            var classIds = enrollments.Select(e => e.ClassId).ToList();

            // Get today's schedules for these classes
            var today = DateTime.Today.DayOfWeek.ToString();
            var schedules = await _context.ClassSchedules
                .Include(cs => cs.Class).ThenInclude(c => c.Subject)
                .Where(cs => classIds.Contains(cs.ClassId) && cs.DayOfWeek == today)
                .ToListAsync();

            var vm = new StudentHomeViewModel();

            // Today's classes
            foreach (var sched in schedules)
            {
                vm.TodaysClasses.Add(new TodaysClassInfo
                {
                    ClassName = sched.Class?.Subject?.Code + " - " + sched.Class?.Subject?.Title,
                    TimeSlot = sched.TimeSlot
                });
            }

            // All classes table
            foreach (var enrollment in enrollments)
            {
                var cls = enrollment.Class;
                var teacher = cls?.Teacher;
                var teacherUser = teacher?.User;
                vm.AllClasses.Add(new ClassScoreInfo
                {
                    ClassName = cls?.Subject?.Code + " - " + cls?.Subject?.Title,
                    TeacherName = teacher?.FirstName + " " + teacher?.LastName,
                    TeacherEmail = teacherUser?.Email ?? "N/A",
                    Score = enrollment.Grade ?? "N/A"
                });
            }

            // GPA calculation (average of numeric scores)
            var numericScores = enrollments
                .Select(e => double.TryParse(e.Grade, out var score) ? (double?)score : null)
                .Where(s => s.HasValue)
                .Select(s => s.Value)
                .ToList();
            vm.GPA = numericScores.Any() ? numericScores.Average() : 0.0;

            return View(vm);
        }
    }
}
