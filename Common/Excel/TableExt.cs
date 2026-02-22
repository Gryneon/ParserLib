#if HAS_OFFICE
namespace Common.Excel;

/// <summary>An abstract base class that implements the ITableExt interface, providing a foundation for table-like data structures.</summary>
public abstract class TableExt : ITableExt
{
  public abstract Dictionary<string, Collection<string>> Data { get; }
  public string this[string column, int row] => ((ITableExt) this)[column, row];
  public Collection<string> this[string col] => ((ITableExt) this)[col];

  public Dictionary<string, string> this[int row] => ((ITableExt) this)[row];
}
#endif
