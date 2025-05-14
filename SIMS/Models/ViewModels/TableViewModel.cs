namespace SIMS.ViewModels
{
    public class TableViewModel
    {
        public IEnumerable<object> Items { get; set; } = Array.Empty<object>();
        public List<TableColumn> Columns { get; set; } = new();
    }
}