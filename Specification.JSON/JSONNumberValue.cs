using System.Text.Json;

namespace Specification.JSON;

/// <summary>A JSON numeric value.</summary>
/// <param name="value">The numeric value to assign.</param>
public class JSONNumberValue (decimal value) : IJSONNode
{
  public decimal Value { get; set; } = value;
  public JsonValueKind Type => JsonValueKind.Number;
  public override string ToString () => $"{Value}";
  object? IJSONNode.Value => Value;
}
