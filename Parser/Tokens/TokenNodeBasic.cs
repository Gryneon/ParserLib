#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Tokens;

/// <summary>
/// A simple token node.
/// </summary>
public sealed class TokenNodeBasic : TokenNode
{
  public string Literal { get; set; }
  public TokenNodeBasic (TokenNodeType type, string literal)
  {
    Type = type;
    Literal = literal;
  }
  public override bool Match => CurrentToken?.Content?.Equals(Literal, Spec.Active.SC) ?? false;
}
