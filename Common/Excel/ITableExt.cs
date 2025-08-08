using System.Data;

namespace Common.Excel;

public interface ITableExt
{
  Dictionary<string, Collection<string>> Data { get; }

  string this[(string Column, int Row) t] => Data[t.Column][t.Row];
  Collection<string> this[string col] => Data[col];

  Dictionary<string, string> this[int row] => [.. Data.Select(item => new KeyValuePair<string, string>(item.Key, item.Value[row]))];
}
