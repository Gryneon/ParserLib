#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenTypedValue<T> : IToken<T> where T : notnull
{
  public string ValueType => ValueTypeToken.Content;
  public required IToken<T> ValueTypeToken { get; init; }
  public string Name => NameToken.Content;
  public required IToken<T> NameToken { get; init; }
  public T? Type { get; init; }
  public bool HasType => Type is not null;
  public TokenCollection<T> Children { get; init; } = [];
  public bool Exempt { get; set; }
  public int Index { get; init; }

  public int CompareTo (IToken<T>? other) => other is TokenTypedValue<T> tv ? Name.CompareTo(tv.Name, SCO) : -1;
  public override bool Equals (object? obj) => obj is TokenTypedValue<T> tv && Name.Equals(tv.Name, SCO) && ValueType.Equals(tv.ValueType, SCO);

  public override int GetHashCode () => HashCode.Combine(Name, ValueType);

  public static bool operator == (TokenTypedValue<T> left, TokenTypedValue<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenTypedValue<T> left, TokenTypedValue<T> right) => !(left == right);
  public static bool operator < (TokenTypedValue<T> left, TokenTypedValue<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenTypedValue<T> left, TokenTypedValue<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenTypedValue<T> left, TokenTypedValue<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenTypedValue<T> left, TokenTypedValue<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
