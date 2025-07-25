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
    public class TeacherScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TeacherScheduleController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            // Find the teacher
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null) return NotFound();

            // Get all class IDs the teacher is teaching
            var classIds = await _context.Classes
                .Where(c => c.TeacherId == teacher.TeacherId)
                .Select(c => c.ClassId)
                .ToListAsync();

            // Get all schedules for those classes
            var schedules = await _context.ClassSchedules
                .Include(cs => cs.Class).ThenInclude(c => c.Subject)
                .Include(cs => cs.Class).ThenInclude(c => c.Teacher).ThenInclude(t => t.User)
                .Include(cs => cs.Class).ThenInclude(c => c.Enrollments).ThenInclude(e => e.Student).ThenInclude(s => s.User)
                .Where(cs => classIds.Contains(cs.ClassId))
                .ToListAsync();

            var vm = new TeacherScheduleViewModel();
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
