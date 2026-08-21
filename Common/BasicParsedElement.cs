#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Xml.Linq;

using T = Common.BasicType;

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
  extension(T type)
  {
    public bool IsPrimitive => type is T.Number or T.String or T.Boolean;
    public bool IsDictionary => type is T.Object or T.Element;
    public bool IsCollection => type is T.Array or T.Element or T.Object;
  }
}

public interface IParsedEntity
{
  string? Origin { get; set; }
  T Type { get; }
  object? Value { get; set; }

  bool Equals (IParsedEntity? other);
  bool Equals (object? obj);
  int GetHashCode ();
  string ToString ();
}

public class BasicParsedEntity : IParsedEntity
{
  public string? Origin { get; set; }
  public object? Value { get; set; }
  public virtual T Type { get; set; }
  public bool IsError => Type is T.Invalid;
  public IParsedEntity? Parent { get; set; }

  public BasicParsedEntity () { }
  public BasicParsedEntity (string origin)
  {
    Origin = origin;

    if (Origin.StartsWithAny(["\"", "\'"], SCO))
    {
      Type = T.String;
      Value = Origin[1..^1];
    }
    else if (DateTime.TryParse(Origin, CIIC, out DateTime dt))
    {
      Type = T.String;
      Value = dt.ToString("yyyy-MM-dd HH:mm", CIIC);
    }
    else if (decimal.TryParse(Origin, out decimal d))
    {
      Type = T.Number;
      Value = d;
    }
    else if (bool.TryParse(Origin, out bool b))
    {
      Type = T.Number;
      Value = b;
    }
    else if (Origin.Equals("null", SCO))
    {
      Type = T.Null;
      Value = null;
    }
    else if (Origin.Length == 0)
    {
      Type = T.Absent;
      Value = SE;
    }
    else
    {
      Type = T.Invalid;
      Value = default;
    }
  }
  public BasicParsedEntity (object? value)
  {
    switch (value)
    {
      case string s when s.StartsWithAny(["'", "\""]):
        Value = s[1..^1];
        Type = T.String;
        Origin = s;
        break;
      case decimal d:
        Value = d;
        Type = T.Number;
        Origin = $"{d}";
        break;
      case bool b:
        Value = b;
        Type = T.Boolean;
        Origin = $"{b}";
        break;
      case IEnumerable<KeyValuePair<string, object>> kvps:
        Value = ParseObject(kvps);
        Origin = null;
        Type = T.Object;
        break;
      case IEnumerable<object> list:
        Value = ParseArray(list);
        Origin = null;
        Type = T.Array;
        break;
    }
  }
  public BasicParsedEntity (T type, string? origin = null)
  {
    Origin = origin;

    switch (type)
    {
      case T.Null when origin.Is("null") || origin is null:
        Value = null;
        Type = type;
        break;
      case T.String when origin?.StartsWithAny(["'", "\""]) == true:
        Value = origin[1..^1];
        Type = type;
        break;
      case T.Boolean when bool.TryParse(origin, out bool val):
        Value = val;
        Type = type;
        break;
      case T.Number when decimal.TryParse(origin, out decimal dec):
        Value = dec;
        Type = type;
        break;
      case T.String or T.LooseContent when origin?.Length > 0:
        Value = origin;
        Type = T.LooseContent;
        break;
      case T.Array or T.Object or T.Element:
        throw new InvalidOperationException("Invalid type, assembly of primitives comes first.");
      case T.Invalid:
        throw new InvalidOperationException("ParseValue reported that the object was Invalid.");
      case T.Absent:
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
    string s => Type is T.String && (Value as string)!.Equals(s, SCO),
    bool b when Value is IConvertible ic => Type == T.Boolean && b == ic.ToBoolean(CIIC),
    IConvertible d when Value is IConvertible iconv => Type is T.Number && d.ToDecimal(CIIC) == iconv.ToDecimal(CIIC),
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
