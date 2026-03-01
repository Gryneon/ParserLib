namespace Parser.Ops;

/// <summary>A collection of operations that are executed in the given order.</summary>
public sealed class OperationCollection : Operation, IReadOnlyCollection<IOperation>, IPlaceholderOperation
{
  public Collection<IOperation> Operations { get; init; }

  public int Count => Operations.Count;
  public override bool NoInput => true;
  public override bool NoExecution => true;
  public override bool NoOutput => true;
  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    int nextOrEnd = index + 1 >= operations.Count ? -1 : index + 1;
    int first = operations.Count;
    operations.AddRange([.. Operations, Op.JumpTo(nextOrEnd)]);
    operations.Replace(index, [Op.JumpTo(first)]);
    return operations.Count;
  }

  public OperationCollection (IEnumerable<IOperation> ops)
  {
    ops.ThrowIfNull();
    Operations = [.. ops];
    foreach (IOperation op in Operations)
    {
      op.ApplyProperties(ContinueOnFail, SkipOperation);
    }
  }
  public IEnumerator<IOperation> GetEnumerator () => Operations.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
