
namespace Parser.Ops;

public sealed class OperationLabel (string name) : IOperation, IPlaceholderOperation
{
  public string Name { get; } = name;
  bool IOperation.ContinueOnFail { get; set; }
  public bool IgnoreAllLoads => true;
  bool IOperation.SkipOperation { get; set; }
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  public bool NeverExecutes => true;

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, IParser? parser_ref = null)
  {
    parser_ref?.Labels.Add(Name, index);
    operations.ThrowIfNull();
    operations.RemoveAt(index);
    return operations.Count;
  }

  OpStatus IOperation.DoOperation (IParser parser_ref) => throw new UnknownOperationException("Placeholder found in operation execution.");
}
