using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>Represents an XML header.</summary>
public class XMLHeader () : XMLNodeAttr, IXMLObject
{
  public override string Serialize () => $"<?{Tag} {Attributes.TextJoin()}?>";
  public override string ToString () => Serialize();
}
