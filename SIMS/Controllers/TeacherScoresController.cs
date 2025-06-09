using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class TeacherScoresController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Scores";
            return View();
        }
    }
}
