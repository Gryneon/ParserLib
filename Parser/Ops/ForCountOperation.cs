namespace Parser.Ops;

public sealed class ForCountOperation : LoopOperation
{
  public string? LengthKey { get; init; }
  public int Length { get; set; } = -1;

  protected override IOperation Continue => new OperationContinue(LoopIndex, 1, CursorKey);
  protected override IOperation StartLoop => new OperationCheckCount(CursorKey, CurrentIndex);

  protected override void Execute ()
  {
    CheckUnpacked(LoopIndex);

    if (Length == -1)
    {
      Length = (int) Data[LengthKey];
    }

    Data[CursorKey] = new CursorData(Parser, 0, Length);
    Parser.SetNextOperationIndex(LoopIndex);
  }
}
