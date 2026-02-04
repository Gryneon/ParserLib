namespace Common;

public class PropertyBase<T> : IProperty<T>
{
  public virtual required string Key { get; set; }
  public virtual T? Value { get; set; }
  public virtual int CompareTo (IProperty<T>? other) => Key.CompareTo(other?.Key, SCO);
  public virtual bool Equals (IProperty<T>? other) => other is not null && Key.Like(other.Key) && (Value is null || Value.Equals(other.Value));
  public override bool Equals (object? obj) => obj is IProperty<T> iprop && Equals(iprop);
  public override int GetHashCode () => HashCode.Combine(Key, Value);
  public static bool operator == (PropertyBase<T> left, PropertyBase<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (PropertyBase<T> left, PropertyBase<T> right) => !(left == right);
  public static bool operator < (PropertyBase<T> left, PropertyBase<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (PropertyBase<T> left, PropertyBase<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (PropertyBase<T> left, PropertyBase<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (PropertyBase<T> left, PropertyBase<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
