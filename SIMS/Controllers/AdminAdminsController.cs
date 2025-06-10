using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class AdminAdminsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Manage Admins";
            return View();
        }
    }
}
