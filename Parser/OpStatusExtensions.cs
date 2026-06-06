namespace Parser;

public static class OpStatusExtensions
{
  extension(OpStatus status)
  {
    /// <summary>Checks a status for failure.</summary>
    /// <returns><see langword="true"/> if the status was a failure code, <see langword="false"/> otherwise.</returns>
    public bool IsFail => status.IsWithin(OpStatus.Fail, OpStatus.PastFail);
    public bool IsPass => !status.IsFail && !status.IsEnd;
    public bool IsEnd => status is OpStatus.EndCommand;
  }
}
