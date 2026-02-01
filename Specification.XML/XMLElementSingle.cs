using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>
/// Represents a single self closing XML tag.
/// </summary>
public class XMLElementSingle () : XMLNodeAttr, IGeneratable<TokenObject, XMLElementSingle>, IXMLObject
{
  /// <inheritdoc/>
  public static XMLElementSingle Generate (TokenObject obj)
  {
    obj.ThrowIfNull();

    XMLElementSingle result = new()
    {
      Tag = obj.Name
    };

    result.AssignAttributes(obj);

    return result;
  }
}
