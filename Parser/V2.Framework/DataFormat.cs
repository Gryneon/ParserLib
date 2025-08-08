namespace Parser.V2.Framework;

/// <summary>
/// Describes a data syntax or format.
/// </summary>
public abstract class DataFormat (string name) : IDataFormat
{
  public string Name { get; init; } = name;
  public abstract DataFormatType Type { get; }
  public required Collection<IOperation> Operations { get; init; }
  public required Type ExpectedResult { get; init; }
  public Dictionary<string, IDataItem> FormatData { get; } = [];
}
