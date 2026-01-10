#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
//#pragma warning disable IDE1006 // Naming Rule Violation

namespace Specification.Decorate;

//Decorate Prototype
//https://regex101.com/r/YtlFqj/1
//https://regex101.com/r/mTwORe/2

[DefinitionExport]
public static class Definition
{
  [Export("zdoom.decorate")]
  public static Spec Spec => new()
  {
    FileInferences = [],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    IsTextFile = true,
    SC = SCOIC,
    Name = "zdoom.decorate",
    Operations = [
      new TokenizeOperation(),
      Operation.End
    ]
  };
}