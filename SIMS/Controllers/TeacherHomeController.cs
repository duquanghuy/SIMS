using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
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
