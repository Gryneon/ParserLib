namespace Parser.Ops;

/// <summary>A collection of operations that are executed in the given order.</summary>
public sealed class OperationCollection : IOperation, IReadOnlyCollection<IOperation>, IPlaceholderOperation
{
  public Collection<IOperation> Operations { get; init; }
  public OpStatus Status { get; set; }
  public bool ContinueOnFail { get; set; }
  public bool SkipOperation { get; set; }

  public int Count => Operations.Count;
  public bool NeverExecutes => true;

  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  bool IOperation.IgnoreAllLoads => true;

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    int nextOrEnd = index + 1 >= operations.Count ? -1 : index + 1;
    int first = operations.Count;
    operations.AddRange([.. Operations, Operation.JumpTo(nextOrEnd)]);
    operations.Replace(index, [Operation.JumpTo(first)]);
    return operations.Count;
  }

  public OperationCollection (IEnumerable<IOperation> ops)
  {
    ops.ThrowIfNull();
    ops = ops.Select(item => item.ApplyProperties(ContinueOnFail, SkipOperation));
    Operations = [.. ops];
  }
  OpStatus IOperation.DoOperation (XParser parser_ref) => throw new UnknownOperationException("Placeholder found in operation execution.");
  public IEnumerator<IOperation> GetEnumerator () => Operations.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
