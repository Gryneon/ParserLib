#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

/// <summary>A Token representing a flag.</summary>
/// <remarks>Contains a boolean and a name, and only the name is token-backed.</remarks>
public sealed class TokenFlag : TokenBase, INameToken
{
  public bool State { get; init; }
  public string? Name => NameToken?.Content;
  public required IToken? NameToken { get; set; }
  public override bool Equals (object? obj) => obj is TokenFlag flag && (Name?.Equals(flag.Name, SCO) ?? false) && State == flag.State;
  public override int GetHashCode () => HashCode.Combine(Name, State);
}
