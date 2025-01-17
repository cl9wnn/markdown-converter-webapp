using Markdown.Abstract_classes;
using Markdown.Interfaces;
namespace Markdown.Classes.Tags;

public class MarkedListTag : TagElement, ILineTag
{
    public override string[] MdTags => ["*", "-", "+"];
    public override string OpenHtmlTag => "<li>";
    public override string CloseHtmlTag => "</li>";
    public override int MdLength => 1;

    public string RenderLine(string line, int indentLevel)
    {
        int spaceCount = 4;

        string indentString = new string(' ', spaceCount + indentLevel * 2 * spaceCount);

        return  indentString +$"{OpenHtmlTag}{line}{CloseHtmlTag}";
    }

}
