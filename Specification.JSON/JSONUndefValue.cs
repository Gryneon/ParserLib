using System.Text.Json;

namespace Specification.JSON;

/// <summary>A JSON undefined value.</summary>
public class JSONUndefValue () : IJSONNode
{
  public JsonValueKind Type => JsonValueKind.Undefined;
  public override string ToString () => "undefined";
  public object? Value => null;
}
