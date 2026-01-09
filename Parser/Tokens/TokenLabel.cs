#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenLabel<T> : TokenBase<T> where T : notnull
{
  public string Name => NameToken.Content;
  public required IToken<T> NameToken { get; init; }
  public override int CompareTo (IToken<T>? other) => other is TokenLabel<T> tv ? Name.CompareTo(tv.Name, SCO) : -1;
  public override bool Equals (object? obj) => obj is TokenLabel<T> tv && Name.Equals(tv.Name, SCO);
  public override int GetHashCode () => Name.GetHashCode(SCO);
}
