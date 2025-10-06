namespace Parser.Ops;

internal abstract class OperationAction : IOperation
{
  public OpStatus Status { get; }
  public bool ContinueOnFail { get; set; }
  public bool SkipOperation { get; set; }
  public bool DebugOperation { get; set; }

  bool IOperation.EndOperation => false;

  OpStatus IOperation.DoOperation<TParser> (TParser parser_ref) => OpStatus.Pass;
}
