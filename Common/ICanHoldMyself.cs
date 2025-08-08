namespace Common;

public interface ICanHoldMyself<T> where T : ICanHoldMyself<T>
{
  T? Value { get; }
  Collection<T>? Children { get; }
  bool HasChildren => Children is not null;
  bool IsSingle => Value is not null;
  bool IsNull => Children is null && Value is null;
}
