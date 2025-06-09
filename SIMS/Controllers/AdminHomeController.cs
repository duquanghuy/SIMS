using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            return View();
        }
    }
}
