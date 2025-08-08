using Common.Extensions;

using Parser.Ops;

namespace Parser.V2;

public class OpSection<T, TOutput>
  where T : class
{
  public void Validate () { }
  public Collection<IOperation> Operations { get; init; } = [];

  public bool IsCollectionOperation { get; init; }
  public bool IterateThroughOperationsNotData { get; init; }
  public OpSection () { }
  public OpSection (IEnumerable<IOperation> ops) => Operations.AddRange(ops);
}
