#pragma warning disable CA1710 // Rename Parser.Library to end in either 'Dictionary' or 'Collection'

using System.Xml.Linq;

using Parser.Inference;

namespace Parser;

public static class SpecInstructionParser
{
  private static readonly XNamespace NS = "Parser/Spec";

  private static bool BooleanParse (string? text, bool value_on_fail)
  {
    return text is null
      ? value_on_fail
      : !text.Like(["false", "0", "no"]) && (text.Like(["true", "1", "yes"])
      ? true
      : throw Err.ThrowNoSpec($"Bad Boolean Value Encountered: {text}"));
  }

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
      string name = specxml.Element(NS + "Name")!.Value;
      string textfile = specxml.Element(NS + "TextFile")!.Value;
      bool is_textfile = BooleanParse(textfile, false);
      IEnumerable<XElement>? instructionElements = specxml.Element(NS + "Instructions")?.Elements();
      IEnumerable<IOperation> ops = instructionElements?.Select(OperationFactory.Produce) ?? [];
      XElement? fileInf = specxml.Element(NS + "FileInferences");
      ReadOnlyCollection<InferenceNode> inferenceNodes = [];// = ParseFileInferences(fileInf);

      Spec specobj = new()
      {
        Name = name,
        Operations = [.. ops],
        FileInferences = inferenceNodes,
        IsTextFile = is_textfile,
      };

      specs.Add(specobj);
    }

    return specs;
  }
}
