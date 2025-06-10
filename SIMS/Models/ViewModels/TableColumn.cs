namespace SIMS.ViewModels
{
    public class TableColumn
    {
        /// <summary>
        /// The header text to display in &lt;th&gt;
        /// </summary>
        public string Header { get; set; } = "";

        /// <summary>
        /// The property name on your item to render (via reflection)
        /// </summary>
        public string PropertyName { get; set; } = "";
    }
}