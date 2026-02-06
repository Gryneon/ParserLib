namespace Parser;

/// <summary>
/// The data for a loop or cursor object.
/// </summary>
public class CursorData
{
  /// <summary>Creates a new cursor and sets itself as the last accessed.</summary>
  /// <param name="index">The index of the cursor.</param>
  /// <param name="key">The key to iterate through.</param>
  /// <param name="data">The reference to the <see cref="DataDictionary"/> where the keyed data is stored.</param>
  public CursorData (int index, string key, DataDictionary data)
  {
    Index = index;
    Key = key;
    Data = data;
    Last = this;
  }

  /// <summary>The index of the cursor.</summary>
  public int Index { get; set; }
  /// <summary>The key the cursor is operating on.</summary>
  public string Key { get; set; }
  /// <summary>A reference to the data dictionary the parser is using.</summary>
  public DataDictionary Data { get; }
  /// <summary>The object referenced.</summary>
  public object Cursor => Data[Key].AsCollection()[Index];
  /// <summary>The outermost created cursor.</summary>
  public static CursorData? Last { get; private set; }
}
