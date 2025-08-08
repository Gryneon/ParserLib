#pragma warning disable IDE0072 // Add missing cases

namespace Parser.V2;

public enum ParserStatus
{
  Error = -1,
  Unknown = 0,

  Files_Present = 1,
  Data_Present = 2,
  Ops_Loaded = 4,
  Spec_Loaded = 8,
  LastOp_Passed = 16,

  Fail = 256,
}
