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
  Absent
}

/// <summary>A basic json style dictionary.</summary>
public class BasicParsedObject : IEquatable<BasicParsedObject>
{
  public BasicType Type { get; private set; } = BasicType.Null;
  public object? Value { get; set; }

  public static BasicParsedObject Invalid (string value) => new()
  {
    Type = BasicType.Invalid,
    Value = value
  };
  public static BasicParsedObject Absent => new()
  {
    Type = BasicType.Absent,
    Value = SE
  };
  private static (BasicType Type, object? Value) ParseString (string value)
  {
    if (value.StartsWithAny(["\"", "\'"], SCO))
    {
      return (BasicType.String, value[1..^1]);
    }
    else if (decimal.TryParse(value, out decimal d))
    {
      return (BasicType.Number, d);
    }

    return value switch
    {
      "true" => (BasicType.Boolean, true),
      "false" => (BasicType.Boolean, false),
      _ => (BasicType.Null, null),
    };
  }
  public void StoreValue (object? value)
  {
    Value = value switch
    {
      null => null,
      decimal => value,
      int => value,
      string s => ParseString(s).Value,
      bool => value,
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
  private static Dictionary<string, BasicParsedObject> ParseObject (IEnumerable<KeyValuePair<string, object>> objects)
  {
    Dictionary<string, BasicParsedObject> result = [];
    foreach (KeyValuePair<string, object> obj in objects)
    {
      string key = obj.Key;
      object value = obj.Value;
      if (value is BasicParsedObject bpo)
      {
        result.Add(key, bpo);
      }
      else
      {
        BasicParsedObject item = new();
        item.StoreValue(value);
        result.Add(key, item);
      }
    }
    return result;
  }
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
    (BasicType Type, object? Value) check = ParseString(initial);
    Type = check.Type;
    Value = check.Value;
  }

  public BasicParsedObject this[string key]
  {
    get => Value is Dictionary<string, BasicParsedObject> dic && dic.TryGetValue(key, out BasicParsedObject? value) ? value : Absent;
    set
    {
      if (Value is Dictionary<string, BasicParsedObject> dic)
      {
        BasicParsedObject item = new();
        item.StoreValue(value);
        dic[key] = item;
      }
      else
      {
        Debug.Log(MsgClass.Error, $"Cannot write to key '{key}' as there is not a dictionary.", this);
      }
    }
  }

  public BasicParsedObject this[int index]
  {
    get
    {
      if (Value is IEnumerable<BasicParsedObject> arr && arr.ICount < index && index >= 0)
        return arr.At(index);
      else
        return Absent;
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
    if (obj == null)
      return false;

    if (obj is BasicParsedObject bpo)
      return Equals(bpo);

    if (obj is string s)
      return Type is BasicType.String && (Value as string)!.Equals(s, SCO);

    if (obj is decimal d)
      return Type is BasicType.Number && (decimal) Value! == d;

    return false;
  }
  public bool Equals (BasicParsedObject? other) =>
    other != null && ((Value is null && other.Value is null) || (Value is not null && other.Value is not null && other.Type == Type && Type switch
    {
      BasicType.Null => true,
      BasicType.String => (Value as string)!.Equals(other.Value as string, SCO),
      BasicType.Number => (decimal) Value == (decimal) other.Value,
      BasicType.Array => Value.AsCollection().SequenceEqual(other.Value.AsCollection()),
      BasicType.Object => Value.AsCollection().SequenceEqual(other.Value.AsCollection()),
      BasicType.Boolean => (bool) Value == (bool) other.Value,
      BasicType.Invalid => false,
      BasicType.Absent => true,
      _ => false
    }));
  public override int GetHashCode () => Value?.GetHashCode() ?? 0;
}

/// <summary>A basic attribute/element dictionary.</summary>
/// <remarks>This does not support mixed content.</remarks>
public class BasicParsedElement : IReadOnlyCollection<BasicParsedElement>
{
  /// <summary>The element name.</summary>
  public string Name { get; }
  /// <summary>The attributes of this element.</summary>
  public Dictionary<string, string> Attributes { get; } = [];
  /// <summary>The child elements of this element.</summary>
  public Collection<BasicParsedElement> Elements { get; } = [];
  /// <summary>The value if it contains a value not elements.</summary>
  public string? Value { get; }

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
