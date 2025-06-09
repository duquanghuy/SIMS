using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class TeacherAssignmentsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Teacher Assignments";
            return View();
        }
    }
}
    