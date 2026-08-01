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
  /// <summary>An array of <see cref="IBasicObject"/> items.</summary>
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
/// <summary>Base interface for complex objects.</summary>
public interface IBasicObject : IEquatable<IBasicObject>, IEquatable<string>, IEquatable<bool>, IEquatable<decimal>
{
  string Value { get; }
  BasicType Type { get; }
}
/// <summary>A primitive object.</summary>
public sealed class BasicParsedPrimitive : IBasicObject
{
  public required string Value { get; init; }
  public required BasicType Type { get; init; }
  public decimal NumericValue { get; init; }
  public bool BooleanValue { get; init; }
  private static BasicParsedPrimitive Null => new() { Type = BasicType.Null, Value = "null" };
  internal static BasicParsedPrimitive Invalid (string value) => new()
  {
    Type = BasicType.Invalid,
    Value = value
  };
  internal static BasicParsedPrimitive Absent => new()
  {
    Type = BasicType.Absent,
    Value = SE
  };
  private static BasicParsedPrimitive True => new()
  {
    Type = BasicType.Boolean,
    Value = "true",
    BooleanValue = true,
    NumericValue = 1
  };
  private static BasicParsedPrimitive False => new()
  {
    Type = BasicType.Boolean,
    Value = "false",
  };
  private static BasicParsedPrimitive String (string value) => new()
  {
    Type = BasicType.String,
    Value = value[1..^1]
  };
  private static BasicParsedPrimitive Number (string value) => new()
  {
    Type = BasicType.Number,
    Value = value,
    NumericValue = decimal.Parse(value, CIIC),
    BooleanValue = decimal.Parse(value, CIIC) >= 1
  };
  public bool Equals (IBasicObject? other) =>
    ((other is null || other.Value.Length == 0) && Type is BasicType.Absent) ||
    (Value.Equals(other?.Value, SCO) && Type == other.Type);
  public bool Equals (string? other) => Value.Equals(other, SCO) && Type is BasicType.String;
  public bool Equals (bool other) => BooleanValue == other && Type is BasicType.Boolean or BasicType.Number;
  public bool Equals (decimal other) => NumericValue == other && Type is BasicType.Boolean or BasicType.Number;

  public static implicit operator BasicParsedPrimitive (string value)
  {
    return value switch
    {
      "null" => Null,
      "true" => True,
      "false" => False,
      string when value.Length >= 2 && value.StartsWith('"', SCO) && value.EndsWith('"', SCO) => String(value),
      string when value.IsNumber => Number(value),
      string when value.Length > 0 => Invalid(value),
      _ => Absent
    };
  }
}

/// <summary>A basic json style dictionary.</summary>
public class BasicParsedObject : IBasicObject
{
  public Dictionary<string, IBasicObject> Properties { get; } = [];

  public string Value => GetJSONString();

  private string GetJSONString ()
  {
    string result = "{";
    const string end = "}";
    bool firstProp = true;

    foreach (var property in Properties)
    {
      if (!firstProp) result += ",";
      firstProp = false;
      result += property.Key;
      result += ":";
      result += property.Value;
    }

    return result + end;
  }

  public BasicType Type { get; } = BasicType.Object;

  public BasicParsedObject () { }
  public BasicParsedObject (System.Text.Json.Nodes.JsonObject value)
  {
    foreach (var property in value)
    {
      // TODO: Finish this
    }
  }

  public IBasicObject this[string key] =>
    Properties.TryGetValue(key, out IBasicObject? value)
    ? value
    : BasicParsedPrimitive.Absent;

  public bool Equals (IBasicObject? other) => other is not null && Value.Equals(other.Value, SCO) && other.Type == Type;
  public bool Equals (string? other) => false;
  public bool Equals (bool other) => false;
  public bool Equals (decimal other) => false;
}

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
