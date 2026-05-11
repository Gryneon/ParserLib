#pragma warning disable CA1822 // Mark members as static

using System.Xml.Linq;

using Parser.Inference;

namespace Parser;

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
