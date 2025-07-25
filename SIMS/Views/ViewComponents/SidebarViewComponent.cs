using Microsoft.AspNetCore.Mvc;
using SIMS.ViewModels;
using System.Security.Claims;

namespace SIMS.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var role = HttpContext.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var items = new List<SidebarItem>();

            if (role == "Admin")
            {
                items.AddRange(new[]
                    {
                        new SidebarItem { Title = "Dashboard",        IconClass = "fa-solid fa-house",           Controller = "AdminHome",      Action = "Index" },
                        new SidebarItem { Title = "Manage Students",  IconClass = "fa-solid fa-user-graduate",   Controller = "AdminStudent",  Action = "Index" },
                        new SidebarItem { Title = "Manage Teachers",  IconClass = "fa-solid fa-chalkboard",      Controller = "AdminTeacher",  Action = "Index" },
                        new SidebarItem { Title = "Manage Classes",   IconClass = "fa-solid fa-layer-group",     Controller = "AdminClass",   Action = "Index" },
                        new SidebarItem { Title = "View Scores",      IconClass = "fa-solid fa-chart-line",      Controller = "AdminViewScores",    Action = "Index" },
                        new SidebarItem { Title = "Schedule",         IconClass = "fa-solid fa-calendar",        Controller = "AdminSchedule",  Action = "Index" },
                        new SidebarItem { Title = "Manage Admins",    IconClass = "fa-solid fa-user-shield",     Controller = "AdminAdmins",    Action = "Index" }
                    });

            }
            else if (role == "Student")
            {
                items.AddRange(new[]
                {
                    new SidebarItem { Title = "Home",         IconClass = "fa-solid fa-house",         Controller = "StudentHome",        Action = "Index" },
                    new SidebarItem { Title = "Schedule",     IconClass = "fa-solid fa-calendar",      Controller = "StudentSchedule",    Action = "Index" },
                    new SidebarItem { Title = "Scores",       IconClass = "fa-solid fa-chart-bar",     Controller = "StudentScores",      Action = "Index" },
                    new SidebarItem { Title = "Info",         IconClass = "fa-solid fa-info-circle",   Controller = "StudentProfile",     Action = "Index" }
                });
            }
            else if (role == "Teacher")
            {
                items.AddRange(new[]
                {
                    new SidebarItem { Title = "Home",         IconClass = "fa-solid fa-house",          Controller = "TeacherHome",        Action = "Index" },
                    new SidebarItem { Title = "Classes",      IconClass = "fa-solid fa-folder",         Controller = "TeacherClasses",     Action = "Index" },
                    new SidebarItem { Title = "Calendar",     IconClass = "fa-solid fa-calendar-days",  Controller = "TeacherSchedule",    Action = "Index" },
                    new SidebarItem { Title = "Info",         IconClass = "fa-solid fa-user",           Controller = "TeacherProfile",     Action = "Index" }
                });
            }


            // Highlight current
            var current = ViewContext.RouteData.Values;
            var currCtrl = current["controller"]?.ToString();
            var currAct = current["action"]?.ToString();

            foreach (var item in items)
            {
                item.IsActive =
                    string.Equals(item.Controller, currCtrl, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(item.Action, currAct, StringComparison.OrdinalIgnoreCase);
            }

            return View(items);
        }
    }
}
