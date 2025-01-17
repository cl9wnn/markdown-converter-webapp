namespace Markdown.Classes;

public class HtmlFileCreator
{
    private readonly string _htmlContent;
    private readonly string _filePath;

    public HtmlFileCreator(string htmlContent)
    {
        _htmlContent = htmlContent;
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "index.html");
    }

    public HtmlFileCreator(string htmlContent, string filePath)
    {
        _htmlContent = htmlContent;
        _filePath = filePath;
    }

    public void WriteToHtmlFile()
    {
        bool fileExists = File.Exists(_filePath);

        File.WriteAllText(_filePath, _htmlContent);

        if (fileExists)
        {
            Console.WriteLine("File overwritten successfully.");
        }
        else
        {
            Console.WriteLine("File created successfully.");
        }
    }
}
