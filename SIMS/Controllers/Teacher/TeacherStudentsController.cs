using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Teacher
{
    public class TeacherStudentsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Students";
            return View();
        }
    }
}
