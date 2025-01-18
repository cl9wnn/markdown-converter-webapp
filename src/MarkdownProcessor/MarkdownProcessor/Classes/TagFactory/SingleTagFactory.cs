using Markdown.Abstract_classes;
using Markdown.Classes.Tags;
using Markdown.Interfaces;

namespace Markdown.Classes.TagFactory;

public class SingleTagFactory 
{
    private readonly IEnumerable<TagElement> _tags;

    public SingleTagFactory(IEnumerable<TagElement> tags)
    {
        _tags = tags;
    }

    public TagElement? GetTag(string line)
    {
        var trimmedLine = line.TrimStart();

        if (trimmedLine.Length == 0)
            return new ParagraphTag();
        
        var symbol = trimmedLine[0].ToString();

        foreach (var tag in _tags)
        {
            if (tag is ILineTag && tag.MdTags.Contains(symbol) && char.IsWhiteSpace(trimmedLine[1]))
            {
                return tag;
            }
        }

        return new ParagraphTag();
    }
}
