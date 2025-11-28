namespace Parser;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ExportAttribute : Attribute
{
  public string FormatName { get; }

  public ExportAttribute (string formatName) => FormatName = formatName;
}
