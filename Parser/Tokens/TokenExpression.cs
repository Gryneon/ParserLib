#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenExpression : TokenBase, ITypeToken
{
  public string? ObjType => TypeToken?.Content;
  public IToken? TypeToken { get; set; }
  public string? LeftValue => LeftValueToken?.Content;
  public IToken? LeftValueToken { get; init; }
  public string? RightValue => RightValueToken?.Content;
  public IToken? RightValueToken { get; init; }

  public (IToken? Left, IToken? Right) LeftRightValueToken
  {
    get => (LeftValueToken, RightValueToken);
    init
    {
      LeftValueToken = value.Left;
      RightValueToken = value.Right;
    }
  }
  public override bool Equals (object? obj) => obj is TokenExpression tv && (LeftValue?.Equals(tv.LeftValue, SCO) ?? false) && (ObjType?.Equals(tv.ObjType, SCO) ?? false) && (RightValue?.Equals(tv.RightValue, SCO) ?? false);
  public override int GetHashCode () => HashCode.Combine(LeftValue, ObjType, RightValue);
  public override string ToString () => $"{LeftValue} {ObjType} {RightValue}";
}

