namespace Specification.XML;

/// <summary>Represents an XML attribute.</summary>
public abstract class XMLNodeAttr : XMLNode
{
  /// <summary>The attributes for this XML object.</summary>
  public Collection<XMLAttr> Attributes { get; init; } = [];
}
