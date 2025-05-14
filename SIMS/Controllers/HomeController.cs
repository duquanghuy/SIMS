using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.Models;
using SIMS.ViewModels;

namespace SIMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Wiki(int page = 1, int pageSize = 10)
        {
            // 1) get all “API” data
            var allItems = DummyWikiData.GetAll();

            // 2) page it
            var paged = allItems
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToList<object>();        // still pass as object for your Table component

            // 3) build your WikiViewModel
            var vm = new WikiViewModel
            {
                Items = paged,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = allItems.Count
            };
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}