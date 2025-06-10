using Microsoft.AspNetCore.Mvc;
using SIMS.ViewModels;

namespace SIMS.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Grab the current controller/action
            var rd = ViewContext.RouteData.Values;
            var ctrl = rd["controller"]?.ToString() ?? "Home";
            var act = rd["action"]?.ToString() ?? "Index";

            var items = new List<BreadcrumbItem>();

            // 1) Home always first
            //    If we're already on Home/Index, set Url=null → active
            var isHomeIndex =
                string.Equals(ctrl, "Home", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(act, "Index", StringComparison.OrdinalIgnoreCase);

            items.Add(new BreadcrumbItem
            {
                Label = "Home",
                Url = isHomeIndex
                          ? null
                          : Url.Action("Index", "Home")
            });

            // 2) If not on Home, add the controller as a link
            if (!string.Equals(ctrl, "Home", StringComparison.OrdinalIgnoreCase))
            {
                items.Add(new BreadcrumbItem
                {
                    Label = ctrl,
                    Url = Url.Action("Index", ctrl)
                });
            }

            // 3) If the action isn’t Index, append it as the current (no Url)
            if (!string.Equals(act, "Index", StringComparison.OrdinalIgnoreCase))
            {
                items.Add(new BreadcrumbItem
                {
                    Label = act,
                    Url = null
                });
            }

            return View(items);
        }
    }
}