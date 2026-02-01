using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

public sealed class XMLComment () : IGeneratable<TokenLabel, XMLComment>, IXMLObject
{
  /// <summary>Comment content.</summary>
  public string Content { get; set; } = SE;
  string IXMLObject.Tag => Content;
  public static XMLComment Generate (TokenLabel obj)
  {
    obj.ThrowIfNull();
    XMLComment result = new()
    {
      Content = obj?.Name ?? SE,
    };

    return result;
  }
}
