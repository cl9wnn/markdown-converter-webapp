using Markdown.Interfaces;

namespace Markdown.Classes;

public class Line
{
    public IEnumerable<Token> Tokens { get; set; }
    public ILineTag Type { get; set; } 
    public int IndentLevel { get; }

    public Line(IEnumerable<Token> tokens, ILineTag type, int indentLevel)
    {
        Tokens = tokens;
        Type = type;
        IndentLevel = indentLevel;
    }
}