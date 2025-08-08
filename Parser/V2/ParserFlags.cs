#pragma warning disable IDE0072 // Add missing cases

namespace Parser.V2;

public enum ParserFlags
{
  Error = -1,
  Unknown = 0,

  DoNotAlterOriginal = 1,
  MaintainExactOrder = 2,
  UseRegularExps = 4,
  MakeSingleResult = 8,
}
