namespace Parser.V2.Framework;

public interface IDataFormat
{
  string Name { get; }
  DataFormatType Type { get; }
  Type ExpectedResult { get; }
  Collection<IOperation> Operations { get; }
  abstract Dictionary<string, IDataItem> FormatData { get; }
}