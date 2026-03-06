namespace Parser.Ops;

public sealed class WhileOperation (string cursor_key, ICondition condition, IEnumerable<IOperation> operations) : Operation, IPlaceholderOperation
{
  /// <summary>The name of this loop.</summary>
  public string CursorKey { get; } = cursor_key;
  /// <summary>Operations to perform.</summary>
  public IEnumerable<IOperation> Operations { get; } = operations;
  /// <summary>While Condition</summary>
  public ICondition Condition { get; } = condition;
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
      Status = Op.ThrowBadDef("Loop Pre-processing not complete.");

    if (Condition is null)
      Status = Op.ThrowBadDef("Condition is null on a while loop");

    if (Condition.Evaluate(Parser))
    {
      Parser.Data.Save<bool>(CursorKey, true);
      Parser.AddCursor(CursorKey);
      Parser.SetNextOperationIndex(OpIndex);
    }
  }
}
