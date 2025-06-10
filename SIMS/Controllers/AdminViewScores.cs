using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class AdminViewScoresController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "View Scores";
            return View();
        }
    }
}
