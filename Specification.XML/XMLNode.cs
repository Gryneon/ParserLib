namespace Specification.XML;

/// <summary>An XML node.</summary>
public abstract class XMLNode : IXMLObject
{
  /// <summary>The name of the element.</summary>
  public string Tag { get; set; } = SE;
  public string? Namespace { get; set; }
  public abstract string Serialize ();
}
