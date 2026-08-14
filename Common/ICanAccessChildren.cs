//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

/// <summary>Provides a simple keyed lookup framework.</summary>
/// <typeparam name="TIndex">The index or key type.</typeparam>
/// <typeparam name="TChild">The child's type.</typeparam>
public interface ICanAccessChildren<TIndex, out TChild> : IEnumerable<TChild> where TIndex : notnull
{
  /// <summary>The number of children in this object.</summary>
  int Count { get; }
  /// <summary>Accesses the child stored at <paramref name="index"/>.</summary>
  /// <param name="index">The index or key to lookup.</param>
  /// <returns>The <typeparamref name="TChild"/> at <paramref name="index"/>.</returns>
  TChild this[TIndex index] { get; }
}
