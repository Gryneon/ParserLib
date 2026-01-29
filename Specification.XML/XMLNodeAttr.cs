using System;

using Parser.Tokens;

namespace Specification.XML;

/// <summary>
/// Represents an XML attribute.
/// </summary>
public abstract class XMLNodeAttr : XMLNode
{
  /// <summary>
  /// The attributes for this XML object.
  /// </summary>
  public Collection<XMLProperty> Attributes { get; } = [];
  /// <summary>
  /// Assigns attributes based on a <see cref="MatchDataSet"/>.
  /// </summary>
  /// <param name="obj"></param>
  protected void AssignAttributes (TokenObject obj)
  {
    if (obj is null || obj.Properties.Count == 0)
      return;

    for (int i = 0; i < obj.Properties.Count; i++)
    {
      TokenProperty? prop = obj.Properties[i] as TokenProperty;
      _ = prop ?? throw new InvalidCastException("TokenObject had an improper property stored.");
      XMLProperty attr = new()
      {
        Key = prop.Name,
        Value = prop.Value
      };
      Attributes.Add(attr);
    }
  }
  /// <summary>
  /// The contents of this XML node.
  /// </summary>
  public Collection<IXMLObject> Children { get; } = [];
}
