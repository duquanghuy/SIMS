using Microsoft.AspNetCore.Mvc;
using SIMS.ViewModels;

namespace SIMS.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
    int currentPage,
    int pageSize,
    int totalItems,
    string action,
    string controller,
    IDictionary<string, string>? routeValues = null)
        {
            var model = new PaginationModel
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                Action = action,
                Controller = controller,
                RouteValues = routeValues
            };
            return View(model);
        }

    }
}
