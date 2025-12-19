#pragma warning disable CA1710 // Identifiers should have correct suffix

using Parser.Tokens.Node;

namespace Parser.Tokens;

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
  public bool IsIgnored => false;
  public string Type { get; init; } = SE;
  public TokenNodeGroup? FromNode { get; init; }
  public TokenNode? LinkNode { get; set; }
  public CToken? Node { get; set; }
  public int Count => Children.Count;

  public void Add (IToken child) => Children.Add(child);
  public void AddRange (IEnumerable<IToken> children)
  {
    children.ThrowIfNull();
    foreach (IToken child in children)
    {
      Add(child);
    }
  }
  public IEnumerator<IToken> GetEnumerator () => Children.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public bool Equals (CToken? other) => other is not null && other.Match(this);
  public override string ToString () => "PToken: [" + Children.Count + "] Type: " + Type + (IsIgnored ? " (Ignored)" : "");

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
