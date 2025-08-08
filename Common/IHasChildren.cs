//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

/// <summary>
/// An interface that indicates that an object contains children.
/// </summary>
/// <typeparam name="TChild">The type of child that this object contains.</typeparam>
public interface IHasChildren<in TChild>
{
  /// <summary>
  /// The number of children in this object.
  /// </summary>
  int Count { get; }
  /// <summary>
  /// Adds a child.
  /// </summary>
  /// <param name="child">The child to add.</param>
  void Add (TChild child);
  /// <summary>
  /// Adds multiple children.
  /// </summary>
  /// <param name="children">The children to add.</param>
  void AddRange (IEnumerable<TChild> children)
  {
    foreach (TChild child in children)
    {
      Add(child);
    }
  }
}
