namespace Common;

/// <summary>Represents a read-only key and value pair.</summary>
/// <typeparam name="TValue">The type of value.</typeparam>
public interface IReadOnlyProperty<TValue> :
  IEquatable<IProperty<TValue>>,
  IComparable<IProperty<TValue>>
{
  /// <summary>The key name.</summary>
  string Key { get; }
  /// <summary>The value assigned to the key.</summary>
  TValue? Value { get; }
}
