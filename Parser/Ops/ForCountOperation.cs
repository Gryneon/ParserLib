namespace Parser.Ops;

public sealed class ForCountOperation : Operation, IPlaceholderOperation
{
  public override bool NoInput { get; }
  public override bool NoOutput => true;
  /// <summary>The name of this loop.</summary>
  public string CursorKey { get; }
  public int TargetCount { get; private set; }
  /// <summary>Operations to perform.</summary>
  public IEnumerable<IOperation> Operations { get; }
  /// <summary>Start of loop section.</summary>
  public int OpIndex { get; private set; }

  public ForCountOperation (string cursor_key, int target_count, IEnumerable<IOperation> operations)
  {
    CursorKey = cursor_key;
    Operations = operations;
    TargetCount = target_count;
    NoInput = true;
  }
  public ForCountOperation (string cursor_key, string input_key, IEnumerable<IOperation> operations) : base(input_key, SE)
  {
    CursorKey = cursor_key;
    Operations = operations;
    TargetCount = -1;
    NoInput = false;
  }

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    Collection<IOperation> additions = [];
    OpIndex = operations.Count;
    IOperation start = new OperationCheckCount(TargetCount, CursorKey, index);
    additions.Add(start);
    additions.AddRange(Operations);
    additions.Add(new OperationContinue());
    foreach (IOperation op in additions)
    {
      if (op is OperationBreak ob)
        ob.SetupBreakTarget(index, CursorKey);

      if (op is OperationContinue oc)
        oc.SetupContinue(OpIndex, 1, CursorKey);
    }
    operations.AddRange(additions);
    return operations.Count;
  }

  protected override void Execute ()
  {
    if (OpIndex == 0)
      Status = Op.ThrowBadDef("Loop Pre-processing not complete.");

    if (TargetCount == -1)
    {
      if (InputKey.IsEmpty())
        Status = Op.ThrowNoVar("Key name is invalid.");
      TargetCount = (int) Data[InputKey];
    }

    Parser.Data.Save<bool>(CursorKey, true);
    Parser.AddCursor(CursorKey);
    Parser.SetNextOperationIndex(OpIndex);
  }
}
