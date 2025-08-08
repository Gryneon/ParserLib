
namespace Parser.V2.Framework;

public class ByteDataFormat (string name) : DataFormat(name)
{
  /// <inheritdoc/>
  public override DataFormatType Type => DataFormatType.Byte;
}
