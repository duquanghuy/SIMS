using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Teacher
{
    public class TeacherProfileController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Profile";
            return View();
        }
    }
}
