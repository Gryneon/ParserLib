using System.Collections;

namespace Parser.Ops;

public sealed class OperationCollection : IOperation, IReadOnlyCollection<IOperation>
{
  public Collection<IOperation> Operations { get; init; }
  public OpStatus Status { get; set; }
  public bool ContinueOnFail { get; set; }
  public bool SkipOperation { get; set; }
  bool IOperation.EndOperation => false;
  public bool DebugOperation { get; set; }
  public int Count => Operations.Count;

  public OperationCollection (IEnumerable<IOperation> ops)
  {
    ArgumentNullException.ThrowIfNull(ops, nameof(ops));
    foreach (IOperation item in ops)
    {
      item.ApplyProperties(ContinueOnFail, SkipOperation, DebugOperation);
    }
    Operations = [.. ops];
  }

  OpStatus IOperation.DoOperation<TParser> (TParser parser_ref) => OpStatus.Pass;
  public IEnumerator<IOperation> GetEnumerator () => Operations.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
