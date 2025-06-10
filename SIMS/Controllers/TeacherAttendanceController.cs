using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class TeacherAttendanceController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Attendance";
            return View();
        }
    }
}
