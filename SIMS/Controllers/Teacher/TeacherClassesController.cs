using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Teacher
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
