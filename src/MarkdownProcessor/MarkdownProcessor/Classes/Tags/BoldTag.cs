using Markdown.Abstract_classes;
namespace Markdown.Classes.Tags;

public class BoldTag : TagElement
{
    public override string[] MdTags => ["__", "**"];
    public override string OpenHtmlTag => "<strong>";
    public override string CloseHtmlTag => "</strong>";
    public override int MdLength => 2;
}
