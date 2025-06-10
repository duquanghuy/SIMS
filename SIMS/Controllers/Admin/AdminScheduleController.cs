using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers.Admin
{
    public class AdminScheduleController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Schedule";
            return View();
        }
    }
}
