using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>
/// Represents an opening XML tag.
/// </summary>
public class XMLElementOpen () : XMLNodeAttr, IGeneratable, IXMLObject
{
  /// <inheritdoc/>
  public static XMLElementOpen Generate (TokenObject obj)
  {
    obj.ThrowIfNull();

    XMLElementOpen result = new()
    {
      Tag = obj.Name,
    };

    result.AssignAttributes(obj);

    return result;
  }
}
