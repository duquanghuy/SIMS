using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
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
