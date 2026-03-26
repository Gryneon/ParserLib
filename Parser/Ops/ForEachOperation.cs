namespace Parser.Ops;

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
    CheckUnpacked(Parser);

    Parser.AddCursor(CursorKey);
    Parser.SetNextOperationIndex(OpIndex);
  }
}
