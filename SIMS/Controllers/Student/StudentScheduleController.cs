using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Student
{
    public class StudentScheduleController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Student Schedule";
            return View();
        }
    }
}
