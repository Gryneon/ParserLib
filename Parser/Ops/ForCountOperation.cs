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
      if (WorkData is not int)
        Status = Op.ThrowBadInput("int", $"{WorkDataType}");
      TargetCount = (int) Data[InputKey];
    }

    Parser.Data.Save<bool>(CursorKey, true);
    Parser.AddCursor(CursorKey);
    Parser.SetNextOperationIndex(OpIndex);
  }
}

/// <summary>Iterates over the specified key, storing the current item in another key to be accessed.</summary>
public sealed class ForEachOperation : Operation, IPlaceholderOperation
{
  public override bool NoInput => true;
  public override bool NoOutput => true;
  /// <summary>The name of this loop.</summary>
  public string CursorKey { get; }
  /// <summary>The key storing the current item of the iteration.</summary>
  public string SelectedKey { get; }
  /// <summary>Operations to perform.</summary>
  public IEnumerable<IOperation> Operations { get; }
  /// <summary>Start of loop section.</summary>
  public int OpIndex { get; private set; }

  public ForEachOperation (string cursor_key, string selected_key, IEnumerable<IOperation> operations) : base(SE, SE)
  {
    CursorKey = cursor_key;
    SelectedKey = selected_key;
    Operations = operations;
  }

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    Collection<IOperation> additions = [];
    OpIndex = operations.Count;
    IOperation start = new OperationCheckCountOfKey(CursorKey, SelectedKey, index);
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

    Parser.AddCursor(CursorKey);
    Parser.SetNextOperationIndex(OpIndex);
  }
}
