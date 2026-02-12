namespace Parser.Ops;

public sealed class OperationLabel (string name) : IOperation, IPlaceholderOperation
{
  /// <summary>The name of this label.</summary>
  public string Name { get; } = name;
  bool IOperation.ContinueOnFail { get; set; }
  public bool NoInput => true;
  public bool NoOutput => true;
  bool IOperation.SkipOperation { get; set; }
  public int LoopBreak { get; set; }
  public int LoopStart { get; set; }
  /// <summary>This operation performs no action, and advances to the next.</summary>
  public bool NoExecution => true;

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    parser_ref?.Labels.Add(Name, index);
    operations.ThrowIfNull();
    return operations.Count;
  }

  public OpStatus DoOperation (XParser parser_ref) => OpStatus.Pass;
}
