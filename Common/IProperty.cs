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
