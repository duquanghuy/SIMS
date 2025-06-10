using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class TeacherClassesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Classes";
            return View();
        }
    }
}
