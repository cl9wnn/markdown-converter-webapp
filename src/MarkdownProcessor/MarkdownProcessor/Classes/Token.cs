namespace Markdown.Classes;
public class Token
{
    public string Word { get; set; }
    public List<TagData> Tags { get; set; }

    public Token(string word, List<TagData> tags)
    {
        Word = word;
        Tags = tags;
    }
}
