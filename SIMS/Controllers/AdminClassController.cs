using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class AdminClassController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Manage Classes";
            return View();
        }
    }
}
