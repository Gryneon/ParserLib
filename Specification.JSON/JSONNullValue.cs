using System.Text.Json;

namespace Specification.JSON;

/// <summary>A JSON null value.</summary>
public class JSONNullValue () : IJSONNode
{
  public JsonValueKind Type => JsonValueKind.Null;
  public override string ToString () => "null";
  public object? Value => null;
}
