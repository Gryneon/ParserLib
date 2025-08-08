namespace Common.Excel;

public abstract class TableExt : ITableExt
{
  public abstract Dictionary<string, Collection<string>> Data { get; }
  public string this[(string Column, int Row) t] => ((ITableExt)this)[(t.Column, t.Row)];
  public Collection<string> this[string col] => ((ITableExt)this)[col];

  public Dictionary<string, string> this[int row] => ((ITableExt)this)[row];
}
