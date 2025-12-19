using System.Text.Json;

namespace Specification.JSON;

/// <summary>A JSON boolean value.</summary>
/// <param name="value">The boolean value to assign.</param>
public class JSONBoolValue (bool value) : IJSONNode
{
  public bool Value { get; set; } = value;
  public JsonValueKind Type => Value ? JsonValueKind.True : JsonValueKind.False;
  public override string ToString () => Value ? "true" : "false";
  object? IJSONNode.Value => Value;
}
