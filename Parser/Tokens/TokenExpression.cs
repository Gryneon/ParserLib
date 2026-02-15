#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenExpression : TokenBase, ITypeToken, IToken
{
  public string? ObjType => TypeToken?.Content;
  public IToken? TypeToken { get; set; }
  public string? LeftValue => LeftValueToken?.Content;
  public IToken? LeftValueToken { get; init; }
  public string? RightValue => RightValueToken?.Content;
  public IToken? RightValueToken { get; init; }
  public override bool Equals (object? obj) => obj is TokenExpression tv && (LeftValue?.Equals(tv.LeftValue, SCO) ?? false) && (ObjType?.Equals(tv.ObjType, SCO) ?? false) && (RightValue?.Equals(tv.RightValue, SCO) ?? false);
  public override int GetHashCode () => HashCode.Combine(LeftValue, ObjType, RightValue);
  public bool IsBinary => LeftValue is not null && RightValue is not null && ObjType is not null;
  public bool IsUnary => LeftValue is null && RightValue is not null && ObjType is not null;
  public bool HasLeftRight => true;
}

