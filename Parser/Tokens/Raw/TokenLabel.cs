#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public abstract class TokenBase<T> : IToken<T> where T : notnull 
{
  public virtual string Content => Children.Select(static t => t.Content).TextJoin();
  public bool Exempt { get; set; }
  public int Index { get; init; }
  public T? Type { get; init; }
  public TokenCollection<T> Children { get; init; } = [];
  public int Count => Children.Count;
  public bool HasType => Type is not null;
  internal string ContentNoNewLine => Content.Replace("\n", "<NL>", SCO);
  public override string ToString () => $"{Index} : {Type} = \"{ContentNoNewLine}\"";
  public override bool Equals (object? obj) => obj is IToken<T> rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Index, Type);
  public virtual int CompareTo (IToken<T>? other) => Index.CompareTo(other?.Index);
  public static bool operator == (TokenBase<T> left, TokenBase<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenBase<T> left, TokenBase<T> right) => !(left == right);
  public static bool operator < (TokenBase<T> left, TokenBase<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenBase<T> left, TokenBase<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenBase<T> left, TokenBase<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenBase<T> left, TokenBase<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}

public sealed class TokenLabel<T> : TokenBase<T> where T : notnull
{
  public string Name => NameToken.Content;
  public required IToken<T> NameToken { get; init; }
  

  public override int CompareTo (IToken<T>? other) => other is TokenLabel<T> tv ? Name.CompareTo(tv.Name, SCO) : -1;
  public override bool Equals (object? obj) => obj is TokenLabel<T> tv && Name.Equals(tv.Name, SCO);

  public override int GetHashCode () => Name.GetHashCode(SCO);
}
