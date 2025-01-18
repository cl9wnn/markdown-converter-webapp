using Markdown.Abstract_classes;
using Markdown.Classes;
using Markdown.Classes.Parsers;
using Markdown.Classes.Renderers;
using Markdown.Classes.TagFactory;
using Markdown.Classes.Tags;
using Markdown.Interfaces;

namespace MarkdownTests;

public class MdProcessorTests
{

    private readonly IMarkdownProcessor _processor;

    public MdProcessorTests()
    {
        IEnumerable<TagElement> tags = [new HeaderTag(), new BoldTag(), new ItalicTag(), new MarkedListTag()];

        var doubleTagFactory = new DoubleTagFactory(tags);
        var singleTagFactory = new SingleTagFactory(tags);

        var lineRenderer = new LineRenderer();
        var listRenderer = new ListRenderer();

        var tokenParser = new TokenParser(doubleTagFactory);
        var lineParser = new LineParser(tokenParser, singleTagFactory);
        var renderer = new HtmlRenderer(tags, lineRenderer, listRenderer);

        _processor = new MarkdownProcessor(lineParser, renderer);
    }


    [Theory]
    [InlineData("__text__", "<p><strong>text</strong></p>")]
    [InlineData("__bold text__", "<p><strong>bold text</strong></p>")]
    [InlineData("__bold and default text__", "<p><strong>bold and default text</strong></p>")]

    public void DoubleUnderscore_ShouldConvertToStrong(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("_text_", "<p><em>text</em></p>")]
    [InlineData("_italic text_", "<p><em>italic text</em></p>")]
    [InlineData("_italic and default text_", "<p><em>italic and default text</em></p>")]
    public void SingleUnderscore_ShouldConvertToEmphasis(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(@"\# Header", "<p># Header</p>")]
    [InlineData(@"\* List", "<p>* List</p>")]
    [InlineData(@"\_text\_", "<p>_text_</p>")]
    [InlineData(@"\_\_text\_\_", "<p>__text__</p>")]
    public void EscapedTags_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(@"te\xt", @"<p>te\xt</p>")]
    [InlineData(@"te\xt \with escaping\", @"<p>te\xt \with escaping\</p>")]
    public void Escaping_ShouldWorkOnlyWithTags(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(@"\\_text_", @"<p>\<em>text</em></p>")]
    [InlineData(@"\\__text__", @"<p>\<strong>text</strong></p>")]
    public void EscapingSymbol_ShouldScreenEscapingSymbol(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("__emphasis _text_ convert__", "<p><strong>emphasis <em>text</em> convert</strong></p>")]
    [InlineData("__emphasis _text another text_ convert__", "<p><strong>emphasis <em>text another text</em> convert</strong></p>")]

    public void NestedEmphasisTag_ShouldConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("_bold __text__ doesnt convert_", "<p><em>bold __text__ doesnt convert</em></p>")]
    [InlineData("_bold __text another text__ doesnt convert_", "<p><em>bold __text another text__ doesnt convert</em></p>")]

    public void NestedStrongTag_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("text_12_3", "<p>text_12_3</p>")]
    [InlineData("text_123_example", "<p>text_123_example</p>")]
    [InlineData("__text123__", "<p><strong>text123</strong></p>")]
    [InlineData("_Element 1_", "<p><em>Element 1</em></p>")]
    public void TagsInsideWordsWithNumbers_ShouldNotConvert(string input, string expected)
    { 
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("_sta_rt", "<p><em>sta</em>rt</p>")]
    [InlineData("cen__te__r", "<p>cen<strong>te</strong>r</p>")]
    [InlineData("en_d_", "<p>en<em>d</em></p>")]
    [InlineData("_sta_rt_exa_mple", "<p><em>sta</em>rt<em>exa</em>mple</p>")]
    [InlineData("_sta_rt_exa_mple_", "<p><em>sta</em>rt<em>exa</em>mple_</p>")]
    public void TagsInsideWords_ShouldConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("diff_erent wo_rd", "<p>diff_erent wo_rd</p>")]
    [InlineData("diff__erent wo__rd", "<p>diff__erent wo__rd</p>")]
    [InlineData("diff_erent word_", "<p>diff_erent word_</p>")]
    public void TagsInsideWordsInDifferentWords_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("__word_", "<p>__word_</p>")]
    [InlineData("_word__", "<p>_word__</p>")]
    [InlineData("__Dont convert_", "<p>__Dont convert_</p>")]
    [InlineData("_Dont convert__", "<p>_Dont convert__</p>")]

    public void UnmatchedSymbols_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("example_ text_", "<p>example_ text_</p>")]
    [InlineData("example__ long line bold text__", "<p>example__ long line bold text__</p>")]

    public void NoWhitespaceAfterTag_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("_example _text", "<p>_example _text</p>")]
    [InlineData("_example long line bold _text", "<p>_example long line bold _text</p>")]
    public void NoWhitespaceBeforeTag_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("__bold _text__ and emphasis_", "<p>__bold _text__ and emphasis_</p>")]
    [InlineData("_bold __text_ and emphasis__", "<p>_bold __text_ and emphasis__</p>")]
    [InlineData("_bold __text and_ emphasis__", "<p>_bold __text and_ emphasis__</p>")]

    public void CrossingUnderscores_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("____", "<p>____</p>")]
    [InlineData("__ __", "<p>__ __</p>")]
    [InlineData("__", "<p>__</p>")]
    [InlineData("text and __ another _text_", "<p>text and __ another <em>text</em></p>")]
    public void EmptyStringBetweenTags_ShouldRemainAsUnderscores(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("#Header text", "<p>#Header text</p>")]
    [InlineData("Header #text", "<p>Header #text</p>")]
    [InlineData("Header # text", "<p>Header # text</p>")]
    [InlineData("# Header text", "<h1>Header text</h1>")]
    public void SingleCharp_ShouldConvertToHeader(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("# __Header__", "<h1><strong>Header</strong></h1>")]
    [InlineData("# _Header_", "<h1><em>Header</em></h1>")]
    [InlineData("# Header __with _different_ symbols__", "<h1>Header <strong>with <em>different</em> symbols</strong></h1>")]
    public void HeaderWithAnotherTags_ShouldWork(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Default Text\nAnother Text", "<p>Default Text</p>\n<p>Another Text</p>")]
    [InlineData("# Header\nDefault Text\nAnother Text", "<h1>Header</h1>\n<p>Default Text</p>\n<p>Another Text</p>")]
    public void Lines_ShouldDividedCorrectrly(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("__Bold text__ and _italic text_", "<p><strong>Bold text</strong> and <em>italic text</em></p>")]
    [InlineData("__Bold text__ and __not tag text_", "<p><strong>Bold text</strong> and __not tag text_</p>")]
    [InlineData("__Bold text__ and _italic text_ and\n * Element", "<p><strong>Bold text</strong> and <em>italic text</em> and</p>\n<ul>\n    <li>Element</li>\n</ul>")]

    public void DifferentTagsInOneParagraph_ShouldWordCorrectly(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("*Element", "<p>*Element</p>")]
    [InlineData("Element *list", "<p>Element *list</p>")]
    [InlineData("Element * list", "<p>Element * list</p>")]
    [InlineData("* Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("* Element 1\n* Element 2\n* Element 3", "<ul>\n    <li>Element 1</li>\n    <li>Element 2</li>\n    <li>Element 3</li>\n</ul>")]
    public void MarkedList_ShouldWork(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("* Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("- Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("+ Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("* Element 1\n- Element 2\n+ Element 3", "<ul>\n    <li>Element 1</li>\n    <li>Element 2</li>\n    <li>Element 3</li>\n</ul>")]
    public void MarkedList_ShouldSupportAllListSymbols(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("* _Italic element_\n* _Italic element_", "<ul>\n    <li><em>Italic element</em></li>\n    <li><em>Italic element</em></li>\n</ul>")]
    [InlineData("* __Bold element__\n* __Bold element__", "<ul>\n    <li><strong>Bold element</strong></li>\n    <li><strong>Bold element</strong></li>\n</ul>")]
    public void MarkedList_ShouldConvertAllTags(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("+ Item A\n+ Item B\n  + Sub-item 1\n  + Sub-item 2\n+ Item C", "<ul>\n    <li>Item A</li>\n    <li>Item B</li>\n        <ul>\n            <li>Sub-item 1</li>\n            <li>Sub-item 2</li>\n        </ul>\n    <li>Item C</li>\n</ul>")]
    [InlineData("+ Item 1\n* Item 2\n  * Sub-item 1\n+ Item 3", "<ul>\n    <li>Item 1</li>\n    <li>Item 2</li>\n        <ul>\n            <li>Sub-item 1</li>\n        </ul>\n    <li>Item 3</li>\n</ul>")]
    [InlineData("- Item 1\r\n  - Sub-item 1.1\r\n    - Sub-item 1.1.1\r\n  - Sub-item 1.2\r\n- Item 2", "<ul>\n    <li>Item 1</li>\n        <ul>\n            <li>Sub-item 1.1</li>\n                <ul>\n                    <li>Sub-item 1.1.1</li>\n                </ul>\n            <li>Sub-item 1.2</li>\n        </ul>\n    <li>Item 2</li>\n</ul>")]

    public void MarkedList_ShouldSupportNestedLists(string input, string expected)
    {
        string result = _processor.ConvertToHtmlFromString(input);

        Assert.Equal(expected, result);
    }
}

