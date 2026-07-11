namespace Parser.Ops;

/// <summary>A collection of operations that are executed in the given order.</summary>
public sealed class OperationCollection : Operation, IReadOnlyCollection<IOperation>, IPlaceholderOperation
{
  private readonly List<IOperation> _operations;
  public ReadOnlyCollection<IOperation> Operations => [.. _operations];
  public int Count => Operations.Count;
  public override bool NoExecution => true;
  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    int nextOrEnd = index + 1 >= operations.Count ? -1 : index + 1;
    int first = operations.Count;
    operations.AddRange([.. Operations, JumpTo(nextOrEnd)]);
    operations.Replace(index, [JumpTo(first)]);
    return operations.Count;
  }

  public OperationCollection (IEnumerable<IOperation> ops)
  {
    _operations = [.. ops];
    _operations.ForEach(op => op.ApplyProperties(ContinueOnFail, SkipOperation));
  }
  public IEnumerator<IOperation> GetEnumerator () => _operations.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public void CheckUnpacked () => Err.ThrowUnpacked("This operation replaces itself with a jump operation, unpacking did not occur.");
}
