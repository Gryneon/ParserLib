namespace Parser.Ops;

public sealed class WhileOperation : Operation, IPlaceholderOperation
{
  public override bool NoOutput => true;
  public override bool NoInput => true;
  /// <summary>The name of this loop.</summary>
  public required string CursorKey { get; init; }
  /// <summary>Operations to perform.</summary>
  public IEnumerable<IOperation> Operations { get; init; } = [];
  /// <summary>While Condition</summary>
  public required string Condition { get; init; }
  /// <summary>Start of loop section.</summary>
  public int OpIndex { get; private set; }

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    Collection<IOperation> additions = [];
    OpIndex = operations.Count;
    IOperation start = new OperationCheckLoopCondition(Condition, CursorKey, index);
    additions.Add(start);
    additions.AddRange(Operations);
    additions.Add(new OperationContinue());
    foreach (IOperation op in additions)
    {
      if (op is OperationBreak ob)
        ob.SetupBreakTarget(index, CursorKey);

      if (op is OperationContinue oc)
        oc.SetupContinue(OpIndex, 0, CursorKey);
    }
    operations.AddRange(additions);
    return operations.Count;
  }

  protected override void Execute ()
  {
    if (OpIndex == 0)
      Status = Err.ThrowBadDef("Loop Pre-processing not complete.");

    if (Condition is null)
      Status = Err.ThrowBadDef("Condition is null on a while loop");

    Parser.Data.Save<bool>(CursorKey, true);
    Data[CursorKey] = new CursorData(Parser);
    Parser.SetNextOperationIndex(OpIndex);
  }
}
