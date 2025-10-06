namespace Parser.Ops;

public sealed class ForEachOperation (IEnumerable<IOperation> sequence) : Operation, IOperation
{
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.EndOperation => false;
  bool IOperation.DebugOperation { get; set; }

  public override OpStatus DoOperation<TParser> (TParser parser_ref)
  {
    CheckInputNull();

    foreach (IOperation op in sequence)
    {
      OpStatus result = op.DoOperation(parser_ref);
      if (result.IsFail(op.ContinueOnFail))
      {
        Status = result;
        return Status;
      }
    }
    return OpStatus.Pass;
  }
}
