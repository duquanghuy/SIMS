using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
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
