//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

/// <summary>The procedure to follow when writing to a dictionary.</summary>
public enum DictionaryMode
{
  /// <summary>Overwrite the previous value (default).</summary>
  Overwrite,
  /// <summary>Ignore the write if there is already a value stored.</summary>
  Ignore,
  /// <summary>Convert the items into a collection, or add the item to the collection.</summary>
  MakeList
}
