using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class AdminTeacherController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Manage Teachers";
            return View();
        }
    }
}
