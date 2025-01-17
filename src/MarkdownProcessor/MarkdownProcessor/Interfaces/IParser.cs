namespace Markdown.Interfaces;

public interface IParser<out T>
{
    IEnumerable<T> Parse(string input);
}
