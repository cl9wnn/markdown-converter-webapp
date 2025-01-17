using Markdown.Abstract_classes;
using Markdown.Interfaces;
namespace Markdown.Classes.Tags
{
    public class HeaderTag : TagElement, ILineTag
    {
        public override string[] MdTags => ["#"];
        public override string OpenHtmlTag => "<h1>";
        public override string CloseHtmlTag => "</h1>";
        public override int MdLength => 1;

        public string RenderLine(string line, int indentLevel)
        {
            return $"{OpenHtmlTag}{line}{CloseHtmlTag}";
        }
    }
}
