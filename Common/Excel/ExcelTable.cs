using System.Data;

namespace Common.Excel;

public class ExcelTable : TableExt
{
  public string Name { get; protected set; }
  protected DataTable DataTable { get; set; }
  public override Dictionary<string, Collection<string>> Data { get; } = [];

  public ExcelTable (ref DataTable table, string? newName = null)
  {
    newName ??= table.TableName;
    Name = newName;
    DataTable = table;
    DataTable.TableName = newName;
    RebuildData();
  }

  protected void RebuildData ()
  {
    Data.Clear();
    foreach (DataColumn col in DataTable.Columns)
    {
      Collection<string> data = [];
      foreach (DataRow row in DataTable.Rows)
        data.Add(row[col] as string ?? SE);
      Data.Add(col.ColumnName, data);
    }
  }
}
