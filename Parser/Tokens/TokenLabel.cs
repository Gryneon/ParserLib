#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenLabel : TokenBase, INameToken
{
  public string? Name => NameToken?.Content;
  public required IToken? NameToken { get; init; }
  public override bool Equals (object? obj) => obj is TokenLabel tv && (Name?.Equals(tv.Name, SCO) ?? false);
  public override int GetHashCode () => Name?.GetHashCode(SCO) ?? 0;
}
