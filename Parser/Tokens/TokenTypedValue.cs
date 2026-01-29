#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenTypedValue : TokenBase, IValueToken, ITypeToken
{
  public string? ObjType => TypeToken?.Content;
  public required IToken? TypeToken { get; init; }
  public string? Value => ValueToken?.Content;
  public required IToken? ValueToken { get; init; }
  public override bool Equals (object? obj) => obj is TokenTypedValue tv && (Value?.Equals(tv.Value, SCO) ?? false) && ValueType.Equals(tv.ObjType, SCO);
  public override int GetHashCode () => HashCode.Combine(Value, ObjType);
}
