//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

/// <summary>The procedure to follow when writing to a dictionary.</summary>
[Flags]
public enum DictionaryMode
{
  /// <summary>Default behavior, normally overwrite.</summary>
  None,
  /// <summary>Overwrite the previous value.</summary>
  Overwrite = 0x1,
  /// <summary>Ignore the write if there is already a value stored.</summary>
  Ignore = 0x2,
  /// <summary>If the existing value is a collection of the same type, add it to the collection.</summary>
  AddToCollection = 0x100,
  /// <summary>If the existing value is of the same type, make the value a collection of the type containing both values.</summary>
  MakeCollection = 0x200,
  /// <summary>If the existing value is a collection, and this value is a collection of the same type, merge the two collections.</summary>
  MergeCollection = 0x400,
}
