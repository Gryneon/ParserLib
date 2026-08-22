#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Xml.Linq;

using Catharsis.Commons;

using static Common.EntityFactory.LocVal;

using BT = Common.BasicType;

namespace Common;

/// <summary>The type of object.</summary>
public enum BasicType
{
  /// <summary>The value 'null'.</summary>
  /// <remarks>JSON <see langword="null"/> value.</remarks>
  Null,

  /// <summary>Quoted text.</summary>
  /// <remarks>JSON values, JSON keys, XML Attribute Values, INI </remarks>
  String,

  /// <summary>Non-quoted numeric data.</summary>
  Number,

  /// <summary>An array of <see cref="BasicParsedEntity"/> items.</summary>
  Array,

  /// <summary>A basic dictionary.</summary>
  Object,

  /// <summary>A <see langword="true"/> or a <see langword="false"/> stored as 'true' and 'false'.</summary>
  Boolean,

  /// <summary>Invalid text. Unable to parse.</summary>
  Invalid,

  /// <summary>This returns when you try to get a value that doesn't exist.</summary>
  Absent,

  /// <summary>This is the starting and ending whitespace in an element.</summary>
  IgnoredWhitespace,

  /// <summary>This is content within a mixed element.</summary>
  LooseContent,
  /// <summary>A complex object that stores a dictionary of values as well as a contained value.</summary>
  Element
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
}

public abstract class ParsedEntity : IParsedEntity, IEquatable<IParsedEntity>
{
  public virtual required string? Origin { get; init; }
  public abstract BT Type { get; }
  public IParsedEntity? Parent { get; init; }
  public abstract bool Equals (IParsedEntity? other);
  public abstract override string ToString ();
}

public class StringEntity : ParsedEntity
{
  public required string Value { get; init; }
  public override BT Type => BT.String;

  public override bool Equals (IParsedEntity? other) => other is StringEntity entity && Value.Equals(entity.Value, SCO);
  public override string ToString () => $"\"{Value}\"";
}
public class NumberEntity : ParsedEntity
{
  public override BT Type => BT.Number;
  public override string ToString () => $"{Value}";
}
public class NullEntity : ParsedEntity
{
  public override BT Type => BT.Null;
  public override string ToString () => "null";
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
  public void AddAttribute (IParsedEntity attribute)
  {
    _attributes.Add(attribute);
  }
  public void AddChild (IParsedEntity child)
  {
    _children.Add(child);
  }
}

public static class EntityFactory
{
  private static readonly IParsedEntity Current;
  private static int Cursor;
  private static readonly string Construct = SE;
  private static string? Origin;
  private static readonly LocVal Location;
  private static char ThisChar => Origin is null ? '\0' : Origin[Cursor];

  internal enum LocVal
  {
    AtStart,
    Outside,
    InElementOpen,
    InsideAnElement,
    InElementClose,
    AtEnd,
  }

  public static IParsedEntity ProduceAll (string content, BT type)
  {
    if (type == BT.Null)
      return new NullEntity() { Origin = content };
    if (type == BT.Element)
    {
      string xml_regex = """

      (?'element'<
      (?'header'\?)?

      (?'close'\s*\/)?
      \s*
      ((?'ns'\w+):)?
      (?'name'\w+)

      (?:
      \s+     (?'attribute'((?'attrns'\w+):)?(?'attrname'\w+)     \s*       =      \s*    ""(?'attrval'([^\n""\\]|\\[^\n])*)"")
      )*
      (?'single'\s*\/)?
      \s*
      (\k'header')?
      >)|(?'ws'\s+)|(?'content'[^<]+?(?=\s*<))

      """;
      MatchCollection openelems = Regex.Matches(content, xml_regex);
      while (1 < 2)
      {
        if (ThisChar is '<')
        {
          switch (Location)
          {
            case AtStart or Outside:
              // Setup root object
              break;
            case InElementOpen or InElementClose:
              throw new InvalidOperationException("");
            case InsideAnElement:
              // Setup new element
              break;
            case AtEnd:
              throw new InvalidOperationException("");
          }
          Cursor++;

        }
        if (ThisChar is '<')
        {
          IndexOfElementOpen = cursor;
        }
      }
    }
    new IParsedEntity
  }

  public BasicParsedEntity (string origin)
  {
    Origin = origin;

    if (Origin.StartsWithAny(["\"", "\'"], SCO))
    {
      Type = BT.String;
      Value = Origin[1..^1];
    }
    else if (DateTime.TryParse(Origin, CIIC, out DateTime dt))
    {
      Type = BT.String;
      Value = dt.ToString("yyyy-MM-dd HH:mm", CIIC);
    }
    else if (decimal.TryParse(Origin, out decimal d))
    {
      Type = BT.Number;
      Value = d;
    }
    else if (bool.TryParse(Origin, out bool b))
    {
      Type = BT.Number;
      Value = b;
    }
    else if (Origin.Equals("null", SCO))
    {
      Type = BT.Null;
      Value = null;
    }
    else if (Origin.Length == 0)
    {
      Type = BT.Absent;
      Value = SE;
    }
    else
    {
      Type = BT.Invalid;
      Value = default;
    }
  }
  public BasicParsedEntity (object? value)
  {
    switch (value)
    {
      case string s when s.StartsWithAny(["'", "\""]):
        Value = s[1..^1];
        Type = BT.String;
        Origin = s;
        break;
      case decimal d:
        Value = d;
        Type = BT.Number;
        Origin = $"{d}";
        break;
      case bool b:
        Value = b;
        Type = BT.Boolean;
        Origin = $"{b}";
        break;
      case IEnumerable<KeyValuePair<string, object>> kvps:
        Value = ParseObject(kvps);
        Origin = null;
        Type = BT.Object;
        break;
      case IEnumerable<object> list:
        Value = ParseArray(list);
        Origin = null;
        Type = BT.Array;
        break;
    }
  }
  public BasicParsedEntity (BT type, string? origin = null)
  {
    Origin = origin;

    switch (type)
    {
      case BT.Null when origin.Is("null") || origin is null:
        Value = null;
        Type = type;
        break;
      case BT.String when origin?.StartsWithAny(["'", "\""]) == true:
        Value = origin[1..^1];
        Type = type;
        break;
      case BT.Boolean when bool.TryParse(origin, out bool val):
        Value = val;
        Type = type;
        break;
      case BT.Number when decimal.TryParse(origin, out decimal dec):
        Value = dec;
        Type = type;
        break;
      case BT.String or BT.LooseContent when origin?.Length > 0:
        Value = origin;
        Type = BT.LooseContent;
        break;
      case BT.Array or BT.Object or BT.Element:
        throw new InvalidOperationException("Invalid type, assembly of primitives comes first.");
      case BT.Invalid:
        throw new InvalidOperationException("ParseValue reported that the object was Invalid.");
      case BT.Absent:
        Value = SE;
        Type = type;
        break;
      default:
        throw new InvalidOperationException("ParseValue reported an unknown type.");
    }
  }

  private static BasicParsedEntity ParseItem (object obj) =>
    obj is BasicParsedEntity bpe ? bpe : new(obj);
  private static KeyValuePair<string, BasicParsedEntity> ParseItem (KeyValuePair<string, object> kvp) =>
    new(kvp.Key, ParseItem(kvp.Value));
  private static Collection<BasicParsedEntity> ParseArray (IEnumerable<object> objects) =>
    [.. objects.Select(ParseItem)];
  private static Dictionary<string, BasicParsedEntity> ParseObject (IEnumerable<KeyValuePair<string, object>> kvps) =>
    kvps.Select(ParseItem).ToDictionary();

  public bool Equals (IParsedEntity? other)
  {
    if (other is null)
      return false;
    else if (Value is null && other.Value is not null)
      return false;
    else if (Value is not null && other.Value is null)
      return false;
    else if (Value is null && other.Value is null)
      return Type == other.Type;
    else if (Value is IEnumerable<object> idic && other.Value is IEnumerable<object> odic)
      return idic.Order().SequenceEqual(odic.Order());
    else if (Value is not null && other.Value is not null && Value is IConvertible ic && other.Value is IConvertible oc)
      return Type == other.Type && ic.ToDecimal(CIIC) == oc.ToDecimal(CIIC);
    else if (Value is not null && other.Value is not null && Value is string istr && other.Value is string ostr)
      return Type == other.Type && istr.Equals(ostr, SCO);
    else
      return false;
  }
  public override bool Equals (object? obj) => obj switch
  {
    null => false,
    IParsedEntity bpo => Equals(bpo),
    string s => Type is BT.String && (Value as string)!.Equals(s, SCO),
    bool b when Value is IConvertible ic => Type == BT.Boolean && b == ic.ToBoolean(CIIC),
    IConvertible d when Value is IConvertible iconv => Type is BT.Number && d.ToDecimal(CIIC) == iconv.ToDecimal(CIIC),
    _ => false
  };
  public override int GetHashCode () => Value?.GetHashCode() ?? 0;
  public override string ToString () => Type + ": " + Value + $"(From: \"{Origin ?? "<null>"}\")";
}
/*
/// <summary>A basic json style dictionary.</summary>
public class BasicParsedJSONObject : BasicParsedEntity
{
  public override T Type => T.Object;

  private string ToString_Object ()
  {
    if (Value is not IDictionary<string, BasicParsedObject> dic)
      return "<error>";

    string result = "{";
    const string end = "}";
    bool firstProp = true;

    foreach (KeyValuePair<string, BasicParsedObject> property in dic)
    {
      if (!firstProp) result += ",";
      firstProp = false;
      result += property.Key;
      result += ":";
      result += property.Value;
    }

    return result + end;
  }

  private string ToString_Array ()
  {
    string result = "[";
    const string end = "]";
    bool firstProp = true;

    foreach (BasicParsedEntity item in Value.AsCollection<BasicParsedEntity>())
    {
      if (!firstProp) result += ",";
      firstProp = false;
      result += item.ToString();
    }

    return result + end;
  }

  public BasicParsedEntity this[string key]
  {
    get => Value is Dictionary<string, BasicParsedEntity> dic && dic.TryGetValue(key, out BasicParsedEntity? value) ? value : new(T.Absent);
    set
    {
      BasicParsedEntity item = new(value);
      if (Value is IDictionary<string, BasicParsedEntity> dic)
      {
        dic[key] = item;
      }
      else if (Value is IList<BasicParsedEntity> list)
      {
        list[int.Parse(key, CIIC)] = item;
      }
      else
      {
        Debug.Log(MsgClass.Error, $"Cannot write to key '{key}' as the value is not a dictionary.", this);
      }
    }
  }
  /*
  public BasicParsedObject this[int index]
  {
    get
    {
      if (Value is IList<BasicParsedObject> arr && arr.Count < index && index >= 0)
        return arr[index];
      else
        return new(Absent);
    }
    set
    {
      if (Value is IList<BasicParsedObject> col && col.Count < index && index >= 0)
      {
        BasicParsedObject item = new();
        item.StoreValue(value);
        col[index] = item;
      }
      else
      {
        Debug.Log(MsgClass.Error, $"Cannot insert to index {index}.", this);
      }
    }
  }
  
  public override bool Equals (object? obj)
  {
    if (obj is null)
      return false;

    if (obj is BasicParsedEntity bpo)
      return Equals(bpo);

    if (obj is string s)
      return Type is T.String && (Value as string)!.Equals(s, SCO);

    if (obj is decimal d)
      return Type is T.Number && (decimal) Value! == d;

    return false;
  }
}
*/
/// <summary>A basic attribute/element dictionary.</summary>
/// <remarks>This will soon support mixed content.</remarks>
public class BasicParsedElement : BasicParsedEntity, IReadOnlyCollection<BasicParsedElement>
{
  public bool HasAttributes => Attributes.Count != 0;
  public bool HasBody => Elements.Count != 0 || Origin is not null;

  /// <summary>The element name.</summary>
  public string Name { get; }

  /// <summary>The attributes of this element.</summary>
  public Dictionary<string, string> Attributes { get; } = [];

  /// <summary>The child elements of this element.</summary>
  public Collection<BasicParsedElement> Elements => Value as Collection<BasicParsedElement> ?? [];

  /// <summary>The value if it contains a value not elements.</summary>

  public int Count => Elements.Count;

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

  /// <summary>Gets the element at the index.</summary>
  /// <param name="index">The index to get the element from.</param>
  /// <returns>The element at the given index.</returns>
  /// <exception cref="ArgumentOutOfRangeException">The index is negative or is too large.</exception>
  public BasicParsedElement this[int index]
  {
    get => index >= Elements.Count || index < 0 ? throw new ArgumentOutOfRangeException(nameof(index)) : Elements[index];
  }

  public BasicParsedElement this[string ofType, int index]
  {
    get => Elements.Where(e => e.Name.Is(ofType)).At(index);
  }

  public IEnumerable<BasicParsedElement> this[string ofType]
  {
    get => Elements.Where(e => e.Name.Is(ofType));
  }

  /// <summary>Looks up and retrieves the attribute value as a <see langword="string"/>.</summary>
  /// <param name="attribute">The attribute to lookup.</param>
  /// <returns>The attribute value as a <see langword="string"/>, or an empty string if there is no attribute of that name.</returns>
  public string GetAttribute (string attribute) => Attributes.TryGetValue(attribute, out string? value) ? value : SE;

  public IEnumerator<BasicParsedElement> GetEnumerator () => Elements.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
/*
internal class PieceData
{
  public string TokenType { get; set; }
  public string PropertyName { get; set; }
  public Type DataType { get; set; }
}

internal class TokenPiece : IIndexSortable
{
  public int Index { get; set; }
  public string Value { get; }
  public string TokenType { get; }
  public PieceData Data { get; }
}
*/
