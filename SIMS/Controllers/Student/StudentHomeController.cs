using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Student
{
    public class StudentHomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Student Home";
            return View();
        }
    }
}
