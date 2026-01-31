//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

public interface ICanAccessChildren<TIndex, out TChild> : IEnumerable<TChild> where TIndex : notnull
{
  /// <summary>
  /// The number of children in this object.
  /// </summary>
  int Count { get; }
  TChild this[TIndex index] { get; }
}
