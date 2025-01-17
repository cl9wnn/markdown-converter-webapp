using Markdown.Abstract_classes;
namespace Markdown.Classes.Tags;
public class ItalicTag : TagElement
{
    public override string[] MdTags => ["_"];
    public override string OpenHtmlTag =>  "<em>";
    public override string CloseHtmlTag => "</em>";
    public override int MdLength => 1;
}

