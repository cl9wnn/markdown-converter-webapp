using Markdown.Abstract_classes;
using Markdown.Classes.Parsers;
using Markdown.Classes.Renderers;
using Markdown.Classes.TagFactory;
using Markdown.Classes.Tags;

namespace Markdown.Classes;

public class Program
{ 
    public static void Main(string[] args)
    {
        string input = "_text_";
        IEnumerable<TagElement> tags = [new HeaderTag(), new BoldTag(), new ItalicTag(), new MarkedListTag()];

        var singleTagFactory = new SingleTagFactory(tags);
        var doubleTagFactory = new DoubleTagFactory(tags);

        var lineRenderer = new LineRenderer();
        var listRenderer = new ListRenderer();

        var tokenParser = new TokenParser(doubleTagFactory);
        var lineParser = new LineParser(tokenParser, singleTagFactory);
        var renderer = new HtmlRenderer(tags, lineRenderer, listRenderer);

        var processor = new MarkdownProcessor(lineParser, renderer);

        string result = processor.ConvertToHtmlFromString(input);

        Console.WriteLine(result); 
    }
}
