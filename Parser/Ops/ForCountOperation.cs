namespace Parser.Ops;

public sealed class ForCountOperation : Operation, IPlaceholderOperation
{
  public override bool NoInput => true;
  public override bool NoOutput => true;
  /// <summary>The name of this loop.</summary>
  public required string CursorKey { get; init; }
  public string? LengthKey { get; init; }
  public int Length { get; set; } = -1;
  /// <summary>Operations to perform.</summary>
  public IEnumerable<IOperation> Operations { get; init; }
  /// <summary>Start of loop section.</summary>
  public int OpIndex { get; private set; }

  public ForCountOperation (string cursor_key, int target_count, IEnumerable<IOperation> operations)
  {
    CursorKey = cursor_key;
    Operations = operations;
    Length = target_count;
  }
  public ForCountOperation (string cursor_key, string length_key, IEnumerable<IOperation> operations)
  {
    CursorKey = cursor_key;
    LengthKey = length_key;
    Operations = operations;
  }

  public ForCountOperation ()
  {
  }

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    Collection<IOperation> additions = [];
    OpIndex = operations.Count;
    IOperation start = new OperationCheckCount(CursorKey, index);
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

    if (Length == -1)
    {
      if (WorkData is not int)
        Status = Op.ThrowBadInput("int", $"{WorkDataType}");
      Length = (int) Data[InputKey];
    }

    Data[CursorKey] = new CursorData(Parser, 0, Length != -1 ? Length : (int) Data[LengthKey]);
    Parser.SetNextOperationIndex(OpIndex);
  }
}
