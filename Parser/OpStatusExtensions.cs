using Common.Extensions;

namespace Parser;

public static class OpStatusExtensions
{
  public static bool IsFail (this OpStatus status, bool continueOnFail = false) =>
    status.IsWithin(OpStatus.Fail, OpStatus.PastFail) && !continueOnFail;
}
