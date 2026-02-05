#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenTypedValue : TokenBase, IValueToken, ITypeToken
{
  public string? ObjType => TypeToken?.Content;
  public required IToken? TypeToken { get; set; }
  public string? Value => ValueToken?.Content;
  public required IToken? ValueToken { get; set; }
  public override bool Equals (object? obj) => obj is TokenTypedValue tv && (Value?.Equals(tv.Value, SCO) ?? false) && (ObjType?.Equals(tv.ObjType, SCO) ?? false);
  public override int GetHashCode () => HashCode.Combine(Value, ObjType);

  public override string ToString () => $"{Value} as {ObjType}";
}

