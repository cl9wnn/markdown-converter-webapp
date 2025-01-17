using Markdown.Abstract_classes;
namespace Markdown.Interfaces;

public interface ITagFactory
{
    TagElement? GetTag(string token);
}
