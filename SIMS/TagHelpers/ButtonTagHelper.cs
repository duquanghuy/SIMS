using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SIMS.TagHelpers
{
    [HtmlTargetElement("button", Attributes = "asp-btn")]
    public class ButtonTagHelper : TagHelper
    {
        public string AspBtn { get; set; } = "primary";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var existing = output.Attributes.TryGetAttribute("class", out var cls)
                             ? cls.Value?.ToString()
                             : string.Empty;

            var @class = $"{existing} btn-common btn-{AspBtn}".Trim();
            output.Attributes.SetAttribute("class", @class);
        }
    }
}