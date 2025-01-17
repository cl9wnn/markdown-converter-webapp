using Markdown.Abstract_classes;
using Markdown.Interfaces;

namespace Markdown.Classes.TagFactory;

public class DoubleTagFactory : ITagFactory
{
    private readonly IEnumerable<TagElement> _tags;

    public DoubleTagFactory(IEnumerable<TagElement> tags)
    {
        _tags = tags;
    }

    public TagElement? GetTag(string symbol)
    {
        foreach (var tag in _tags)
        {
            if (tag is not ILineTag && tag.MdTags.Contains(symbol))
            {
                return tag;
            }
        }
        return null;
    }
}
