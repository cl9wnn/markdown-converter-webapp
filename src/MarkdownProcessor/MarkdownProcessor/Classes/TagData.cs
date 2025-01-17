using Markdown.Abstract_classes;
namespace Markdown.Classes;

public class TagData
{
    public TagElement? Tag { get; set; }
    public int Index { get; set; }
    public bool IsClosing { get; set; }

    public TagData(TagElement? tag, int index, bool isClosing = false)
    {
        Tag = tag;
        Index = index;
        IsClosing = isClosing;
    }
}