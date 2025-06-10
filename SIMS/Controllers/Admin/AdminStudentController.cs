using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Admin
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
