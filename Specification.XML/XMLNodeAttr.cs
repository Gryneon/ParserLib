namespace Specification.XML;

/// <summary>
/// Represents an XML attribute.
/// </summary>
public abstract class XMLNodeAttr : XMLNode
{
  /// <summary>
  /// The attributes for this XML object.
  /// </summary>
  public Collection<XMLAttribute> Attributes { get; } = [];
  /// <summary>
  /// Assigns attributes nased on a <see cref="MatchDataDictionary"/>.
  /// </summary>
  /// <param name="mdd"></param>
  protected void AssignAttributes (MatchDataDictionary mdd)
  {
    if (!mdd.HasGroup("attrname") || mdd["attrname"].Count == 0)
      return;

    for (int i = 0; i < mdd["attrname"].Count; i++)
    {
      XMLAttribute attr = new()
      {
        Key = mdd["attrname"][i].Content,
        Value = mdd["attrval"][i].Content
      };
      Attributes.Add(attr);
    }
  }
  /// <summary>
  /// The contents of this XML node.
  /// </summary>
  public Collection<IXMLObject> Children { get; } = [];
}
