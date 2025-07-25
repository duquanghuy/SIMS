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
    public class StudentScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentScheduleController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            // Find the student
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound();

            // Get all class IDs the student is enrolled in
            var classIds = await _context.Enrollments
                .Where(e => e.StudentId == student.StudentId)
                .Select(e => e.ClassId)
                .ToListAsync();

            // Get all schedules for those classes
            var schedules = await _context.ClassSchedules
                .Include(cs => cs.Class).ThenInclude(c => c.Subject)
                .Include(cs => cs.Class).ThenInclude(c => c.Teacher).ThenInclude(t => t.User)
                .Include(cs => cs.Class).ThenInclude(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                        .ThenInclude(s => s.User)
                .Where(cs => classIds.Contains(cs.ClassId))
                .ToListAsync();

            var vm = new StudentScheduleViewModel();
            foreach (var sched in schedules)
            {
                if (Enum.TryParse<DayOfWeek>(sched.DayOfWeek, out var d)
                    && vm.Slots.ContainsKey(d)
                    && vm.Slots[d].ContainsKey(sched.TimeSlot))
                {
                    vm.Slots[d][sched.TimeSlot].Add(sched);
                }
            }

            return View(vm);
        }
    }
}
