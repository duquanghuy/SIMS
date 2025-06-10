using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class AdminStudentController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Manage Students";
            return View();
        }
    }
}
