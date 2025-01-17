using Markdown.Interfaces;

namespace Markdown.Classes.Parsers;

public class TokenParser : IParser<Token>
{
    private readonly ITagFactory _doubleTagFactory;

    public TokenParser(ITagFactory doubleTagFactory)
    {
        _doubleTagFactory = doubleTagFactory;
    }

    public IEnumerable<Token> Parse(string content)
    {
        var words = content.Split(' ');
        var tokens = new List<Token>();
        var tagStack = new Stack<string>();

        foreach (var word in words)
        {
            var tokenTags = ParseWordTags(word, tagStack);

            if (tokenTags.Count == 1 && IsTagInsideWord(tokenTags.First(), word))
            {
                tokenTags.Clear();
                if (tagStack.Count > 0)
                {
                    tagStack.Pop();
                }
            }

            tokens.Add(new Token(word, tokenTags));
        }

        RemoveUnclosedTags(tokens, tagStack);
        return tokens;
    }


    private List<TagData> ParseWordTags(string word, Stack<string> tagStack)
    {
        var tokenTags = new List<TagData>();
        bool isEscaped = false;

        if (IsEmptyWord(word))
        {
            return tokenTags;
        }

        for (int i = 0; i < word.Length; i++)
        {
            string symbol = word[i].ToString();


            if (symbol == "\\" && !isEscaped)
            {
                isEscaped = true;
                continue;
            }


            if (IsTag(symbol) && !isEscaped)
            {
                var currentTag = DetermineTag(word, i);

                if (ContainsDigitsInsideTag(word, i, currentTag))
                {
                    continue;
                }

                if (tagStack.Contains(currentTag))
                {
                    if (tagStack.Peek() != currentTag)
                    {
                        tokenTags.Clear();
                        break;
                    }

                    if (HasSymbolBeforeClosingTag(word, currentTag, i) && !IsBoldTagNested(currentTag, tagStack))
                    {
                        tokenTags.Add(CreateClosingTag(currentTag, i));
                        tagStack.Pop();
                    }
                }
                else
                {

                    if (HasSymbolAfterOpenTag(word, currentTag, i) && !IsBoldTagNested(currentTag, tagStack))
                    {
                        tokenTags.Add(CreateOpeningTag(currentTag, i));
                        tagStack.Push(currentTag);
                    }
                }

                i += currentTag.Length - 1;
            }

            isEscaped = false;
        }

        return tokenTags;
    }


    private bool IsTag(string symbol)
    {
        return _doubleTagFactory.GetTag(symbol) != null;
    }

    private bool ContainsDigitsInsideTag(string word, int tagPosition, string tag)
    {
        int start = word.IndexOf(tag, tagPosition);
        int end = word.LastIndexOf(tag);

        if (start != -1 && end != -1 && start < end)
        {
            var innerContent = word.Substring(start + tag.Length, end - (start + tag.Length));

            return innerContent.All(char.IsDigit);
        }

        return false;
    }

    private string DetermineTag(string word, int index)
    {
        string symbol = word[index].ToString();

        if (index + 1 < word.Length && IsTag(symbol + word[index + 1]))
        {
            return symbol + word[index + 1];
        }

        return symbol;
    }

    private bool IsBoldTagNested(string currentTag, Stack<string> tagStack)
    {
        return tagStack.Contains("_") && currentTag == "__";
    }

    private bool IsEmptyWord(string word)
    {
        return word.All(c => IsTag(c.ToString()));
    }
    private bool HasSymbolAfterOpenTag(string word, string currentTag, int index)
    {
        return index + currentTag.Length < word.Length && word[index + currentTag.Length] != ' ';
    }

    private bool HasSymbolBeforeClosingTag(string word, string currentTag, int index)
    {
        return index >= currentTag.Length && word[index - currentTag.Length] != ' ';
    }

    private bool IsTagInsideWord(TagData tokenTag, string word)
    {
        return tokenTag.Index != 0 && tokenTag.Index != word.Length - tokenTag.Tag.MdLength;
    }

    private void RemoveUnclosedTags(List<Token> tokens, Stack<string> tagStack)
    {
        while (tagStack.Count > 0)
        {
            var unclosedTag = tagStack.Pop();

            var tagToRemove = _doubleTagFactory.GetTag(unclosedTag);

            for (int i = tokens.Count - 1; i >= 0; i--)
            {
                var tagToRemoveIndex = tokens[i].Tags.FindIndex(t => t.Tag == tagToRemove);
                if (tagToRemoveIndex != -1)
                {
                    tokens[i].Tags.RemoveAt(tagToRemoveIndex);
                    break;
                }
            }
        }
    }

    private TagData CreateOpeningTag(string tag, int index)
    {
        var tagElement = _doubleTagFactory.GetTag(tag);
        return new TagData(tagElement, index);
    }

    private TagData CreateClosingTag(string tag, int index)
    {
        var tagElement = _doubleTagFactory.GetTag(tag);
        return new TagData(tagElement, index, isClosing: true);
    }
}


