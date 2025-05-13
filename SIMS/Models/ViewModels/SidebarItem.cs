namespace SIMS.ViewModels
{
    public class SidebarItem
    {
        public string Title { get; set; }
        public string IconClass { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }
    }
}