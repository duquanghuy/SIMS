using Microsoft.AspNetCore.Mvc;

namespace SIMS.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}