#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenTypedValue : TokenBase
{
  public string ValueType => ValueTypeToken.Content;
  public required IToken ValueTypeToken { get; init; }
  public string Value => ValueToken.Content;
  public required IToken ValueToken { get; init; }
  public override bool Equals (object? obj) => obj is TokenTypedValue tv && Value.Equals(tv.Value, SCO) && ValueType.Equals(tv.ValueType, SCO);
  public override int GetHashCode () => HashCode.Combine(Value, ValueType);
}
