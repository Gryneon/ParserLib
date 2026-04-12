namespace Parser;

public enum CursorType
{
  /// <summary>The cursor key is the current index of the iterative loop.</summary>
  KeyIsIndex,
  /// <summary>The cursor key is <see langword="true"/> while the loop is active.</summary>
  KeyIsBoolean,
  /// <summary>The cursor key is the <see cref="IList{T}"/> of the currently executing loop.</summary>
  KeyIsCollection
}

/// <summary>The data for a loop or cursor object.</summary>
public class CursorData
{
  /// <summary>Creates a new cursor.</summary>
  /// <param name="index">The index of the cursor.</param>
  /// <param name="key">The key to iterate through.</param>
  public CursorData (int index, string key)
  {
    Index = index;
    Key = key;
  }

  /// <summary>The index of the cursor.</summary>
  public int Index { get; set; }
  /// <summary>The key the cursor is operating on.</summary>
  public string Key { get; set; }
}
