using Parser.Tokens;

namespace Specification.XML;

/// <summary>Represents an XML header.</summary>
public class XMLHeader () : XMLNodeAttr, IGeneratable, IXMLObject
{
  /// <inheritdoc/>
  public static XMLHeader Generate (TokenObject obj)
  {
    XMLHeader result = new()
    {
      Tag = "xml"
    };

    result.AssignAttributes(obj);

    return result;
  }
}
