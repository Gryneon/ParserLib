using Common.Extensions;

namespace Parser.V2.Framework;

public static class OpStatusExtensions
{
  public static bool IsFail (this OpStatus status, bool continueOnFail) =>
    status.IsWithin(OpStatus.Fail, OpStatus.PastFail) && !continueOnFail;
}
