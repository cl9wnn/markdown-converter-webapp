using Markdown.Interfaces;

namespace Markdown.Classes;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly IParser<Line> _lineParser;
    private readonly IRenderer _renderer;
    private readonly IFileParser? _fileParser;


    public MarkdownProcessor(IParser<Line> parser, IRenderer renderer)
    {
        _lineParser = parser;
        _renderer = renderer;
    }
    public MarkdownProcessor(IParser<Line> parser, IRenderer renderer, IFileParser fileParser)
    {
        _lineParser = parser;
        _renderer = renderer;
        _fileParser = fileParser;

    }

    public string ConvertToHtmlFromString(string markdownText)
    {
        var lines = _lineParser.Parse(markdownText);

        return _renderer.Render(lines);
    }

    public string ConvertToHtmlFromFile(string filePath, IFileParser fileParser)
    {
        var text = fileParser.Parse(filePath);

        var lines = _lineParser.Parse(text);

        return _renderer.Render(lines);
    }


}
    

