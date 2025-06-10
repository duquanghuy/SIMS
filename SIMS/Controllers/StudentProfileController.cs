using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class StudentProfileController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Student Profile";
            return View();
        }
    }
}
