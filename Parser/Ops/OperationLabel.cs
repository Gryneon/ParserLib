namespace Parser.Ops;

public sealed class OperationLabel (string name) : Operation, IPlaceholderOperation
{
  /// <summary>The name of this label.</summary>
  public string Name { get; } = name;
  public override bool NoInput => true;
  public override bool NoExecution => true;
  public override bool NoOutput => true;

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    parser_ref?.Labels.Add(Name, index);
    operations.ThrowIfNull();
    return operations.Count;
  }

  protected override void Execute ()
  {

  }
}
