#pragma warning disable CA1710 // Identifiers should have correct suffix

using Parser.Tokens.Node;

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
  public bool IsIgnored { get; set; }
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
    Properties = [.. from item in mdd.Groups
      select new KeyValuePair<string, string>(item.Key, item.Value.Content)];
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

  public override string ToString () => $"{Position} - {Type}:{Content}";
  public void AddRange (IEnumerable<IToken> children)
  {
    children.ThrowIfNull();
    foreach (IToken child in children)
    {
      Add(child);
    }
  }
}
