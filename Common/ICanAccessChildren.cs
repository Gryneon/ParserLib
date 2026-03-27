//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

public interface ICanAccessChildren<TIndex, out TChild> : IEnumerable<TChild> where TIndex : notnull
{
  /// <summary>The number of children in this object.</summary>
  int Count { get; }
  TChild this[TIndex index] { get; }
}

/// <summary>Defines a contract for objects that can output their content in a printable format with a specified indentation
/// level.</summary>
/// <remarks>Implementations should use the provided indentation level to format the output appropriately. This
/// interface is typically used to support custom printing or pretty-printing of objects for debugging or display
/// purposes.</remarks>
public interface IPrintable
{
  void Print (int indent);
  void Print () => Print(0);
}
