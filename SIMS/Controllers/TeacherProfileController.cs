using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
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
