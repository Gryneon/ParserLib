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
  #region INI / REG
  /// <summary>A named section.</summary>
  Section,
  /// <summary>A Key\Value pair.</summary>
  Property,
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
  static bool Equals(IParsedEntity? obj_a, IParsedEntity? obj_b) =>
    (obj_a is null && obj_b is null) || (obj_a is not null && obj_b is not null && obj_a.Equals(obj_b));
  bool Equals(IParsedEntity? other);
  bool Equals(object? obj);
  int GetHashCode();
  string ToString();
  /// <summary>Sets the parent property after the type has been contructed.</summary>
  /// <param name="parent">The parent or encompassing object.</param>
  void SetParent(IParsedEntity parent);
}

public interface IPrimitiveEntity : IParsedEntity
{
  string Content { get; }
}

public abstract class ParsedEntity : IParsedEntity, IEquatable<IParsedEntity>
{
  private IParsedEntity? _parent;

  public virtual string? Origin { get; init; }
  public abstract BT Type { get; }
  public IParsedEntity? Parent { get => _parent; init => _parent = value; }
  public abstract bool Equals(IParsedEntity? other);
  public abstract override string ToString();
  public void SetParent(IParsedEntity? parent) => _parent = parent;
}

public class StringEntity : ParsedEntity, IPrimitiveEntity
{
  public string Content => $"\"{Value}\"";
  public required string Value { get; init; }
  public override BT Type => BT.String;

  public override bool Equals(IParsedEntity? other) =>
    other is StringEntity entity && Value.Equals(entity.Value, SCO);
  public override string ToString() => $"\"{Value}\"";
}
/// <summary>An entity representing an xml document.</summary>
public class XMLDocumentEntity : ParsedEntity
{
  /// <summary>The entire text contents of the file.</summary>
  public required string Content { get; init; }
  public IParsedEntity? Header { get; protected set; }
  public IParsedEntity? RootNode { get; protected set; }
  public override BT Type => BT.Document;

  public override bool Equals(IParsedEntity? other) =>
    other is XMLDocumentEntity de && Content.Equals(de.Content, SCO);
  public override string ToString() => Content;
  public void SetHeader(IParsedEntity? header) => Header = header;
  public void SetRoot(IParsedEntity? root) => RootNode = root;
}
/// <summary>An entity representing a json document.</summary>
public class JSONDocumentEntity : ParsedEntity
{
  /// <summary>The entire text contents of the file.</summary>
  public required string Content { get; init; }
  public IParsedEntity? RootObject { get; protected set; }
  public override BT Type => BT.Document;

  public override bool Equals(IParsedEntity? other) =>
    other is JSONDocumentEntity je && Content.Equals(je.Content, SCO);
  public override string ToString() => Content;
  public void SetRoot(IParsedEntity root)
  {
    RootObject = root;
    root.SetParent(this);
  }
}
/// <summary>An entity representing a number or decimal.</summary>
public class NumberEntity : ParsedEntity, IPrimitiveEntity
{
  public required decimal Value { get; init; }
  public override BT Type => BT.Number;

  public string Content => $"{Value}";

  public override bool Equals(IParsedEntity? other) => other is NumberEntity ne && ne.Value == Value;
  public override string ToString() => $"{Value}";
}
/// <summary>An entity representing a null value.</summary>
public class NullEntity : ParsedEntity, IPrimitiveEntity
{
  private const string NullString = "null";

  public override BT Type => BT.Null;

  public string Content => NullString;
  public override bool Equals(IParsedEntity? other) => other is NullEntity;
  public override string ToString() => NullString;
}

public class BooleanEntity : ParsedEntity, IPrimitiveEntity
{
  public required bool Value { get; init; }
  public override BT Type => BT.Boolean;
  public string Content => Value ? "true" : "false";
  public override bool Equals(IParsedEntity? other) => other is BooleanEntity be && be.Value == Value;
  public override string ToString() => Content;
}
public class AttributeEntity : ParsedEntity
{
  public override BT Type => BT.Attribute;
  public string? Namespace { get; init; }
  public required string Key { get; init; }
  public required string Value { get; init; }

  public override bool Equals(IParsedEntity? other) =>
    other is AttributeEntity ae &&
    Key.Equals(ae.Key, SCO) &&
    Value.Equals(ae.Value, SCO) &&
    ((Namespace.IsEmpty && ae.Namespace.IsEmpty) || (Namespace?.Equals(ae.Namespace, SCO) == true));
  public override string ToString() => $"{Key}=\"{Value}\"";
}
public class PropertyEntity : ParsedEntity
{
  public override BT Type => BT.Property;
  public required string Key { get; init; }
  public required IParsedEntity Value { get; init; }

  public override bool Equals(IParsedEntity? other) =>
    other is PropertyEntity pe &&
    Key.Equals(pe.Key, SCO) &&
    Value.Equals(pe.Value);
  public override string ToString() => $"\"{Key}\":{Value}";
}
public class ElementEntity : ElementOpenPlaceholder
{
  private readonly Collection<IParsedEntity> _children = [];
  public ReadOnlyCollection<IParsedEntity> Attributes
  {
    get => [.. _attributes];
    init => _attributes.AddRange(value);
  }
  public ReadOnlyCollection<IParsedEntity> Children
  {
    get => [.. _children];
    init => AddChildren(value);
  }
  public bool IsHeader { get; init; }
  public override BT Type => BT.Element;

  public override string ToString()
  {
    string attrs = Attributes.Select(child => child.ToString()).TextJoin(" ");
    string children = Children.Select(child => child.ToString()).TextJoin(Chars.LFs);

    if (IsHeader)
      return $"<?xml {attrs}?>";

    string elem = $"<{Name} {attrs}";

    return Children.Count == 0
    ? elem + " />"
    : elem + ">" + children + $"</{Name}>";
  }
  public void AddChild(IParsedEntity child)
  {
    child.SetParent(this);
    _children.Add(child);
  }
  public void AddChildren(IEnumerable<IParsedEntity> children) => children.Foreach(AddChild);
  public override bool Equals(IParsedEntity? other) =>
    other is ElementEntity ee &&
    Attributes.SequenceEqual(ee.Attributes) &&
    Children.SequenceEqual(ee.Children) &&
    Name.Equals(ee.Name, SCO);
}
public class ElementClosePlaceholder : ParsedEntity
{
  public required string Name { get; init; }
  public string? Namespace { get; init; }
  public override BT Type => BT.Placeholder;

  public override bool Equals(IParsedEntity? other) =>
    other is ElementClosePlaceholder ecp &&
    Name.Is(ecp.Name) &&
    ((Namespace is null && ecp.Namespace is null) || Namespace.Is(ecp.Namespace));
  public override string ToString() => $"</{Name}>";
}
public class ElementOpenPlaceholder : ParsedEntity
{
  private readonly Collection<IParsedEntity> _attributes = [];

  public required string Name { get; init; }
  public ReadOnlyCollection<IParsedEntity> Attributes
  {
    get => [.. _attributes];
    init => AddAttributes(value);
  }
  public string? Namespace { get; init; }
  public override BT Type => BT.Placeholder;
  public void AddAttribute(IParsedEntity attribute)
  {
    attribute.SetParent(this);
    _attributes.Add(attribute);
  }
  public void AddAttributes(IEnumerable<IParsedEntity> attributes) => attributes.Foreach(AddAttribute);
  public override bool Equals(IParsedEntity? other) =>
    other is ElementOpenPlaceholder eop &&
    Name.Is(eop.Name) &&
    Attributes.SequenceEqual(eop.Attributes) &&
    ((Namespace is null && eop.Namespace is null) || Namespace.Is(eop.Namespace));
  public override string ToString() => $"</{Name}>";

}
public class ContentEntity : ParsedEntity
{
  public required string Content { get; init; }
  public override BT Type => BT.LooseContent;

  public override bool Equals(IParsedEntity? other) =>
    other is ContentEntity ce && Content.Is(ce.Content);
  public override string ToString() => Content;
}
public class ObjectEntity : ParsedEntity
{
  private readonly Collection<IParsedEntity> _properties = [];
  public Collection<IParsedEntity> Properties
  {
    get => [.. _properties];
    init => AddProperties(value);
  }
  public override BT Type => BT.Object;

  public void AddProperty(IParsedEntity property)
  {
    property.SetParent(this);
    _properties.Add(property);
  }

  public void AddProperties(IEnumerable<IParsedEntity> properties) => properties.Foreach(AddProperty);
  public override bool Equals(IParsedEntity? other) =>
    other is ObjectEntity oe && Properties.SequenceEqual(oe.Properties);
  public override string ToString() => Properties.TextJoin(",");
}
public class ArrayEntity : ParsedEntity
{
  public void AddValue(IParsedEntity child)
  {
    child.SetParent(this);
    _values.Add(child);
  }

  public void AddValues(IEnumerable<IParsedEntity> children) => children.Foreach(AddValue);
  private readonly Collection<IParsedEntity> _values = [];
  public Collection<IParsedEntity> Values
  {
    get => [.. _values];
    init => AddValues(value);
  }
  public override BT Type => BT.Array;

  public override bool Equals(IParsedEntity? other) =>
    other is ArrayEntity ce && Values.SequenceEqual(ce.Values);
  public override string ToString() => Values.TextJoin(",");
}
public class SymbolEntity : ParsedEntity
{
  public required string Content { get; init; }
  public override BT Type => BT.Placeholder;

  public static implicit operator string(SymbolEntity ce) => ce.Content;
  public static implicit operator SymbolEntity(string s) => new()
  {
    Content = s,
    Origin = s
  };
  public static bool operator ==(SymbolEntity left, string right)
  {
    return left.Content.Is(right);
  }
  public static bool operator !=(SymbolEntity left, string right)
  {
    return !(left == right);
  }

  public override bool Equals(IParsedEntity? other) =>
    other is SymbolEntity ce && Content.Is(ce.Content);
  public override string ToString() => Content;

  public override int GetHashCode() => Content.GetHashCode(SCO);
  public override bool Equals(object? obj) => obj switch
  {
    string s => this == s,
    IParsedEntity ipe => Equals(ipe),
    _ => false
  };
}
public class WhitespaceEntity : ParsedEntity
{
  public required string Content { get; init; }
  public override BT Type => BT.IgnoredWhitespace;

  public override bool Equals(IParsedEntity? other) =>
    other is ContentEntity ce && Content.Is(ce.Content);
  public override string ToString() => Content;
}
public partial class EntityFactory(string origin = EmptyString)
{
  protected ElementEntity? Current { get; set; }
  public string? Origin { get; set; } = origin;

  /// <summary>JSON Tokenizing Regex</summary>
  [GeneratedRegex(JSONRegex, ROIPW | ROML | ROEC, 3000)]
  [AllowNull]
  protected static partial Regex JSON_PreCompiled { get; }
  [SS("regex")]
  protected const string JSONRegex =
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
  [GeneratedRegex(XMLRegex, ROIPW | ROML | ROEC, 3000)]
  [AllowNull]
  protected static partial Regex XML_PreCompiled { get; }
  [SS("regex")]
  protected const string XMLRegex =
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
    (?'ws'(?<=\>)\s+) |
    (?'ws'(?<=[^\s>])\s+) |
    (?# Leading or Trailing Whitespace)
    (?'content'(?<=\>\s*)[^<]+?(?=\s*<)) |
    (?# XML Comment)
    (?'comment'<!-- ([^-]| -[^-]) -->)
    """;

  private static Collection<IParsedEntity> ParseAttributes(Match match)
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
  private static Collection<IParsedEntity> ParseAttributes(XElement element, ElementEntity parent)
  {
    Collection<IParsedEntity> result = [];
    foreach (XAttribute attr in element.Attributes())
    {
      result.Add(new AttributeEntity()
      {
        Key = attr.Name.LocalName,
        Value = attr.Value,
        Origin = attr.ToString(),
        Parent = parent,
        Namespace = attr.Name.NamespaceName.IsEmpty ? null : attr.Name.NamespaceName
      });
    }
    return result;
  }
  private static ElementEntity GetHeader(Match match) => new()
  {
    IsHeader = true,
    Name = "xml",
    Origin = match.Value,
    Attributes = [.. ParseAttributes(match)],
  };
  private static ContentEntity GetContent(Match match) => new()
  {
    Content = match.Value,
    Origin = match.Value
  };
  private static WhitespaceEntity GetWhitespace(Match match) => new()
  {
    Content = match.Value,
    Origin = match.Value
  };
  private static ElementClosePlaceholder GetClose(Match match) => new()
  {
    Name = match.Groups["name"].Value,
    Namespace = match.HasValidGroup("ns") ? match.Groups["ns"].Value : null,
    Origin = match.Value,
  };
  private static ElementOpenPlaceholder GetOpen(Match match) => new()
  {
    Name = match.Groups["name"].Value,
    Namespace = match.HasValidGroup("ns") ? match.Groups["ns"].Value : null,
    Origin = match.Value,
    Attributes = [.. ParseAttributes(match)]
  };
  private static ElementEntity GetElement(Match match) => new()
  {
    Name = match.Groups["name"].Value,
    Namespace = match.HasValidGroup("ns") ? match.Groups["ns"].Value : null,
    Origin = match.Value,
    Attributes = [.. ParseAttributes(match)]
  };
  private static SymbolEntity GetSymbol(Match match) => new()
  {
    Content = match.Value,
    Origin = match.Value
  };
  private static IParsedEntity ElementSelector(Match match)
  {
    if (match.HasValidGroup("header"))
      return GetHeader(match);

    if (match.HasValidGroup("close"))
      return GetClose(match);

    if (match.HasValidGroup("single"))
      return GetElement(match);

    return GetOpen(match);
  }
  public static IParsedEntity CheckMatch(Match match)
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

  public IParsedEntity FromXElement(XElement root)
  {
    XMLDocumentEntity document = new()
    {
      Origin = root.Value,
      Content = root.Value,
    };

    Current = new()
    {
      Name = root.Name.LocalName,
      Origin = root.Value,
      Parent = (IParsedEntity?)Current ?? document,
      Namespace = root.Name.NamespaceName.IsEmpty ? null : root.Name.NamespaceName,
    };
    document.SetRoot(Current);
    Current.AddAttributes(ParseAttributes(root, Current));
    Current.AddChildren([.. root.Elements().Select(FromXElement)]);

    return document;
  }
  public IParsedEntity FromString(BT type) =>
    Origin.IsEmpty
    ? throw new InvalidOperationException("Origin is empty, cannot parse.")
    : FromString(Origin, type);
  public static IParsedEntity FromString(string content, BT type)
  {
    IParsedEntity? document;
    IParsedEntity? parent = null;
    Collection<IParsedEntity> inside = [];
    Collection<string?> keys = [];
    MatchCollection matches;

    int get_depth() => inside.Count - 1;

    IParsedEntity obj_create(IParsedEntity? inside_entity, IParsedEntity child_obj)
    {
      if (inside_entity is null)
        (document as JSONDocumentEntity)?.SetRoot(child_obj);
      else if (inside_entity is ObjectEntity oe)
        oe.AddProperty(child_obj);
      else if (inside_entity is ArrayEntity ae)
        ae.AddValue(child_obj);
      else
        throw new InvalidOperationException($"Cannot create an object inside a {inside_entity.Type}.");

      inside.Add(child_obj);
      keys.Add(null);
      return child_obj;
    }
    IParsedEntity? obj_exit()
    {
      return keys[get_depth()] is not null
        ? throw new InvalidOperationException($"Key {keys[get_depth()]} ws not popped.")
        : inside.Pop();
    }
    void obj_set_key(string key)
    {
      if (keys[get_depth()] is null)
      {
        keys[get_depth()] = key;
      }
      else
      {
        throw new InvalidOperationException($"Key is already set for this object. ({keys[get_depth()]})");
      }
    }
    string obj_pop_key()
    {
      if (keys[get_depth()] is null)
      {
        throw new InvalidOperationException($"Key is not set for this object at depth {get_depth()}.");
      }
      else
      {
        string result = keys[get_depth()]!;
        keys[get_depth()] = null;
        return result;
      }
    }
    bool obj_chk_key() => keys[get_depth()] is not null;

    switch (type)
    {
      case BT.Null:
        return new NullEntity() { Origin = content };
      #region BT.Element
      case BT.Element:
        matches = XML_PreCompiled.Matches(content);

        document = new XMLDocumentEntity()
        {
          Origin = content,
          Content = content,
        };

        foreach (Match match in matches)
        {
          IParsedEntity item = CheckMatch(match);

          switch (item)
          {
            case ElementEntity ee when ee.IsHeader:
              ((XMLDocumentEntity)document).SetHeader(item);
              continue;
            case ElementOpenPlaceholder eop when parent is null:
              parent = new ElementEntity()
              {
                Name = eop.Name,
                Origin = eop.Origin,
                Namespace = eop.Namespace,
                Parent = document,
                Attributes = eop.Attributes,
              };
              ((XMLDocumentEntity)document).SetRoot(parent);
              inside.Add(parent);
              continue;
            case WhitespaceEntity when parent is null:
              continue;
            case ContentEntity when parent is null:
              throw new InvalidDataException("Cannot have loose content outside the root element.");
            case ElementOpenPlaceholder inner_eop when parent is not null:
              ElementEntity inner = new()
              {
                Name = inner_eop.Name,
                Origin = inner_eop.Origin,
                Namespace = inner_eop.Namespace,
                Parent = parent,
                Attributes = inner_eop.Attributes,
              };
              ((ElementEntity)parent).AddChild(inner);
              inside.Add(inner);
              parent = inner;
              continue;
            case ElementClosePlaceholder inner_ecp when parent is ElementEntity ee:
              if (!ee.Name.Is(inner_ecp.Name))
                throw new InvalidDataException($"Mismatched elements, or you missed a closing tag somewhere. ({ee.Name}) != ({inner_ecp.Name})");
              inside.Drop();
              parent = inside.Peek();
              continue;
            case ContentEntity inner_ce when parent is not null:
              inner_ce.SetParent(parent);
              (parent as ElementEntity)?.AddChild(inner_ce);
              continue;
            case ElementEntity inner_ee when parent is not null:
              inner_ee.SetParent(parent);
              (parent as ElementEntity)?.AddChild(inner_ee);
              continue;
            case NumberEntity or StringEntity or NullEntity or AttributeEntity:
              throw new InvalidDataException($"Cannot have an entity of this type ({item.TypeName}) in an XML factory.");
            default:
              throw new InvalidOperationException($"Item was not handled. ({item.Type}, {item.Origin}) ");
          }
        }
        return document;
      #endregion BT.Element
      case BT.Object:
        matches = JSON_PreCompiled.Matches(content);

        document = new JSONDocumentEntity()
        {
          Origin = content,
          Content = content,
        };

        foreach (Match match in matches)
        {
          IParsedEntity item = CheckMatch(match);

          switch (item)
          {
            case SymbolEntity se when se == "{":
              parent = obj_create(parent, new ObjectEntity());
              continue;
            case SymbolEntity se when se == "}":
              parent = obj_exit();
              continue;
            case SymbolEntity se when se == "[":
              parent = obj_create(parent, new ArrayEntity());
              continue;
            case SymbolEntity se when se == "]":
              parent = obj_exit();
              continue;
            case SymbolEntity se when se.Content is "," or ":":
              continue;
            // Element Entities are not allowed in JSON
            case ElementEntity or ContentEntity or AttributeEntity:
              throw new InvalidDataException($"Cannot have an entity of this type ({item.TypeName}) in a JSON factory.");
            // The keyname is empty, and we have a string entity, so this is the key for the next property.
            case StringEntity se when parent is ObjectEntity oe && !obj_chk_key():
              obj_set_key(se.Value);
              continue;
            // The keyname is not empty, and we have a primitive entity, so this is the value for the current property.
            case IPrimitiveEntity ipe when parent is ObjectEntity oe && obj_chk_key():
              string keyname = obj_pop_key();
              PropertyEntity prop = new()
              {
                Key = keyname,
                Origin = $"\"{keyname}\":{ipe.Origin}",
                Value = ipe,
              };
              oe.AddProperty(prop);
              continue;
          }
        }
        break;
      default:
        throw new InvalidOperationException($"Invalid BasicType ({type}) sent to EntityFactory.");

    }
    throw new InvalidOperationException($"Invalid BasicType ({type}) sent to EntityFactory.");
  }
}
