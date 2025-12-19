#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenObject<T> : IToken<T>, IReadOnlyCollection<IReadOnlyProperty<string>>, IReadOnlyCollection<IProperty<string>> where T : notnull
{
  // Assigned Properties
  public string Name => NameToken.Content;
  public string? ObjType => TypeToken is Token<T> t ? t.Content : null;

  // Tokens Kept
  public required Token<T> NameToken { get; init; }
  public IToken<T>? TypeToken { get; init; }

  public IList<TokenProperty<T>> Properties { get; init; } = [];
  public IList<TokenFlag<T>> Flags { get; init; } = [];

  // Needed for sorting and classification
  public required T Type { get; set; }
  public int Index { get; set; }
  public bool HasType => true;

  public IList<IToken<T>> Children { get; init; } = [];
  public int Count => Children.Count;

  public int CompareTo (IToken<T>? other) => Index.CompareTo(other?.Index);
  public int CompareTo (TokenObject<T>? other) => Name.CompareTo(other?.Name, SCO);

  public override bool Equals (object? obj) => obj switch
  {
    TokenObject<string> ips =>
      typeof(T).IsInstanceOfType(SE) &&
      ObjType == ips.ObjType &&
      Properties.SequenceEqual(ips.Properties as IList<TokenProperty<T>> ?? []) &&
      Flags.SequenceEqual(ips.Flags as IList<TokenFlag<T>> ?? []) &&
      Name == ips.Name &&
      (Type.ToString()?.Equals(ips.Type, SCO) ?? false),
    TokenObject<T> ips =>
      typeof(T).IsInstanceOfType(ips.Type) &&
      ObjType == ips.ObjType &&
      Properties.SequenceEqual(ips.Properties) &&
      Flags.SequenceEqual(ips.Flags) &&
      Name == ips.Name &&
      Type.Equals(ips.Type),
    _ => false
  };

  public override int GetHashCode () => HashCode.Combine(Name, Type, ObjType, Properties, Flags);
  public IEnumerator<IReadOnlyProperty<string>> GetEnumerator () => Properties.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  IEnumerator<IProperty<string>> IEnumerable<IProperty<string>>.GetEnumerator () => (IEnumerator<IProperty<string>>) GetEnumerator();

  public static bool operator == (TokenObject<T> left, TokenObject<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenObject<T> left, TokenObject<T> right) => !(left == right);
  public static bool operator < (TokenObject<T> left, TokenObject<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenObject<T> left, TokenObject<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenObject<T> left, TokenObject<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenObject<T> left, TokenObject<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
