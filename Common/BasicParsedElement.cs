#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Xml.Linq;

namespace Common;

/// <summary>A basic attribute/element dictionary.</summary>
public class BasicParsedElement
{
  /// <summary>The element name.</summary>
  public string Name { get; }
  /// <summary>The attributes of this element.</summary>
  public Dictionary<string, string> Attributes { get; } = [];
  /// <summary>The child elements of this element.</summary>
  public Collection<BasicParsedElement> Elements { get; } = [];
  /// <summary>The value if it contains a value not elements.</summary>
  public string? Value { get; }

  public BasicParsedElement (XElement element)
  {
    Name = element.Name.LocalName;

    foreach (XAttribute a in element.Attributes())
    {
      Attributes.Add(a.Name.LocalName, a.Value);
    }

    if (element.HasElements)
    {
      foreach (XElement e in element.Elements())
      {
        Elements.Add(new(e));
      }
    }
    else if (element.Value.IsNotEmpty)
    {
      Value = element.Value;
    }
  }

  public BasicParsedElement this[int index]
  {
    get => Elements[index];
  }

  public BasicParsedElement this[int index, string ofType]
  {
    get => Elements.Where(e => e.Name.Is(ofType)).At(index);
  }

  /// <summary>Looks up and retrieves the attribute value as a <see langword="string"/>.</summary>
  /// <param name="attribute">The attribute to lookup.</param>
  /// <returns>The attribute value as a <see langword="string"/>, or an empty string if there is no attribute of that name.</returns>
  public string this[string attribute]
  {
    get => Attributes.TryGetValue(attribute, out string? value) ? value : SE;
  }
}
