#pragma warning disable CA1710 // Rename Parser.Library to end in either 'Dictionary' or 'Collection'

using System.Xml.Linq;

using Parser.Inference;

namespace Parser;

public static class SpecInstructionParser
{
  private static readonly XNamespace NS = "Parser/Spec";

  public static Spec LoadSpec (string path)
  {
    XDocument doc = XDocument.Load(path);
    XElement root = doc.Root ?? throw Err.ThrowNoSpec("Spec XML is not good.");
    XElement spec = root.Name.LocalName.Equals("Definition", SCO) ? root.Element(NS + "Spec") ?? Err.ThrowNoSpec("No Spec in definition.") : Err.ThrowNoSpec("Root element must be Definition.");
    XElement? prefabs = root.Element(NS + "Prefabs");
    var prefab_list = prefabs?.Elements(NS + "Prefab");
    var tokenRules_list = prefab_list?.Elements(NS + "TokenRule");
    var tokenLookups_list = prefab_list?.Elements(NS + "TokenLookup");
    var tokenGroups_list = prefab_list?.Elements(NS + "GroupTokenRule");
    var constructs_list = prefab_list?.Elements(NS + "Construct");
    XElement? tokenLookups = root.Element(NS + "TokenLookups");
    XElement? tokenRules = root.Element(NS + "TokenRules");

    string name = (root.Element(NS + "Name") ?? throw Err.ThrowNoSpec("Invalid XML - No Name in Spec.")).Value;
    bool? textfile = bool.TryParse(root.Element(NS + "TextFile")?.Value, out bool result) ? result : null;
    // Parse instructions
    IEnumerable<XElement>? instructionElements = root.Element(NS + "Instructions")?.Elements();
    ReadOnlyCollection<IOperation> ops = [.. instructionElements?.Select(OperationFactory.Produce) ?? []];

    // Parse file inferences
    XElement? fileInf = root.Element(NS + "FileInferences");
    ReadOnlyCollection<InferenceNode> inferenceNodes = [];// = ParseFileInferences(fileInf);

    return new Spec
    {
      Name = name,
      Operations = ops,
      FileInferences = inferenceNodes,
      IsTextFile = textfile ?? true,
    };
  }
}
