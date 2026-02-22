using Common.Extensions;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>Represents a single self closing XML tag.</summary>
public class XMLElementSingle () : XMLNodeAttr, IXMLObject
{
  public override string Serialize () => $"<{Tag} {Attributes.TextJoin(" ")}/>";
  public override string ToString () => Serialize();
}
