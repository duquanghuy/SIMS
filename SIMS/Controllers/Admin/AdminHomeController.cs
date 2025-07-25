using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
using SIMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMS.Controllers.Admin
{
    public class AdminHomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminHomeController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var vm = new AdminDashboardViewModel();

            // Statistic cards
            vm.TotalStudents = await _context.Students.CountAsync();
            vm.TotalTeachers = await _context.Teachers.CountAsync();
            vm.TotalClasses = await _context.Classes.CountAsync();
            vm.TotalSubjects = await _context.Subjects.CountAsync();
            vm.ActiveEnrollments = await _context.Enrollments.CountAsync();

            // Average score (school-wide)
            var allGrades = await _context.Enrollments
                .Where(e => e.Grade != null)
                .Select(e => e.Grade)
                .ToListAsync();
            var allScores = allGrades
                .Select(g => double.TryParse(g, out var score) ? (double?)score : null)
                .Where(s => s.HasValue)
                .Select(s => s.Value)
                .ToList();
            vm.AverageScore = allScores.Any() ? allScores.Average() : 0.0;

            // Students per class
            vm.StudentsPerClass = await _context.Classes
                .Include(c => c.Subject)
                .Select(c => new ClassCount
                {
                    ClassLabel = c.Subject.Code,
                    Count = c.Enrollments.Count
                }).ToListAsync();

            // Students by subject
            vm.StudentsBySubject = await _context.Subjects
                .Select(s => new SubjectCount
                {
                    SubjectLabel = s.Code,
                    Count = s.Classes.SelectMany(c => c.Enrollments).Count()
                }).ToListAsync();

            // Enrollment trends (by day)
            vm.EnrollmentTrends = await _context.Enrollments
                .GroupBy(e => e.EnrolledOn.Date)
                .OrderBy(g => g.Key)
                .Select(g => new EnrollmentTrend
                {
                    Date = g.Key,
                    Count = g.Count()
                }).ToListAsync();

            // Average score per class
            var classList = await _context.Classes
                .Include(c => c.Subject)
                .Include(c => c.Enrollments)
                .ToListAsync();
            vm.AverageScorePerClass = classList
                .Select(c => new ClassScore
                {
                    ClassLabel = c.Subject.Code,
                    AverageScore = c.Enrollments
                        .Select(e => double.TryParse(e.Grade, out var score) ? (double?)score : null)
                        .Where(s => s.HasValue)
                        .Select(s => s.Value)
                        .DefaultIfEmpty(0).Average()
                }).ToList();

            return View(vm);
        }
    }
}
