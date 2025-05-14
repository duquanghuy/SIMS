namespace SIMS.ViewModels
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public string Action { get; set; } = "";
        public string Controller { get; set; } = "";
        public IDictionary<string, string>? RouteValues { get; set; }
    }
}
