namespace Common;

public interface IProperty<TValue> :
  IEquatable<IProperty<TValue>>,
  IComparable<IProperty<TValue>>
{
  string Key { get; }
  TValue Value { get; }
}
