using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Admin
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
