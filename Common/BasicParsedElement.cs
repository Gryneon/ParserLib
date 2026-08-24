#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Data;
using System.Xml.Linq;

using BT = Common.BasicType;

namespace Common;

/// <summary>The type of object.</summary>
public enum BasicType
{
  /// <summary>Invalid text. Unable to parse.</summary>
  Invalid = -1,
  /// <summary>This returns when you try to get a value that doesn't exist.</summary>
  Absent = 0,
  /// <summary>This gets removed by the second stage parser.</summary>
  Placeholder = 2,
  Document = 3,
  #region JSON
  /// <summary>The value 'null'.</summary>
  /// <remarks>JSON <see langword="null"/> value.</remarks>
  Null,
  /// <summary>Quoted text.</summary>
  /// <remarks>JSON values, JSON keys, XML Attribute Values, INI </remarks>
  String,
  /// <summary>Non-quoted numeric data.</summary>
  Number,
  /// <summary>An array of <see cref="IParsedEntity"/> items.</summary>
  Array,
  /// <summary>A basic dictionary.</summary>
  Object,
  /// <summary>A <see langword="true"/> or a <see langword="false"/> stored as 'true' and 'false'.</summary>
  Boolean,
  #endregion
  #region XML
  /// <summary>This is the starting and ending whitespace in an element.</summary>
  IgnoredWhitespace,
  /// <summary>This is content within a mixed element.</summary>
  LooseContent,
  /// <summary>A complex object that stores attributes, elements, and content.</summary>
  Element,
  Attribute,
  #endregion
}

public static class BasicTypeExt
{
  extension(BT type)
  {
    public bool IsPrimitive => type is BT.Number or BT.String or BT.Boolean;
    public bool IsDictionary => type is BT.Object or BT.Element;
    public bool IsCollection => type is BT.Array or BT.Element or BT.Object;
  }
}

public interface IParsedEntity
{
  string? Origin { get; }
  BT Type { get; }
  IParsedEntity? Parent { get; }
  bool Equals (IParsedEntity? other);
  bool Equals (object? obj);
  int GetHashCode ();
  string ToString ();
  /// <summary>Sets the parent property after the type has been contructed.</summary>
  /// <param name="parent">The parent or encompassing object.</param>
  void SetParent (IParsedEntity parent);
}

public abstract class ParsedEntity : IParsedEntity, IEquatable<IParsedEntity>
{
  private IParsedEntity? _parent;

  public virtual required string? Origin { get; init; }
  public abstract BT Type { get; }
  public IParsedEntity? Parent { get => _parent; init => _parent = value; }
  public abstract bool Equals (IParsedEntity? other);
  public abstract override string ToString ();
  public void SetParent (IParsedEntity? parent) => _parent = parent;
}

public class StringEntity : ParsedEntity
{
  public required string Value { get; init; }
  public override BT Type => BT.String;

  public override bool Equals (IParsedEntity? other) =>
    other is StringEntity entity && Value.Equals(entity.Value, SCO);
  public override string ToString () => $"\"{Value}\"";
}
/// <summary>An entity representing an xml document.</summary>
public class XMLDocumentEntity : ParsedEntity
{
  /// <summary>The entire text contents of the file.</summary>
  public required string Content { get; init; }
  public IParsedEntity? Header { get; protected set; }
  public IParsedEntity? RootNode { get; protected set; }
  public override BT Type => BT.Document;

  public override bool Equals (IParsedEntity? other) =>
    other is XMLDocumentEntity de && Content.Equals(de.Content, SCO);
  public override string ToString () => Content;
  public void SetHeader (IParsedEntity? header) => Header = header;
  public IParsedEntity? SetRoot (IParsedEntity? root) => RootNode = root;
}
/// <summary>An entity representing a number or decimal.</summary>
public class NumberEntity : ParsedEntity
{
  public required decimal Value { get; init; }
  public override BT Type => BT.Number;

  public override bool Equals (IParsedEntity? other) => other is NumberEntity ne && ne.Value == Value;
  public override string ToString () => $"{Value}";
}
public class NullEntity : ParsedEntity
{
  public override BT Type => BT.Null;

  public override bool Equals (IParsedEntity? other) => other is NullEntity;
  public override string ToString () => "null";
}
public class AttributeEntity : ParsedEntity
{
  public override BT Type => BT.Attribute;
  public string? Namespace { get; init; }
  public required string Key { get; init; }
  public required string Value { get; init; }

  public override bool Equals (IParsedEntity? other) =>
    other is AttributeEntity ae &&
    Key.Equals(ae.Key, SCO) &&
    Value.Equals(ae.Value, SCO) &&
    (Namespace.IsEmpty && ae.Namespace.IsEmpty || (Namespace?.Equals(ae.Namespace, SCO) == true));
  public override string ToString () => $"{Key}=\"{Value}\"";
}
public class ElementEntity : ParsedEntity
{
  private readonly Collection<IParsedEntity> _attributes = [];
  private readonly Collection<IParsedEntity> _children = [];
  public ReadOnlyCollection<IParsedEntity> Attributes
  {
    get => [.. _attributes];
    init => _attributes.AddRange(value);
  }
  public ReadOnlyCollection<IParsedEntity> Children
  {
    get => [.. _children];
    init => _children.AddRange(value);
  }
  public string? Namespace { get; init; }
  public required string Name { get; init; }
  public bool IsHeader { get; init; }
  public override BT Type => BT.Null;

  public override string ToString ()
  {
    string attrs = Attributes.Select(child => child.ToString()).TextJoin(" ");
    string children = Children.Select(child => child.ToString()).TextJoin(Chars.LFs);

    if (IsHeader)
      return $"<?xml {attrs}?>";

    string elem = $"<{Name} {attrs}";

    if (Children.Count == 0)
    {
      return elem + " />";
    }

    return elem + ">" + children + $"</{Name}>";
  }
  public void AddAttribute (IParsedEntity attribute) => _attributes.Add(attribute);
  public void AddAttributes (IEnumerable<IParsedEntity> attributes) => _attributes.AddRange(attributes);
  public void AddChild (IParsedEntity child) => _children.Add(child);
  public void AddChildren (IEnumerable<IParsedEntity> children) => _children.AddRange(children);
  public override bool Equals (IParsedEntity? other) =>
    other is ElementEntity ee &&
    Attributes.SequenceEqual(ee.Attributes) &&
    Children.SequenceEqual(ee.Children) &&
    Name.Equals(ee.Name, SCO);
  public void AssignParent ()
  {
    foreach (IParsedEntity entity in Children)
    {
      entity.SetParent(this);
    }

    foreach (IParsedEntity entity in Attributes)
    {
      entity.SetParent(this);
    }
  }
}
public class ElementClosePlaceholder : ParsedEntity
{
  public required string Name { get; init; }
  public string? Namespace { get; init; }
  public override BT Type => BT.Placeholder;

  public override bool Equals (IParsedEntity? other) =>
    other is ElementClosePlaceholder ecp &&
    Name.Is(ecp.Name) &&
    (Namespace is null && ecp.Namespace is null || Namespace.Is(ecp.Namespace));
  public override string ToString () => $"</{Name}>";
}
public class ElementOpenPlaceholder : ParsedEntity
{
  private readonly Collection<IParsedEntity> _attributes = [];

  public required string Name { get; init; }
  public ReadOnlyCollection<IParsedEntity> Attributes
  {
    get => [.. _attributes];
    init => _attributes.AddRange(value);
  }
  public string? Namespace { get; init; }
  public override BT Type => BT.Placeholder;

  public override bool Equals (IParsedEntity? other) =>
    other is ElementOpenPlaceholder eop &&
    Name.Is(eop.Name) &&
    (Namespace is null && eop.Namespace is null || Namespace.Is(eop.Namespace));
  public override string ToString () => $"</{Name}>";

}
public class ContentEntity : ParsedEntity
{
  public required string Content { get; init; }
  public override BT Type => BT.LooseContent;

  public override bool Equals (IParsedEntity? other) =>
    other is ContentEntity ce && Content.Is(ce.Content);
  public override string ToString () => Content;
}
public class WhitespaceEntity : ParsedEntity
{
  public required string Content { get; init; }
  public override BT Type => BT.IgnoredWhitespace;

  public override bool Equals (IParsedEntity? other) =>
    other is ContentEntity ce && Content.Is(ce.Content);
  public override string ToString () => Content;
}
public class EntityFactory
{
  private static XMLDocumentEntity? Document;
  private static ElementEntity? Current;
  private static readonly string? Origin;

  /// <summary>JSON Tokenizing Regex</summary>
  [SS("regex")]
  private const string JSONRegex =
    """
    (?#primitives)
    (?'key' " (?'keyname'\w+) " (?=\s*[:=])) |
    (?'strvalue'  (?<=[:=]\s*) " (?'value'([^\\"]|\\.)*) " ) |
    (?'numvalue'  (?<=[:=]\s*)   (?'value'[0-9.eExXbB]+ )  ) |
    (?'boolvalue' (?<=[:=]\s*)   (?'value'true|false)      ) |
    (?'nullvalue' (?<=[:=]\s*)   (?'value'null)            ) |
    (?#operators)
    (?'Ao' \[) |
    (?'Ac' \]) |
    (?'Bo' \{) |
    (?'Bc' \}) |
    (?'Cm' \,) |
    (?'Eq' [=:]) |
    (?#commments)
    (?'comment'   \/\/.* ) |
    (?'comment'   \/\*([^*]|\*[^/])*\*\/ )
    """;

  [SS("regex")]
  private const string XMLRegex =
    """
    (?# Element Piece)
    (?'element'
      <
      (?# '?' for header definition)
      (?'header'\?)?
      \s*
      (?'close'\/)?
      \s*
      (?# optional namespace)
      ((?'ns'\w+):)?
      (?'name'\w+)

      (?# attributes)
      (
        \s+
        (?'attribute'
          (
            (?'attrns'\w+)
            :
          )?
          (?'attrname'\w+)
          \s*
          =
          \s*
          ""(?'attrval'([^\n""\\]|\\[^\n])*)""
        )
      )*

      (?'single'\s*\/)?
      \s*
      (?# '?' for ending the header definition)
      (\k'header')?
      >
    ) |

    (?# Leading or Trailing Whitespace)
    (?'ws'(?<=\>)\s+(?=[^<\s])) |

    (?# Leading or Trailing Whitespace)
    (?'content'(?<=\>\s*)[^<]+?(?=\s*<)) |
    (?# XML Comment)
    (?'comment'<!-- ([^-]| -[^-]) -->)
    """;

  private static Collection<IParsedEntity> ParseAttributes (Match match)
  {
    Collection<string> origins = [.. match.Groups["attributes"].Captures.Select(c => c.Value)];
    Collection<string> keys = [.. match.Groups["attrname"].Captures.Select(c => c.Value)];
    Collection<string> values = [.. match.Groups["attrval"].Captures.Select(c => c.Value)];
    int count = origins.Count;
    if (keys.Count == count && values.Count == count)
    {
      Collection<IParsedEntity> attributes = [];
      for (int i = 0; i < count; i++)
      {
        attributes.Add(new AttributeEntity() { Key = keys[i], Value = values[i], Origin = origins[i] });
      }
      return attributes;
    }
    else
    {
      throw new InvalidOperationException($"Keys ({keys.Count}) and Values ({values.Count}) do not match Origin Count ({count}).");
    }
  }
  private static Collection<IParsedEntity> ParseAttributes (XElement element)
  {
    Collection<IParsedEntity> result = [];
    foreach (XAttribute attr in element.Attributes())
    {
      result.Add(new AttributeEntity()
      {
        Key = attr.Name.LocalName,
        Value = attr.Value,
        Origin = attr.ToString(),
        Parent = Current,
        Namespace = attr.Name.NamespaceName.IsEmpty ? null : attr.Name.NamespaceName
      });
    }
    return result;
  }
  private static ElementEntity GetHeader (Match match) => new()
  {
    IsHeader = true,
    Name = "xml",
    Origin = match.Value,
    Attributes = [.. ParseAttributes(match)],
  };
  private static ContentEntity GetContent (Match match) => new()
  {
    Content = match.Value,
    Origin = match.Value
  };
  private static WhitespaceEntity GetWhitespace (Match match) => new()
  {
    Content = match.Value,
    Origin = match.Value
  };
  private static ElementClosePlaceholder GetClose (Match match) => new()
  {
    Name = match.Groups["name"].Value,
    Namespace = match.HasValidGroup("ns") ? match.Groups["ns"].Value : null,
    Origin = match.Value,
  };
  private static ElementOpenPlaceholder GetOpen (Match match) => new()
  {
    Name = match.Groups["name"].Value,
    Namespace = match.HasValidGroup("ns") ? match.Groups["ns"].Value : null,
    Origin = match.Value,
    Attributes = [.. ParseAttributes(match)]
  };
  private static ElementEntity GetElement (Match match) => new()
  {
    Name = match.Groups["name"].Value,
    Namespace = match.HasValidGroup("ns") ? match.Groups["ns"].Value : null,
    Origin = match.Value,
    Attributes = [.. ParseAttributes(match)]
  };
  private static IParsedEntity ElementSelector (Match match)
  {
    if (match.HasValidGroup("header"))
    {
      return GetHeader(match);
    }
    if (match.HasValidGroup("close"))
    {
      return GetClose(match);
    }
    if (match.HasValidGroup("single"))
    {
      return GetElement(match);
    }
    return GetOpen(match);
  }
  public static IParsedEntity CheckMatch (Match match)
  {
    if (!match.Success) throw new InvalidOperationException("Match was not a success.");

    if (match.HasValidGroup("element"))
      return ElementSelector(match);

    if (match.HasValidGroup("content"))
      return GetContent(match);

    if (match.HasValidGroup("ws"))
      return GetWhitespace(match);

    throw new InvalidOperationException("The groups needed to process this item are missing.");
  }

  public static IParsedEntity FromXElement (XElement root)
  {
    Document = new XMLDocumentEntity()
    {
      Origin = root.Value,
      Content = root.Value,
    };

    Current = new()
    {
      Name = root.Name.LocalName,
      Origin = root.Value,
      Parent = (IParsedEntity?) Current ?? Document,
      Attributes = [.. ParseAttributes(root)],
      Namespace = root.Name.NamespaceName.IsEmpty ? null : root.Name.NamespaceName,
    };
    Document.SetRoot(Current);
    Current.AddChildren([.. root.Elements().Select(FromXElement)]);

    return Document;
  }

  public static IParsedEntity ProduceAll (string content, BT type)
  {
    IParsedEntity last;

    if (type == BT.Null)
      return new NullEntity() { Origin = content };

    if (type == BT.Element)
    {

      Collection<IParsedEntity> entities = [];
      foreach (Match match in Regex.Matches(content, XMLRegex, ROEC | ROIPW | ROML))
      {
        last = CheckMatch(match);
        entities.Add(last);
      }

      Collection<ElementEntity> inside = [];

      Document = new XMLDocumentEntity()
      {
        Origin = content,
        Content = content,
      };
      while (entities.Count > 0)
      {
        IParsedEntity item = entities.Dequeue();

        if (item is ElementEntity ee && ee.IsHeader)
        {
          Document.SetHeader(item);
          continue;
        }
        if (item is ElementOpenPlaceholder eop && Current is null)
        {
          Current = new()
          {
            Name = eop.Name,
            Origin = eop.Origin,
            Namespace = eop.Namespace,
            Parent = Document,
            Attributes = eop.Attributes,
          };
          Document.SetRoot(Current);
          inside.Add(Current);
          continue;
        }
        if (item is WhitespaceEntity && Current is null)
        {
          continue;
        }
        if (item is ContentEntity && Current is null)
        {
          throw new InvalidDataException("Cannot have loose content outside the root element.");
        }
        if (item is ElementOpenPlaceholder inner_eop && Current is not null)
        {
          ElementEntity inner = new()
          {
            Name = inner_eop.Name,
            Origin = inner_eop.Origin,
            Namespace = inner_eop.Namespace,
            Parent = Current,
            Attributes = inner_eop.Attributes,
          };
          Current.AddChild(inner);
          Current = inner;
          inside.Add(Current);
          continue;
        }
        if (item is ElementClosePlaceholder inner_ecp && Current is not null)
        {
          if (!Current.Name.Is(inner_ecp.Name))
            throw new InvalidDataException($"Mismatched elements, or you missed a closing tag somewhere. ({Current.Name}) != ({inner_ecp.Name})");

          inside.Drop();
          Current = inside.Peek();
          continue;
        }
        if (item is ContentEntity inner_ce && Current is not null)
        {
          inner_ce.SetParent(Current);
          Current.AddChild(inner_ce);
          continue;
        }
        if (item is ElementEntity inner_ee && Current is not null)
        {
          inner_ee.SetParent(Current);
          Current.AddChild(inner_ee);
          continue;
        }
        if (item is NumberEntity or StringEntity or NullEntity or AttributeEntity)
        {
          throw new InvalidDataException($"Cannot have an entity of this type ({item.TypeName}) in an XML factory.");
        }

        throw new InvalidOperationException($"Item was not handled. ({item.Type}, {item.Origin}) ");

      }
      return Document;
    }
    if (type == BT.Object)
    {
      //TODO: JSON Processing
    }

    throw new InvalidOperationException($"Invalid BasicType ({type}) sent to EntityFactory.");
  }
}
