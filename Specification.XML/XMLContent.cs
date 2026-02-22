using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>Represents text content in the XML file.</summary>
public sealed class XMLContent () : IXMLObject
{
  /// <summary>Text content between tags.</summary>
  public string Content { get; set; } = SE;
  string IXMLObject.Tag => Content;

  public string Serialize () => Content;
  public override string ToString () => Serialize();
}
