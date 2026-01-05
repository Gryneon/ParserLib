#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Tokens.Raw;

public abstract class TokenBase<T> : IToken<T> where T : notnull
{
  internal string _content = SE;
  public virtual string Content
  {
    get => Children.Select(static t => t.Content).TextJoin();
    set => _content = value;
  }
  public bool Ignored { get; set; }
  public bool Exempt { get; set; }
  public int Index { get; init; }
  public T? Type { get; set; }
  public TokenCollection<T> Children { get; init; } = [];
  public virtual int Count => Children.Count;
  public bool HasType => Type is not null;
  internal string ContentNoNewLine =>
    Content.
    Replace("\n", "<LF>", SCO).
    Replace("\r", "<CR>", SCO);
  public override string ToString () => $"{Index} : {Type} = \"{ContentNoNewLine}\"";
  public override bool Equals (object? obj) => obj is IToken<T> rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Index, Type);
  public virtual int CompareTo (IToken<T>? other) => Index.CompareTo(other?.Index); public static bool operator == (TokenBase<T> left, TokenBase<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenBase<T> left, TokenBase<T> right) => !(left == right);
  public static bool operator < (TokenBase<T> left, TokenBase<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenBase<T> left, TokenBase<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenBase<T> left, TokenBase<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenBase<T> left, TokenBase<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
