#pragma warning disable IDE0072 // Add missing cases

using OP = Parser.OpStatus;

namespace Parser.V2;

public struct ResultPackage
{
  public OP Status { get; set; }
  public dynamic Data { get; set; }
}
