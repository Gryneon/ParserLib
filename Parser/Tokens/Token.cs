#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class RegexToken : IRegexToken, IEquatable<CToken>
{
  public Dictionary<string, string> Properties { get; init; } = [];
  public Collection<IToken> Children { get; } = [];
  public string? Content { get; init; }
  public bool HasProperties => Properties.Count != 0;
  public int Length { get; }
  public int Depth { get; set; }
  public int Position { get; init; }
  public int EndPos => Position + Length - 1;
  public string Type { get; init; }
  public TokenNodeGroup? FromNode { get; init; }
  public TokenNode? LinkNode { get; set; }
  public CToken? Node { get; set; }
  public int Count => Children.Count;

  public void Add (IToken child) => Children.Add(child);
  public IEnumerator<IToken> GetEnumerator () => Children.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public bool Equals (CToken? other) => other is not null && other.Match(this);

  public RegexToken (MatchDataSet mdd, string type = EmptyString)
  {
    mdd.ThrowIfNull();
    Type = type;
    Position = mdd.Pos;
    Content = mdd.Content;
    Children = [.. from item in mdd.Groups
      select new RegexToken(item.Value)];
  }
  public RegexToken (GroupDataSet gd, string type = EmptyString)
  {
    gd.ThrowIfNull();
    Type = type;
    Content = gd.Content;
    Position = gd.Pos;
    Children = [.. from item in gd.Captures
      select new RegexToken(item)];
  }
  public RegexToken (CaptureData cd, string type = EmptyString)
  {
    cd.ThrowIfNull();
    Type = type;
    Content = cd.Content;
    Position = cd.Pos;
  }
}

public sealed class ParentToken : IParentToken, IEquatable<CToken>
{
  public TemplateSet? Template { get; init; }
  public Collection<IToken> Children { get; } = [];
  public Dictionary<string, string> Properties { get; init; } = [];
  string? IToken.Content => null;
  public bool HasProperties => Properties.Count > 0;
  public int Length => Children.Last().EndPos - Children.First().Position;
  public int Depth { get; set; }
  public int Position => Children.First().Position;
  public int EndPos => Children.Last().EndPos;
  public string Type { get; init; } = SE;
  public TokenNodeGroup? FromNode { get; init; }
  public TokenNode? LinkNode { get; set; }
  public CToken? Node { get; set; }
  public int Count => Children.Count;

  public void Add (IToken child) => Children.Add(child);
  public IEnumerator<IToken> GetEnumerator () => Children.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public bool Equals (CToken? other) => other is not null && other.Match(this);

  public ParentToken (TokenNode node, IEnumerable<IToken> tokens, string type = EmptyString)
  {
    LinkNode = node;
    Children = [.. tokens];
    Type = type;
  }
  public ParentToken (IEnumerable<IToken> tokens, string type = EmptyString)
  {
    Children = [.. tokens];
    Type = type;
  }
  public ParentToken (TokenNodeGroup grp, IEnumerable<IToken> tokens, string type = EmptyString)
  {
    FromNode = grp;
    Children = [.. from item in tokens select new Token(item)];
    Type = type;
  }
  public ParentToken () { }
}

/// <summary>Base abstract for tokens.<br/>
/// Reference this and not <see cref="IToken"/> when defining a class.<br/>
/// Reference <see cref="IToken"/> when creating a field or property, or returning a value from a method.
/// </summary>
/// <remarks>
/// A basic token object used by the <see cref="IParser"/>.<br/>
/// </remarks>
/// <seealso cref="IToken"/>
public class Token : IToken, ICloneable, IEquatable<CToken>
{
  #region Properties - Content
  /// <summary>The content of this token.</summary>
  public string? Content { get; init; }
  /// <summary>The position of the token in the original string.</summary>
  public int Position { get; init; }
  public int EndPos => Position + Length - 1;
  /// <summary>The length of the token.</summary>
  public int Length => Content!.Length;
  public int Depth { get; set; }
  /// <summary>The type of token this is.</summary>
  public string Type { get; init; }
  #endregion
  #region Properties Properties
  /// <summary>Whether this token has any properties.</summary>
  public bool HasProperties => false;
  #endregion
  #region Property Command Values
  public string? Key { get; init; }
  public string? Value { get; init; }
  #endregion
  #region Properties - Origin
  public TokenNodeGroup? FromNode { get; init; }
  public TokenNode? LinkNode { get; set; }
  public CToken? Node { get; set; }
  #endregion
  #region Constructors
  /// <summary>Creates a <see cref="Token"/> from a string and optionally a type.</summary>
  /// <param name="type">The type of token this is.</param>
  /// <param name="content"><see cref="string"/> content to initialize this token with.</param>
  public Token (string content, string type = EmptyString)
  {
    Content = content;
    Type = type;
  }
  public Token (MatchDataSet mdd, string type = EmptyString)
  {
    mdd.ThrowIfNull();
    Type = type;
    Position = mdd.Pos;
    Content = mdd.Content;
  }
  public Token (IToken token)
  {
    token.ThrowIfNull();
    Type = token.Type;
    Content = token.Content;
    Position = token.Position;
    Depth = token.Depth;
  }
  public Token (string content, int pos, string type = EmptyString, int depth = 0)
  {
    Position = pos;
    Content = content;
    Type = type;
  }
  public Token (TokenNodeRef item)
  {
    if (item is null)
    {
      Type = SE;
      Content = SE;
      return;
    }
    Content = item.ToString() ?? SE;
    Type = item.RefName;
    LinkNode = item;
  }

  #endregion
  #region Overrides & Interfaces
  /// <inheritdoc/>
  public override string? ToString () =>
    $"Type: {Type} Text: " + Content;
  /// <inheritdoc/>
  public object Clone () => new Token(this);
  /// <summary>Creates a <see cref="Token"/> from a <see cref="MatchDataSet"/> object.</summary>
  /// <param name="mdd">The originating object.</param>
  /// <returns>A token that represents the contents of the <see cref="MatchDataSet"/> object.</returns>
  public static Token Generate (MatchDataSet mdd) => new(mdd);
  public bool Equals (CToken? other) => other is not null && other.Match(this);
  #endregion
}
