using Markdown.Classes.Tags;

namespace Markdown.Classes.Renderers;

public class ListRenderer
{
    private bool _isListOpen = false;
    private int _currentLevel = 0;

    public string RenderList(Line line)
    {
        var renderedLines = string.Empty;
        var indentLevel = line.IndentLevel;

        if (line.Type is MarkedListTag && !_isListOpen)
        {
            _isListOpen = true;
            renderedLines = "<ul>";
        }

        if (indentLevel > _currentLevel)
        {
            renderedLines += GetIndent(indentLevel) + "<ul>";
            _currentLevel = indentLevel;
        }
        else if (indentLevel < _currentLevel)
        {
            renderedLines += GetIndent(_currentLevel) + "</ul>";
            _currentLevel = indentLevel;
        }

        if (line.Type is not MarkedListTag && _isListOpen)
        {
            _isListOpen = false;
            renderedLines += "\n</ul>";
        }

        return renderedLines;
    }

    public string CloseOpenList()
    {
        return _isListOpen ? "</ul>" : string.Empty;
    }

    private string GetIndent(int level)
    {
        return new string(' ', level * 8);
    }
}