using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Teacher
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
    