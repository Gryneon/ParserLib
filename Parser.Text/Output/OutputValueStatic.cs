namespace Parser.Text.Output;

public struct OutputValueStatic (string varName, string groupName, object value) : IOutputValue
{
  public string VarName { get; set; } = varName;
  public string GroupName { get; set; } = groupName;
  public object Value { get; set; } = value;
  public static OutputValueStatic operator ! (OutputValueStatic item) => new(item.VarName, item.GroupName, item.Value is bool b ? !b : item.Value);
}
