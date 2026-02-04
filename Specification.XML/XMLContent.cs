using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>
/// Represents text content in the XML file.
/// </summary>
public sealed class XMLContent () : IGeneratable, IXMLObject
{
  /// <summary>
  /// Text content between tags.
  /// </summary>
  public string Content { get; set; } = SE;
  /// <inheritdoc/>
  string IXMLObject.Tag => Content;

  /// <inheritdoc/>
  public static XMLContent Generate (TokenTypedValue ttv)
  {
    ttv.ThrowIfNull();
    XMLContent result = new()
    {
      Content = ttv.Content,
    };

    return result;
  }
}
