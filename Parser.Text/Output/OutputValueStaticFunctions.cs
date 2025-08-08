namespace Parser.Text.Output;

/// <summary>
/// Static functions useful when defining output values in a specification.
/// </summary>
public static class OutputValueStaticFunctions
{
  public static OutputValueStatic OVS (string varname, string groupName, object value) => new(varname, groupName, value);
  public static OutputValueContent OVC (string varname, string groupName) => new(varname, groupName);
  public static OutputValueParse OVP (string varname, string groupName, string format) => new(varname, groupName, format);
}