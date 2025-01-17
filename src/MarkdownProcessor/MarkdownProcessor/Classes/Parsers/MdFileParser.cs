using Markdown.Interfaces;

namespace Markdown.Classes.Parsers;

public class MdFileParser : IFileParser
{
    public string Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File Not Found: {filePath}");
        }

        if (Path.GetExtension(filePath).ToLower() != ".md")
        {
            throw new ArgumentException("The file must have a .md extension.");
        }

        string[] lines = File.ReadAllLines(filePath);

        return string.Join("\n", lines);
    }
}
