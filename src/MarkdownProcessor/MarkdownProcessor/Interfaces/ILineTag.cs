namespace Markdown.Interfaces;

public interface ILineTag
{ 
    string RenderLine(string content, int indentLevel);
}
