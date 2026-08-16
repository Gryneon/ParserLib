#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Xml.Linq;

namespace Common;

/// <summary>The type of object.</summary>
public enum BasicType
{
  /// <summary>The value 'null'.</summary>
  Null,
  /// <summary>Quoted text.</summary>
  String,
  /// <summary>Non-quoted numeric data.</summary>
  Number,
  /// <summary>An array of <see cref="BasicParsedObject"/> items.</summary>
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
  LooseContent
}

public union ElementValue : IU
{
  public BasicParsedElement Element { get; set; }
}

public static class BasicTypeExt
{
  extension(BasicType type)
  {
    public BasicType InitialType => type is BasicType.Absent or BasicType.Null or BasicType.IgnoredWhitespace or BasicType.Invalid ? BasicType.Null : type;
  }
}

public class BasicParsedEntity : IEquatable<BasicParsedEntity>
{
  internal string? Origin { get; set; }
  public virtual object? Value { get; set; }
  public BasicType Type { get; internal set; } = BasicType.Null;
  public bool IsError => Type is BasicType.Invalid;

  [MemberNotNull(nameof(Type), nameof(Origin))]
  protected void Parse ()
  {
    Type = Type.InitialType;

    if (Origin.IsEmpty)
    {
      Type = BasicType.Absent;
      Origin = SE;
      Value = default;
      return;
    }

    // Does not have a type but has content.
    if (Value is null && Type is BasicType.Null)
    {
      if (Origin.StartsWithAny(["\"", "\'"], SCO))
      {
        Type = BasicType.String;
        Value = Origin[1..^1];
      }
      else if (DateTime.TryParse(Origin, CIIC, out DateTime dt))
      {
        Type = BasicType.String;
        Value = dt.ToString("yyyy-MM-dd HH:mm", CIIC);
      }
      else if (decimal.TryParse(Origin, out decimal d))
      {
        Type = BasicType.Number;
        Value = d;
      }
      else if (bool.TryParse(Origin, out bool b))
      {
        Type = BasicType.Number;
        Value = b;
      }
      else
      {
        Type = BasicType.Invalid;
        Value = default;
      }
    }

    // Has a type but unparsed.
    else if (Value is null && Type is not BasicType.Null)
    {
      Value = Type switch
      {
        BasicType.String => MakeString(Origin),
        BasicType.Number => MakeDecimal(),
      };
    }
  }
  public bool Equals (BasicParsedEntity? other)
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
    BasicParsedEntity<object> bpo => Equals(bpo),
    string s => Type is BasicType.String && (Value as string)!.Equals(s, SCO),
    bool b when Value is IConvertible ic => Type == BasicType.Boolean && b == ic.ToBoolean(CIIC),
    IConvertible d when Value is IConvertible iconv => Type is BasicType.Number && d.ToDecimal(CIIC) == iconv.ToDecimal(CIIC),
    _ => false
  };
  public override int GetHashCode () => Value?.GetHashCode() ?? 0;
  public override string ToString () => Type + ": " + Value + $"(From: \"{Origin}\")";
}

/// <summary>A basic json style dictionary.</summary>
public class BasicParsedObject : BasicParsedEntity<object>
{

  public void StoreValue (object? value)
  {
    Value = value switch
    {
      null => null,
      decimal => value,
      int => value,
      string s => Parse().Value,
      bool => value,
      KeyValuePair<string, object> kvp => ParseObject([kvp]),
      IEnumerable<KeyValuePair<string, object>> dic => ParseObject(dic),
      IEnumerable<object> arr => ParseArray(arr),
      _ => "<error>"
    };

    Type = Value switch
    {
      "<error>" => BasicType.Invalid,
      null => BasicType.Null,
      decimal => BasicType.Number,
      int => BasicType.Number,
      string => BasicType.String,
      bool => BasicType.Boolean,
      Dictionary<string, BasicParsedObject> => BasicType.Object,
      Collection<BasicParsedObject> => BasicType.Array,
      _ => BasicType.Invalid
    };
  }
  private static Collection<BasicParsedObject> ParseArray (IEnumerable<object> objects)
  {
    Collection<BasicParsedObject> result = [];
    foreach (object obj in objects)
    {
      if (obj is BasicParsedObject bpo)
      {
        result.Add(bpo);
      }
      else
      {
        BasicParsedObject item = new();
        item.StoreValue(obj);
        result.Add(item);
      }
    }
    return result;
  }
  private static BasicParsedObject ParseItem (object obj)
  {
    if (obj is BasicParsedObject bpo)
    {
      return bpo;
    }
    else
    {
      BasicParsedObject item = new();
      item.StoreValue(obj);
      return item;
    }
  }
  private static KeyValuePair<string, BasicParsedObject> ParseItem (KeyValuePair<string, object> obj)
  {
    string key = obj.Key;
    object value = obj.Value;
    return new(key, ParseItem(value));
  }
  private static Dictionary<string, BasicParsedObject> ParseObject (IEnumerable<KeyValuePair<string, object>> objects) =>
    objects.Select(ParseItem).ToDictionary();
  public override string ToString ()
  {
    return Type switch
    {
      BasicType.Null => "null",
      BasicType.String => $"\"{Value}\"",
      BasicType.Number => $"{Value}",
      BasicType.Array => ToString_Array(),
      BasicType.Object => ToString_Object(),
      BasicType.Boolean => $"{Value}",
      BasicType.Invalid => "<error>",
      BasicType.Absent => "null",
      _ => "undefined"
    };
  }
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

    foreach (BasicParsedObject item in Value.AsCollection<BasicParsedObject>())
    {
      if (!firstProp) result += ",";
      firstProp = false;
      result += item.ToString();
    }

    return result + end;
  }
  public BasicParsedObject ()
  {
    Type = BasicType.Null;
  }
  public BasicParsedObject (string initial)
  {
    Origin = initial;
    Parse();
    Type = check.Type;
    Value = check.Value;
  }
  public BasicParsedObject (BasicParsedEntity<object> initial)
  {
    Origin = initial.Origin;
    Type = initial.Type;
    Value = initial.Value;
  }

  public BasicParsedObject this[string key]
  {
    get => Value is Dictionary<string, BasicParsedObject> dic && dic.TryGetValue(key, out BasicParsedObject? value) ? value : new(Absent);
    set
    {
      BasicParsedObject item = new();
      item.StoreValue(value);
      if (Value is IDictionary<string, BasicParsedObject> dic)
      {
        dic[key] = item;
      }
      else if (Value is IList<BasicParsedObject> list)
      {
        list[int.Parse(key, CIIC)] = item;
      }
      else
      {
        Debug.Log(MsgClass.Error, $"Cannot write to key '{key}' as the value is not a dictionary.", this);
      }
    }
  }

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

    if (obj is BasicParsedEntity<object> bpo)
      return Equals(bpo);

    if (obj is string s)
      return Type is BasicType.String && (Value as string)!.Equals(s, SCO);

    if (obj is decimal d)
      return Type is BasicType.Number && (decimal) Value! == d;

    return false;
  }
}

/// <summary>A basic attribute/element dictionary.</summary>
/// <remarks>This will soon support mixed content.</remarks>
public class BasicParsedElement : BasicParsedEntity<Collection<BasicParsedElement>>, IReadOnlyCollection<BasicParsedElement>
{
  public bool HasAttributes => Attributes.Count != 0;
  public bool HasBody => Elements.Count != 0 || Origin is not null;
  /// <summary>The element name.</summary>
  public string Name { get; }
  public override Collection<BasicParsedElement>? Value => Elements;
  /// <summary>The attributes of this element.</summary>
  public Dictionary<string, string> Attributes { get; } = [];
  /// <summary>The child elements of this element.</summary>
  public Collection<BasicParsedElement> Elements { get; } = [];
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
