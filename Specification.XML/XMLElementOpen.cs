using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>Represents an opening XML tag.</summary>
public class XMLElementOpen () : XMLNodeAttr, IXMLObject
{
  public override string Serialize () => $"<{Tag} {Attributes.TextJoin(" ")}>";
  public override string ToString () => Serialize();
  public XMLElementClose ClosingElement => new() { Tag = Tag };
}
