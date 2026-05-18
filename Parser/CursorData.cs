namespace Parser;

/// <summary>The data for a loop or cursor object.</summary>
public class CursorData
{
  public XParser Parser { get; set; }
  public int Length
  {
    get => field = ListKey is null ? field : Parser.Data.GetCountOfKey(ListKey);
    set;
  }
  public object? This => ListKey is null ? null :
    Parser.Data.TryLoadArray(ListKey, out IEnumerable<object>? list) ? new List<object>(list)[Index] : null;
  /// <summary>The index of the cursor.</summary>
  public int Index { get; set; }
  /// <summary>The key the cursor is operating on.</summary>
  public string? ListKey { get; }
  public bool AtEnd => Index >= Length;

  /// <summary>Creates a new cursor.</summary>
  /// <param name="parser">The parser reference.</param>
  /// <param name="index">The index of the cursor.</param>
  /// <param name="list_key">The key to iterate through.</param>
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
  public CursorData (XParser parser, int index, int length)
  {
    Index = index;
    Length = length;
    Parser = parser;
  }
  /// <summary>Creates a new cursor without a constraint.</summary>
  /// <param name="parser">The parser reference.</param>
  public CursorData (XParser parser)
  {
    Parser = parser;
    Index = -1;
    Length = -1;
  }
}
