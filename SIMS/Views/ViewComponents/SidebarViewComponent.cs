using Microsoft.AspNetCore.Mvc;
using SIMS.ViewModels;

namespace SIMS.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Build the menu here
            var items = new List<SidebarItem>
            {
                new() { Title="Home",    IconClass="fa-solid fa-house",        Controller="Home", Action="Index" },
                new() { Title="Privacy", IconClass="fa-solid fa-book",         Controller="Home", Action="Privacy" },
                new() { Title="Wiki",    IconClass="fa-brands fa-wikipedia-w", Controller="Home", Action="Wiki" }
            };

            // Determine current route to mark active link
            var current = ViewContext.RouteData.Values;
            var currCtrl = current["controller"]?.ToString();
            var currAct = current["action"]?.ToString();

            foreach (var item in items)
                item.IsActive =
                    string.Equals(item.Controller, currCtrl, StringComparison.OrdinalIgnoreCase)
                 && string.Equals(item.Action, currAct, StringComparison.OrdinalIgnoreCase);

            return View(items);
        }
    }
}