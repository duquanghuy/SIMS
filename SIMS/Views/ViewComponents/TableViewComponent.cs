using Microsoft.AspNetCore.Mvc;
using SIMS.ViewModels;

namespace SIMS.ViewComponents
{
    public class TableViewComponent : ViewComponent
    {
        /// <summary>
        /// Renders a table for the given items and columns, but only up to 10 rows.
        /// </summary>
        public IViewComponentResult Invoke(
            IEnumerable<object> items,
            IEnumerable<TableColumn> columns)
        {
            // only take the first 10 items
            var limited = items.Take(10);

            var model = new TableViewModel
            {
                Items = limited,
                Columns = columns.ToList()
            };
            return View(model);
        }
    }
}