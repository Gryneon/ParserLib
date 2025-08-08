using Microsoft.Office.Interop.Excel;

using XlRange = Microsoft.Office.Interop.Excel.Range;

namespace Common.Excel;

public class SheetExt (Worksheet sheet) : ISheetExt
{
  public Worksheet XlSheet => sheet;
}

public interface ISheetExt
{
  Worksheet XlSheet { get; }
  Collection<string> GetRangeValues (Worksheet sheet, Range rng)
  {
    List<string> result = [];
    List<XlRange> cells = [];
    rng.Nodes.ForEach(i => cells.AddRange(sheet.Range[i.MinAddress, i.MaxAddress].Cells));

    return [.. result];
  }
  Range GetRange (Worksheet sheet, string address) => new(address);
}