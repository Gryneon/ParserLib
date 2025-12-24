#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class Token<T> : IToken<T> where T : notnull
{
  // Required Properties
  public required int Position { get; init; }
  public required string Content { get; init; }
  public T? Type { get; set; }
  public bool Exempt { get; set; }
  public bool Ignored { get; set; }
  // Calculated Properties
  public int LastPosition => Position + Length - 1;
  public int Length => Content.Length;
  public int Index => Position;
  [MemberNotNullWhen(true, nameof(Type))]
  public bool HasType => Type != null;
  internal string ContentNoNewLine => Content.Replace("\n", "<NL>", SCO);

  public TokenCollection<T> Children { get; init; } = [];

  public int CompareTo (IIndexSortable? other) => Index.CompareTo(other?.Index);
  public int CompareTo (IToken<T>? other) => Position.CompareTo(other?.Index);
  public override string ToString () => $"{Position} : {Type} = \"{ContentNoNewLine}\"";
  public override bool Equals (object? obj) => obj is IToken<T> rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Position, Type);

  public static bool operator == (Token<T> left, IToken<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (Token<T> left, IToken<T> right) => !(left == right);
  public static bool operator < (Token<T> left, IToken<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (Token<T> left, IToken<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (Token<T> left, IToken<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (Token<T> left, IToken<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
