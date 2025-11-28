namespace Parser;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DefinitionExportAttribute : Attribute
{
  public DefinitionExportAttribute (bool multiple = false)
  {
    Multiple = multiple;
  }

  public bool Multiple { get; }
}
