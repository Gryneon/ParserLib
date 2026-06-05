namespace Parser.Ops;

public sealed class WhileOperation : LoopOperation
{
  /// <summary>While Condition</summary>
  public required string? Condition { get; init; }
  protected override IOperation Continue => new OperationContinue(LoopIndex, 0, CursorKey);
  protected override IOperation StartLoop
  {
    get
    {
      if (Condition is null)
        Status = Err.ThrowBadDef("Condition is null on a while loop");
      return new OperationCheckLoopCondition(Condition, CursorKey, CurrentIndex);
    }
  }

  protected override void Execute ()
  {
    CheckUnpacked(LoopIndex);

    Data[CursorKey] = new CursorData(Parser);
    Parser.SetNextOperationIndex(LoopIndex);
  }
}
