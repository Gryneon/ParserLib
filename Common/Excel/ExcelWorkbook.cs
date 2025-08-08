﻿using System.Data;

using DataTable = System.Data.DataTable;

using ListColumn = Microsoft.Office.Interop.Excel.ListColumn;
using ListObject = Microsoft.Office.Interop.Excel.ListObject;
using ListRow = Microsoft.Office.Interop.Excel.ListRow;
using Name = Microsoft.Office.Interop.Excel.Name;
using Names = Microsoft.Office.Interop.Excel.Names;
using QueryTable = Microsoft.Office.Interop.Excel.QueryTable;
using Sheet = Microsoft.Office.Interop.Excel.Worksheet;
using Style = Microsoft.Office.Interop.Excel.Style;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using XlRange = Microsoft.Office.Interop.Excel.Range;

namespace Common.Excel;

public class ExcelWorkbook
{
  public string? Path { get; }
  public Workbook Workbook { get; }
  public Dictionary<string, Name> Names { get; } = [];
  public Dictionary<string, ExcelTable> Tables { get; } = [];
  public Dictionary<string, QueryTable> Queries { get; } = [];
  public Dictionary<string, Style> Styles { get; } = [];
  public Dictionary<string, ISheetExt> Sheets { get; } = [];
  public ExcelWorkbook (string? path)
  {
    Path = path;
    Workbook = App.Instance.Workbooks.Open(path);
    Import();
  }
  protected static void MakeDataColumn (ref DataTable dt, ListColumn col)
  {
    DataColumn dc = new()
    {
      ColumnName = col.Name,
      Caption = col.Name
    };
    dt.Columns.Add(dc);
  }
  protected void Import ()
  {
    foreach (Sheet sheet in Workbook.Worksheets)
    {
      Sheets.Add(sheet.Name, new SheetExt(sheet));
      foreach (ListObject listobj in sheet.ListObjects)
      {
        DataTable dt = new()
        {
          TableName = listobj.Name
        };

        foreach (ListColumn col in listobj.ListColumns)
        {
          MakeDataColumn(ref dt, col);
        }
        foreach (ListRow row in listobj.ListRows)
        {
          int cIndex = 0;
          DataRow dr = dt.NewRow();
          foreach (XlRange cell in row.Range.Cells)
          {
            object value = cell.Value;
            dr[cIndex++] = value;
          }
          dt.Rows.Add(dr);
        }

        Tables.Add(dt.TableName, new(ref dt));
      }
    }
    foreach (QueryTable qt in Workbook.Queries)
      Queries.Add(qt.Name, qt);
    foreach (Style s in Workbook.Styles)
      Styles.Add(s.Name, s);
    foreach (Name nm in Workbook.Names)
      Names.Add(nm.Name, nm);
  }
}
