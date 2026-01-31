using System.Xml.Linq;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>
/// Represents an XML header.
/// </summary>
public class XMLHeader () : XMLNodeAttr, IGeneratable<TokenObject, XMLHeader>, IXMLObject
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
