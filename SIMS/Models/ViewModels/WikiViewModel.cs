namespace SIMS.ViewModels
{
    public class WikiViewModel
    {
        public IEnumerable<object> Items { get; set; } = Array.Empty<object>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
