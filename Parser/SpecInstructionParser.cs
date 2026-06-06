#pragma warning disable CA1710 // Rename Parser.Library to end in either 'Dictionary' or 'Collection'

using System.Xml.Linq;

using Parser.Inference;

namespace Parser;

public static class SpecInstructionParser
{
  private static readonly XNamespace NS = "Parser/Spec";

  public static IEnumerable<Spec> LoadSpecFile (string path)
  {
    Collection<Spec> specs = [];

    XDocument doc = XDocument.Load(path);
    XElement root = doc.Root ?? throw Err.ThrowNoSpec("Spec XML is not good.");
    XElement? prefabs = root.Element(NS + "Prefabs");
    IEnumerable<XElement>? prefab_list = prefabs?.Elements(NS + "Prefab");
    IEnumerable<XElement>? tokenRules_list = prefab_list?.Elements(NS + "TokenRule");
    IEnumerable<XElement>? tokenLookups_list = prefab_list?.Elements(NS + "TokenLookup");
    IEnumerable<XElement>? tokenGroups_list = prefab_list?.Elements(NS + "GroupTokenRule");
    IEnumerable<XElement>? constructs_list = prefab_list?.Elements(NS + "Construct");

    XElement spec = root.Name.LocalName == "Definition" ? root.Element(NS + "Spec") ?? Err.ThrowNoSpec("No Spec in definition.") : Err.ThrowNoSpec("Root element must be Definition.");

    foreach (XElement specxml in root.Elements(NS + "Spec"))
    {
      XElement? xname = specxml.Element(NS + "Name");

      if (xname is null || string.IsNullOrEmpty(xname.Value))
        Err.ThrowNoSpec("Invalid XML - No Name in Spec.");

      XElement? tokenLookups = specxml.Element(NS + "TokenLookups");
      XElement? tokenRules = specxml.Element(NS + "TokenRules");

      string name = spec.Element(NS + "Name")!.Value;
      bool? textfile = bool.TryParse(root.Element(NS + "TextFile")?.Value, out bool result) ? result : null;
      // Parse instructions
      IEnumerable<XElement>? instructionElements = specxml.Element(NS + "Instructions")?.Elements();
      ReadOnlyCollection<IOperation> ops = [.. instructionElements?.Select(OperationFactory.Produce) ?? []];

      // Parse file inferences
      XElement? fileInf = root.Element(NS + "FileInferences");
      ReadOnlyCollection<InferenceNode> inferenceNodes = [];// = ParseFileInferences(fileInf);

      var specobj = new Spec
      {
        Name = name,
        Operations = ops,
        FileInferences = inferenceNodes,
        IsTextFile = textfile ?? true,
      };

      specs.Add(specobj);
    }

    return specs;
  }
}
