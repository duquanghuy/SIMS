using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class StudentScoresController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Student Scores";
            return View();
        }
    }
}
