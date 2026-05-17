#pragma warning disable CA1822 // Mark members as static

using System.Xml.Linq;

using Parser.Inference;

namespace Parser;

public class OpHelper
{
  private static readonly XNamespace NS = "Parser/Spec";

  public OpHelper (XElement? element)
  {
    IsNull = element is null;
    Type = element?.Name.LocalName ?? SE;
    TargetVar = element?.Attribute(NS + "target_var")?.Value;
    CursorVar = element?.Attribute(NS + "cursor_var")?.Value;
    LengthVar = element?.Attribute(NS + "length_var")?.Value;

    foreach (XAttribute a in element?.Attributes() ?? [])
    {
      string key = a.Name.LocalName;
      string val = a.Value;

      Attributes.Add(key, val);
    }

    foreach (XElement e in element?.Elements() ?? [])
    {
      Children.Add(new(e));
    }
  }

  public int AInt (string key, int default_value)
  {
    return Attributes.TryGetValue(key, out string? value) ? int.Parse(value) : default_value;
  }

  public IOperation ToOp ()
  {
    if (IsNull)
      return new OperationEnd();

    switch (Type)
    {
      case "GotoLabel":
        return new OperationJump(Attributes["name"]);
      case "Label":
        return new OperationLabel(Attributes["name"]);
      case "MakeCursor":
        CursorData cursor = new(AInt("position", 0), Attributes["cursor_key"]);
        throw null;
      default:
        throw new OperationBadDefinitionException();
    }
  }

  public bool IsNull { get; }
  public string Type { get; }
  public string? TargetVar { get; set; }
  public string? CursorVar { get; set; }
  public string? LengthVar { get; set; }

  public Dictionary<string, string> Attributes { get; } = [];
  public Collection<OpHelper> Children { get; } = [];
}

public static class SpecInstructionParser
{
  private static readonly XNamespace NS = "Parser/Spec";

  public static Spec LoadSpec (string path)
  {
    XDocument doc = XDocument.Load(path);

    XElement specElement = doc.Root?.Element(NS + "Spec") ?? throw new SpecNotDefinedException("Invalid XML - No Spec in file.");

    string name = (string?) specElement.Element(NS + "Name") ?? throw new SpecNotDefinedException("Invalid XML - No Name in Spec.");

    // Parse instructions
    IEnumerable<XElement>? instructionElements = specElement.Element(NS + "Instructions")?.Elements();
    List<IOperation> ops = [];// = ParseOperations(instructionElements);

    // Parse file inferences
    XElement? fileInf = specElement.Element(NS + "FileInferences");
    List<InferenceNode> inferenceNodes = [];// = ParseFileInferences(fileInf);

    return new Spec
    {
      Name = name,
      Operations = [.. ops],
      FileInferences = [.. inferenceNodes],
      IsTextFile = true
    };
  }
}
