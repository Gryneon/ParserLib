using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>
/// Represents a closing XML element.
/// </summary>
public class XMLElementClose () : XMLNode, IGeneratable, IXMLObject
{
  /// <inheritdoc/>
  public static XMLElementClose Generate (TokenLabel label)
  {
    label.ThrowIfNull();
    return new() { Tag = label?.Name ?? SE };
  }
}
