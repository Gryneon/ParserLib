using Parser.Ops;
using Parser.Text.Ops;

using static Parser.DefinitionStaticFunctions;

namespace Specification.INI;

/// <summary>
/// Defines the INI spec.
/// </summary>
public static class Definition
{
  /// <summary>
  /// The INI Specification.
  /// </summary>
  public static TextSpec Spec => new()
  {
    Name = "ini",
    FileInferences = [
      IfN(ExtIs, "ini"),
      IfN(ExtIs, "vnc"),
      IfN(ExtIs, "inf"),
    ],
    Operations = [
      new DictionaryOperation(Nm("full", @"(?'section'\[(?'name'.*?)\])(\s*(?'key'\w*)\s*\=\s*(?'value'[^;\n]*))*")),
      new GenerateOperation<Section>(Section.Generate, item => item.HasGroup("section"), "matches","sections"),
      new EncapsulateOperation<DocumentSet, Section>("sections", "result"),
      Operation.End
    ],
    CaseInsensitive = false
  };
}
