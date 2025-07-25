using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;


namespace SIMS.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminScheduleController(ApplicationDbContext ctx) => _context = ctx;

        // GET: Admin/AdminSchedule
        public async Task<IActionResult> Index(int weekOffset = 0)
        {
            try
            {
                var vm = new CalendarViewModel { WeekOffset = weekOffset };

                // 1) Figure out which days of the week we care about
                var today = DateTime.Today;
                var startOfWeek = today.AddDays(-((int)today.DayOfWeek))
                                      .AddDays(7 * weekOffset);
                var days = Enumerable.Range(0, 7)
                                     .Select(i => startOfWeek.AddDays(i).DayOfWeek)
                                     .ToList();

                // 2) Load any existing schedules for those days
                var schedules = await _context.ClassSchedules
                    .Include(cs => cs.Class).ThenInclude(c => c.Subject)
                    .Include(cs => cs.Class).ThenInclude(c => c.Teacher)
                    .Where(cs => days.Select(d => d.ToString()).Contains(cs.DayOfWeek))
                    .ToListAsync();

                // 3) Merge into the pre-initialized slots
                foreach (var sched in schedules)
                {
                    if (Enum.TryParse<DayOfWeek>(sched.DayOfWeek, out var d)
                        && vm.Slots.ContainsKey(d)
                        && vm.Slots[d].ContainsKey(sched.TimeSlot))
                    {
                        vm.Slots[d][sched.TimeSlot].Add(sched);
                    }
                }

                // 4) Build the “all classes” dropdown
                var allClasses = await _context.Classes
                    .Include(c => c.Subject)
                    .Include(c => c.Teacher)
                    .Select(c => new {
                        c.ClassId,
                        Display = $"{c.Subject.Code} – {c.Teacher.FirstName} {c.Teacher.LastName}"
                    })
                    .ToListAsync();
                vm.AllClasses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(allClasses, "ClassId", "Display");

                return View(vm);
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int SelectedClassId, DayOfWeek SelectedDay, string SelectedTimeSlot, int weekOffset)
        {
            // Check if a schedule already exists for this day and slot
            var dayString = SelectedDay.ToString();
            var exists = await _context.ClassSchedules.AnyAsync(cs =>
                cs.DayOfWeek == dayString &&
                cs.TimeSlot == SelectedTimeSlot &&
                cs.ClassId == SelectedClassId);
            if (exists)
            {
                // Optionally, add a message to TempData for feedback
                TempData["Error"] = $"This class is already scheduled for {dayString} {SelectedTimeSlot}.";
                return RedirectToAction("Index", new { weekOffset });
            }

            // Create and save the new schedule
            var sched = new SIMS.Models.ClassSchedule
            {
                ClassId = SelectedClassId,
                DayOfWeek = dayString,
                TimeSlot = SelectedTimeSlot
            };
            _context.ClassSchedules.Add(sched);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { weekOffset });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unassign(int scheduleId, int weekOffset)
        {
            var sched = await _context.ClassSchedules.FindAsync(scheduleId);
            if (sched != null)
            {
                _context.ClassSchedules.Remove(sched);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { weekOffset });
        }
    }

}

