namespace Specification.XML;

/// <summary>Represents a closing XML element.</summary>
public class XMLElementClose () : XMLNode, IXMLObject
{
  public override string Serialize () => $"</{Tag}>";
}

/// <summary>Represents a paired XML element open and close with content.</summary>
public class XMLElement () : XMLNodeAttr, IXMLObject
{
  /// <summary>The contents of this XML node.</summary>
  public Collection<IXMLObject> Content { get; } = [];
  public override string Serialize () => $"<{Tag} {Attributes.TextJoin(" ")}>{Content.TextJoin(" ")}</{Tag}>";
  public override string ToString () => Serialize();
}
