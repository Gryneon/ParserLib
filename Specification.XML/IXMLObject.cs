namespace Specification.XML;

/// <summary>Represents an XML structure</summary>
public interface IXMLObject : ITextSerializer
{
  /// <summary>A tag name.</summary>
  string Tag { get; }
}
