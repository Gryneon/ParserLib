namespace Parser.Ops;

public abstract class LoopOperation : Operation, IPlaceholderOperation
{
  public override bool NoOutput => true;
  public override bool NoInput => true;

  /// <summary>The name of this loop.</summary>
  public required string CursorKey { get; init; }
  /// <summary>Operations to perform.</summary>
  public IEnumerable<IOperation> Operations { get; init; } = [];

  /// <summary>Start of loop section.</summary>
  protected int LoopIndex { get; set; }
  /// <summary>The current index of the unpacker.</summary>
  protected int CurrentIndex { get; set; }
  protected abstract IOperation Continue { get; }
  protected abstract IOperation StartLoop { get; }
  protected IEnumerable<IOperation> InnerOperations => Operations.Select(item => item switch
  {
    OperationBreak ob => new OperationBreak(CurrentIndex, CursorKey),
    OperationContinue oc => Continue,
    _ => item
  });

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    CurrentIndex = index;
    LoopIndex = operations.Count;

    operations.Add(StartLoop);
    operations.AddRange(Operations);
    operations.Add(Continue);

    return operations.Count;
  }
}
