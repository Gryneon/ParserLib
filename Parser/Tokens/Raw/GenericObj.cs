#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Linq;

namespace Parser.Tokens.Raw;

[Obsolete("Use Token")]
public class GenericObj<T> : IToken<T>, IProperty<string> where T : notnull
{
  // Assigned Properties
  public string? Name { get; set; }
  public string? Value { get; set; }
  public Collection<IProperty<string>> Properties { get; } = [];
  public Collection<IProperty<bool>> Flags { get; } = [];

  // Tokens Kept
  public Collection<IToken<T>> Tokens { get; } = [];

  // Needed for sorting and classification
  public required T Type { get; set; }
  public string Key
  {
    get => Name ?? SE;
    set => Name = value;
  }
  public int Index { get; set; }
  public bool HasType => true;

  public IList<IToken<T>> Children { get; init; } = [];

  public int CompareTo (IToken<T>? other) => Index.CompareTo(other?.Index);
  int IComparable<IProperty<string>>.CompareTo (IProperty<string>? other) => Key.CompareTo(other?.Key, SCO);
  public bool Equals (IProperty<string>? other) => Key.Equals(other?.Key, SCO) && (Value?.Equals(other?.Value, SCO) ?? false);

  public override bool Equals (object? obj) => obj is GenericObj<T> g && Children.SequenceEqual(g.Children);

  public override int GetHashCode () => Children.GetHashCode();

  public static bool operator == (GenericObj<T> left, GenericObj<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (GenericObj<T> left, GenericObj<T> right) => !(left == right);
  public static bool operator < (GenericObj<T> left, GenericObj<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (GenericObj<T> left, GenericObj<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (GenericObj<T> left, GenericObj<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (GenericObj<T> left, GenericObj<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
