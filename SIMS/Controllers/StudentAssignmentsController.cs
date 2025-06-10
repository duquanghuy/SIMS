using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
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
