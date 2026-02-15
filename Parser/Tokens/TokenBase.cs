#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Tokens;

public abstract class TokenBase : IToken
{
  public virtual string Content
  {
    get => Children.Select(static t => t.Content).TextJoin();
    set => throw new NotSupportedException("Must override Content to set.");
  }
  public bool Ignored { get; set; }
  public bool Exempt { get; set; }
  public int Index { get; init; }
  public string Type { get; set; } = SE;
  public IReadOnlyList<IToken> Children { get; init; } = [];
  public virtual int Count => Children.Count;
  public bool HasType => Type is not null;
  public override string ToString () => $"{Index} : {GetType().Name} is {Type} = {((IToken) this).ToStringSm()}";
  public override bool Equals (object? obj) => obj is IToken rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Index, Type);
  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public virtual bool Equals (IToken? other) => GetHashCode() == other?.GetHashCode();
  public static bool operator == (TokenBase left, IToken right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenBase left, IToken right) => !(left == right);
  public static bool operator < (TokenBase left, IToken right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenBase left, IToken right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenBase left, IToken right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenBase left, IToken right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
