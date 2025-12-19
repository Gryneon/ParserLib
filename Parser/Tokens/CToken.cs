namespace Parser.Tokens;

/// <summary>A rule defining how to match 1 token in a rule group.</summary>
public class CToken
{
  /// <summary>The token type required. If the type matches any of these, it passes this check.</summary>
  /// <remarks>An empty list bypasses the check.</remarks>
  public IEnumerable<string> Type { get; init; } = [];
  /// <summary>The content of the token to match. If the content matches any of these, it passes this check.</summary>
  /// <remarks>An empty list bypasses the check.</remarks>
  public IEnumerable<string> Content { get; init; } = [];
  /// <summary>The required depth value to match.</summary>
  /// <remarks>A null value bypasses this check.</remarks>
  public int? Depth { get; set; }
  /// <summary>Whether or not the token rule is optional or not.</summary>
  public bool IsOptional { get; internal set; }
  /// <summary>Whether or not the token rule can consume multiple tokens.</summary>
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
  /// <summary>Creates a new depth CToken.</summary>
  /// <param name="depth">The depth to look for.</param>
  /// <returns>A <see cref="CToken"/> that checks for a specific depth.</returns>
  public static CToken MkDepth (int depth) => new() { Depth = depth };
  /// <summary>Parses the string passed as a required parameter to the rule into a Collection of <see cref="CToken"/> objects.</summary>
  /// <param name="input">The string input to parse.</param>
  /// <returns>A Collection of <see cref="CToken"/> objects.</returns>
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
