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

public sealed class StartLoopOperation (string key, int jumpDone) : IOperation
{
  public int NextOpIndex { get; set; } = jumpDone;
  public string CursorKey { get; set; } = key;
  public OpStatus Status { get; set; }
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.EndOperation => false;
  bool IOperation.DebugOperation { get; set; }

  public OpStatus DoOperation<TParser> (TParser parser_ref) where TParser : IParser
  {
    if (parser_ref.CountOfKey(CursorKey) <= parser_ref.Cursor)
    {
      parser_ref.NextOpIndex = NextOpIndex;
      parser_ref.Cursor = -1;
      parser_ref.CursorKey = null;
    }
    return OpStatus.Pass;
  }
}

public sealed class NextLoopOperation (int jumpLoopStart) : IOperation
{
  public int LoopStartIndex { get; set; } = jumpLoopStart;
  public OpStatus Status { get; set; }
  bool IOperation.ContinueOnFail { get; set; }
  bool IOperation.SkipOperation { get; set; }
  bool IOperation.EndOperation => false;
  bool IOperation.DebugOperation { get; set; }

  public OpStatus DoOperation<TParser> (TParser parser_ref) where TParser : IParser
  {
    parser_ref.Cursor++;
    parser_ref.NextOpIndex = LoopStartIndex;
    return OpStatus.Pass;
  }
}
