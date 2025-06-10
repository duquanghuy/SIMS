using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Teacher
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
