
namespace Parser.V2.Framework;

public class XMLDataFormat (string name) : DataFormat(name)
{
  public override DataFormatType Type => DataFormatType.Text;
}
