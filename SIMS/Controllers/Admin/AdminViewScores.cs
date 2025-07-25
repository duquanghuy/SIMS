using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SIMS.Controllers.Admin
{
    public class AdminViewScoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminViewScoresController(ApplicationDbContext context) => _context = context;

        // GET: Admin/AdminViewScores
        public async Task<IActionResult> Index(int? classId)
        {
            var vm = new AdminViewScoresViewModel();

            // Populate class dropdown
            vm.Classes = await _context.Classes
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.Subject.Code + " - " + c.Teacher.FirstName + " " + c.Teacher.LastName
                })
                .ToListAsync();

            vm.SelectedClassId = classId;

            if (classId.HasValue)
            {
                // Get students in the selected class
                var students = await _context.Enrollments
                    .Where(e => e.ClassId == classId.Value)
                    .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                    .ToListAsync();

                // For each student, get their info and score
                foreach (var enrollment in students)
                {
                    var student = enrollment.Student;
                    var user = student.User;
                    // Assume score is in Enrollment.Grade (adjust if needed)
                    vm.Students.Add(new StudentScoreInfo
                    {
                        StudentName = student.FirstName + " " + student.LastName,
                        Email = user?.Email ?? "",
                        Phone = string.IsNullOrEmpty(student.PhoneNumber) ? "N/A" : student.PhoneNumber,
                        Score = enrollment.Grade ?? "N/A"
                    });
                }
            }

            return View(vm);
        }
    }
}
