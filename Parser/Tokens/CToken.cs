namespace Parser.Tokens;

public class CToken
{
  /// <summary>The token type required. If the type matches any of these, it passes this check.</summary>
  public IEnumerable<string> Type { get; init; } = [];
  public IEnumerable<string> Content { get; init; } = [];
  public int? Depth { get; set; }
  public bool IsOptional { get; internal set; }
  public bool IsOneOrMany { get; internal set; }

  public CToken () { }

  public bool Match (IToken token)
  {
    token.ThrowIfNull();
    bool mType = Type.IsEmpty() || token.Type.Like(Type);
    bool mContent = Content.IsEmpty() || token.Content.Like(Content);
    bool mDepth = Depth is not null && Depth == token.Depth || Depth is null;

    return mType && mContent && mDepth;
  }

  public static CToken MkType (params string[] type) => new() { Type = type };
  public static CToken MkContent (params string[] content) => new() { Content = content };
  public static CToken MkDepth (int depth) => new() { Depth = depth };
  public static Collection<CToken> Parse (string input)
  {
    if (input is null)
      return [];
    Collection<string> chunks = [.. input.Split([' ', '\t', '\n'], SSORT)];
    Collection<CToken> tokens = [];
    foreach (string item in chunks)
    {
      if (item.IsEmpty())
      {
        continue;
      }
      else if (item.StartsWith("t:", SCO))
      {
        tokens.Add(MkType(item[2..]));
      }
      else if (item.StartsWith("mt:", SCO))
      {
        string item2 = item[3..];
        string[] types = item2.Split(';', SSORT);
        tokens.Add(MkType(types));
      }
      else if (item.StartsWith("d:", SCO))
      {
        int? parsed = item[2..].ToInt();
        if (parsed.HasValue)
          tokens.Add(MkDepth(parsed.Value));
      }
      else if (item.StartsWith("mc:", SCO))
      {
        string item2 = item[3..];
        string[] content = item2.Split(';', SSORT);
        tokens.Add(MkContent(content));
      }
      else
      {
        tokens.Add(MkContent(item));
      }
    }
    return tokens;
  }
}
