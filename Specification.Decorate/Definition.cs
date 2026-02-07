#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1720 // Identifier contains type name
//#pragma warning disable IDE1006 // Naming Rule Violation

namespace Specification.Decorate;

[DefinitionExport]
public static class Definition
{
  [Export]
  public static Spec Spec => new()
  {
    FileInferences = [],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    IsTextFile = true,
    SC = SCOIC,
    TokenType = typeof(DecorateTokenType),
    Name = "zdoom.decorate",
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
      Operation.End
    ]
  };
}