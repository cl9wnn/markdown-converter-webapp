namespace Markdown.Abstract_classes;
public abstract class TagElement
{
    public abstract string[] MdTags { get; }
    public abstract string OpenHtmlTag { get; }
    public abstract string CloseHtmlTag { get; }
    public abstract int MdLength { get; }
}
