using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Teacher
{
    public class TeacherScheduleController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Schedule";
            return View();
        }
    }
}
