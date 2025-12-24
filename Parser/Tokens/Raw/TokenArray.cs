#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenArray<T> : IToken<T>, IReadOnlyCollection<IToken<T>> where T : notnull
{
  // Tokens Kept
  public required TokenCollection<T> Items { get; init; }

  // Needed for sorting and classification
  public required T Type { get; set; }
  public required int Index { get; set; }
  public bool Exempt { get; set; }
  public bool HasType => true;

  public int Count => Items.Count;

  public TokenCollection<T> Children { get; init; } = [];

  public int CompareTo (IToken<T>? other) => Index.CompareTo(other?.Index);
  public bool Equals (IReadOnlyCollection<IToken<T>>? other) => other is not null && Items.SequenceEqual(other);

  public override bool Equals (object? obj) => obj switch
  {
    IReadOnlyCollection<IToken<T>> iroc => Equals(iroc),
    IEnumerable<IToken<T>> ie => ie.SequenceEqual(this.AsEnumerable()),
    _ => false
  };

  public override int GetHashCode () => Items.GetHashCode();
  public IEnumerator<IToken<T>> GetEnumerator () => Items.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

  public static bool operator == (TokenArray<T> left, TokenArray<T> right) => left?.Equals(right) ?? false;
  public static bool operator != (TokenArray<T> left, TokenArray<T> right) => !(left == right);
  public static bool operator < (TokenArray<T> left, TokenArray<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenArray<T> left, TokenArray<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenArray<T> left, TokenArray<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenArray<T> left, TokenArray<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
