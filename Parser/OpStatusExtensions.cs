namespace Parser;

public static class OpStatusExtensions
{
  /// <summary>Checks a status for failure.</summary>
  /// <param name="status">The status to check.</param>
  /// <param name="continueOnFail">The continue on fail value.</param>
  /// <returns><see langword="true"/> if the status was a failure code, <see langword="false"/> otherwise.</returns>
  /// <remarks>If the <paramref name="continueOnFail"/> value is <see langword="true"/>, this method always returns <see langword="true"/>.</remarks>
  public static bool IsFail (this OpStatus status, bool continueOnFail = false) =>
    status.IsWithin(OpStatus.Fail, OpStatus.PastFail) && !continueOnFail;
  public static bool IsPass (this OpStatus status, bool continueOnFail = false) =>
    !(status.IsFail(continueOnFail) || status.IsEnd());
  public static bool IsEnd (this OpStatus status) =>
    status == OpStatus.EndCommand;
}
