using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Teacher
{
    public class TeacherHomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Home";
            return View();
        }
    }
}
