#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenTypedValue<T> : TokenBase<T> where T : notnull
{
  public string ValueType => ValueTypeToken.Content;
  public required IToken<T> ValueTypeToken { get; init; }
  public string Value => ValueToken.Content;
  public required IToken<T> ValueToken { get; init; }
  public override bool Equals (object? obj) => obj is TokenTypedValue<T> tv && Value.Equals(tv.Value, SCO) && ValueType.Equals(tv.ValueType, SCO);
  public override int GetHashCode () => HashCode.Combine(Value, ValueType);
}
