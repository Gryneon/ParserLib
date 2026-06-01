namespace Parser;

public class PositionalItemData
{
  public string Name { get; set; } = SE;
  public int Position { get; set; } = -1;
  public int Length => Data.Length;
  public Memory<byte> Data { get; set; }

  public PositionalItemData (params object[] parameters)
  {
    int the_pos = -1, the_pos2 = -1;

    foreach (object p in parameters)
    {
      if (p is int pos && the_pos == -1)
      {
        the_pos = pos;
      }
      if (p is int alt && the_pos != -1)
      {
        the_pos2 = alt;
      }
      if (p is string s)
      {
        Name = s;
      }
      if (p is Memory<byte> mem)
      {
        Data = mem;
      }
    }

    Position = the_pos == Data.Length ? the_pos2 : the_pos;
  }
}

/// <summary>The data for a loop or cursor object.</summary>
public class CursorData
{
  public required XParser? Parser { get; set; }
  public int Length
  {
    get => field = ListKey is null ? field : Parser?.Data.GetCountOfKey(ListKey) ?? 0;
    set;
  }
  public object? This => ListKey is null ? null :
    (Parser?.Data.TryLoadArray(ListKey, out IEnumerable<object>? list) ?? false) ? new List<object>(list)[Index] : null;
  /// <summary>The index of the cursor.</summary>
  public int Index { get; set; }
  /// <summary>The key the cursor is operating on.</summary>
  public string? ListKey { get; init; }
  public bool AtEnd => Index >= Length;

  #region Constructors
  public CursorData () { }
  /// <summary>Creates a new cursor.</summary>
  /// <param name="parser">The parser reference.</param>
  /// <param name="index">The index of the cursor.</param>
  /// <param name="list_key">The key to iterate through.</param>
  [SetsRequiredMembers]
  public CursorData (XParser parser, int index, string? list_key = null)
  {
    Index = index;
    ListKey = list_key;
    Parser = parser;
  }
  /// <summary>Creates a new cursor.</summary>
  /// <param name="parser">The parser reference.</param>
  /// <param name="index">The index of the cursor.</param>
  /// <param name="length">The maximum iterations.</param>
  [SetsRequiredMembers]
  public CursorData (XParser parser, int index, int length)
  {
    Index = index;
    Length = length;
    Parser = parser;
  }
  /// <summary>Creates a new cursor without a constraint.</summary>
  /// <param name="parser">The parser reference.</param>
  [SetsRequiredMembers]
  public CursorData (XParser parser)
  {
    Parser = parser;
    Index = DNE;
    Length = DNE;
  }
  #endregion
}
