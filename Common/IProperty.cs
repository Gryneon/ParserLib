namespace Common;

/// <summary>Represents a key and value pair.</summary>
/// <typeparam name="TValue">The type of value.</typeparam>
public interface IProperty<TValue> :
  IEquatable<IProperty<TValue>>,
  IComparable<IProperty<TValue>>,
  IReadOnlyProperty<TValue>
{
  /// <summary>The key name.</summary>
  new string Key { get; set; }
  string IReadOnlyProperty<TValue>.Key => Key;
  /// <summary>The value assigned to the key.</summary>
  new TValue? Value { get; set; }
  TValue? IReadOnlyProperty<TValue>.Value => Value;
}

public class PropertyBase<T> : IProperty<T>
{
  public required string Key { get; set; }
  public T? Value { get; set; }
  public int CompareTo (IProperty<T>? other) => Key.CompareTo(other?.Key, SCO);
  public bool Equals (IProperty<T>? other) => other is not null && Key.Like(other.Key) && (Value is null || Value.Equals(other.Value));
  public override bool Equals (object? obj) => obj is IProperty<T> iprop && Equals(iprop);
  public override int GetHashCode () => HashCode.Combine(Key, Value);
  public static bool operator == (PropertyBase<T> left, PropertyBase<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (PropertyBase<T> left, PropertyBase<T> right) => !(left == right);
  public static bool operator < (PropertyBase<T> left, PropertyBase<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (PropertyBase<T> left, PropertyBase<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (PropertyBase<T> left, PropertyBase<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (PropertyBase<T> left, PropertyBase<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
