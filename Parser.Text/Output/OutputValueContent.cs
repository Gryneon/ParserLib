namespace Parser.Text.Output;

/// <summary>
/// Stores an output value for generating objects.
/// This one stores the content in a group and stores it in a field.
/// </summary>
/// <param name="varName">The field name.</param>
/// <param name="groupName">The group name in the <see cref="MatchDataDictionary"/>.</param>
public struct OutputValueContent (string varName, string groupName) : IOutputValue
{
  public string VarName { get; set; } = varName;
  public string GroupName { get; set; } = groupName;
}
