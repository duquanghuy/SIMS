using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Student
{
    public class StudentAssignmentsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Student Assignments";
            return View();
        }
    }
}
