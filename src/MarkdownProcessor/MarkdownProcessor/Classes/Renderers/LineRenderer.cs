using System.Text;

namespace Markdown.Classes.Renderers;

public class LineRenderer
{
    public string RenderLine(Line line)
    {
        var renderedString = new StringBuilder();

        foreach (var token in line.Tokens)
        {
            var word = token.Word;
            var tagDataList = token.Tags;

            if (tagDataList.Count > 0)
            {
                var renderedWord = ReplaceTags(word, tagDataList);
                renderedString.Append(renderedWord);
            }
            else
            {
                renderedString.Append(word);
            }

            renderedString.Append(' ');
        }

        string content = renderedString.ToString().TrimEnd();

        if (line.Type == null)
        {
            return content;
        }

        return line.Type.RenderLine(content, line.IndentLevel);
    }


    private string ReplaceTags(string word, List<TagData> tagDataList)
    {
        var result = new StringBuilder(word);

        foreach (var tagData in tagDataList.OrderByDescending(t => t.Index))
        {
            var tagLength = tagData.Tag.MdLength;
            var replacement = tagData.IsClosing ? tagData.Tag.CloseHtmlTag : tagData.Tag.OpenHtmlTag;

            result.Remove(tagData.Index, tagLength);
            result.Insert(tagData.Index, replacement);
        }

        return result.ToString();
    }

}
