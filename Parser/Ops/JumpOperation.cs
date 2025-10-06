namespace Parser.Ops;

public sealed class JumpOperation (int jumpTo) : IOperation
{
  public int OpIndex { get; set; } = jumpTo;
  public OpStatus Status { get; set; }
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.EndOperation => false;
  bool IOperation.DebugOperation { get; set; }

  public OpStatus DoOperation<TParser> (TParser parser_ref) where TParser : IParser
  {
    parser_ref.NextOpIndex = OpIndex;
    return OpStatus.Pass;
  }
}
