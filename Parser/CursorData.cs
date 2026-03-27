namespace Parser;

/// <summary>The data for a loop or cursor object.</summary>
public class CursorData
{
  /// <summary>Creates a new cursor and sets itself as the last accessed.</summary>
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
