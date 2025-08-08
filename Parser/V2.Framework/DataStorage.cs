using Common.Extensions;

namespace Parser.V2.Framework;

public class DataStorage (object initial_file_contents)
{
  public object CurrentData { get; protected set; } = initial_file_contents;
  protected Dictionary<string, object> KeyedData { get; init; } = [("initial", initial_file_contents).ToKVP()];
  public IDataFormat? CurrentFormat { get; set; }

  public void AssignOutput (object operation_output)
  {
    if (operation_output is null)
      return;
    CurrentData = operation_output;
  }
  public void AssignKeyedData (string key, object operation_output)
  {
    if (operation_output is null || key is null)
      return;
    KeyedData.Add(key, operation_output);
  }
  public object? GetKeyedData (string key) =>
    !KeyedData.TryGetValue(key, out object? value) ? null : value;
}
