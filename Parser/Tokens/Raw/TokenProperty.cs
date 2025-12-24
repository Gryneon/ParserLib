#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Linq;

using Common.Extensions;

namespace Parser.Tokens.Raw;

public sealed class TokenProperty<T> : IToken<T>, IReadOnlyProperty<string>, IProperty<string> where T : notnull
{
  // Assigned Properties
  string IProperty<string>.Key { get => Name; set => value.DoNothing(); }
  public string Name => NameToken.Content;
  string? IProperty<string>.Value
  {
    get => ValueToken?.Content;
    set => value.DoNothing();
  }
  public string? Value => ValueToken?.Content;

  // Tokens Kept
  public required Token<T> NameToken { get; init; }
  public required IToken<T>? ValueToken { get; init; }

  // Needed for sorting and classification
  public required T Type { get; set; }
  public int Index { get; set; }
  public bool Exempt { get; set; }
  public bool HasType => true;

  string IReadOnlyProperty<string>.Key => Name;

  public TokenCollection<T> Children { get; init; } = [];

  int IComparable<IProperty<string>>.CompareTo (IProperty<string>? other) => Name.CompareTo(other?.Key, SCO);
  public bool Equals (IProperty<string>? other) => Name.Equals(other?.Key, SCO) && (Value?.Equals(other?.Value, SCO) ?? false);

  public override bool Equals (object? obj) => obj switch
  {
    IProperty<string> ips => Name.Equals(ips.Key, SCO) && (Value?.Equals(ips.Value, SCO) ?? false),
    _ => false
  };

  public override int GetHashCode () => HashCode.Combine(Name, Value);
  public int CompareTo (IToken<T>? other) => Index.CompareTo(other?.Index);

  public static bool operator == (TokenProperty<T> left, TokenProperty<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenProperty<T> left, TokenProperty<T> right) => !(left == right);
  public static bool operator < (TokenProperty<T> left, TokenProperty<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenProperty<T> left, TokenProperty<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenProperty<T> left, TokenProperty<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenProperty<T> left, TokenProperty<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
