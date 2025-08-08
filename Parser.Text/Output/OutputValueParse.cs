namespace Parser.Text.Output;

public struct OutputValueParse (string varName, string groupName, string value) : IOutputValue
{
  public string VarName { get; set; } = varName;
  public string GroupName { get; set; } = groupName;
  public string ParseType { get; set; } = value;
}
