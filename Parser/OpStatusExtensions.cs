namespace Parser;

public static class OpStatusExtensions
{
  /// <summary>Checks a status for failure.</summary>
  /// <param name="status">The status to check.</param>
  /// <param name="continueOnFail">The continue on fail value.</param>
  /// <returns></returns>
  public static bool IsFail (this OpStatus status, bool continueOnFail = false) =>
    status.IsWithin(OpStatus.Fail, OpStatus.PastFail) && !continueOnFail;
}
