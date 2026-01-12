#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenLabel : TokenBase
{
  public string Name => NameToken.Content;
  public required IToken NameToken { get; init; }
  public override int CompareTo (IToken? other) => other is TokenLabel tv ? Name.CompareTo(tv.Name, SCO) : -1;
  public override bool Equals (object? obj) => obj is TokenLabel tv && Name.Equals(tv.Name, SCO);
  public override int GetHashCode () => Name.GetHashCode(SCO);
}
