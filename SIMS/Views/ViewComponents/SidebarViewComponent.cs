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
                        new SidebarItem { Title = "Manage Students",  IconClass = "fa-solid fa-user-graduate",   Controller = "AdminStudents",  Action = "Index" },
                        new SidebarItem { Title = "Manage Teachers",  IconClass = "fa-solid fa-chalkboard",      Controller = "AdminTeachers",  Action = "Index" },
                        new SidebarItem { Title = "Manage Classes",   IconClass = "fa-solid fa-layer-group",     Controller = "AdminClasses",   Action = "Index" },
                        new SidebarItem { Title = "View Scores",      IconClass = "fa-solid fa-chart-line",      Controller = "AdminScores",    Action = "Index" },
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
                    new SidebarItem { Title = "Assignments",  IconClass = "fa-solid fa-book",          Controller = "StudentAssignments", Action = "Index" },
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
                    new SidebarItem { Title = "Student",      IconClass = "fa-solid fa-id-card",        Controller = "TeacherStudents",    Action = "List" },
                    new SidebarItem { Title = "Assignments",  IconClass = "fa-solid fa-clipboard",      Controller = "TeacherAssignments", Action = "Index" },
                    new SidebarItem { Title = "Scores",       IconClass = "fa-solid fa-chart-line",     Controller = "TeacherScores",      Action = "Index" },
                    new SidebarItem { Title = "Attendance",   IconClass = "fa-solid fa-clock",          Controller = "TeacherAttendance",  Action = "Index" },
                    new SidebarItem { Title = "Calendar",     IconClass = "fa-solid fa-calendar-days",  Controller = "TeacherSchedule",    Action = "Index" },
                    new SidebarItem { Title = "Info",         IconClass = "fa-solid fa-user",           Controller = "TeacherProfile",     Action = "Index" }
                });
            }

            // Shared for all roles
            items.Add(new SidebarItem { Title = "Wiki", IconClass = "fa-brands fa-wikipedia-w", Controller = "Home", Action = "Wiki" });
            items.Add(new SidebarItem { Title = "Settings", IconClass = "fa-solid fa-cogs", Controller = "Settings", Action = "Index" });

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
