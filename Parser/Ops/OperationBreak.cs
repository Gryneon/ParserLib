namespace Parser.Ops;

public sealed class OperationBreak () : Operation
{
  public int BreakTarget { get; private set; }
  [AllowNull]
  public string BreakCursor { get; private set; }
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    Parser.SetNextOperationIndex(BreakTarget);
    Parser.RemCursorByKey(BreakCursor);
  }

  public void SetupBreakTarget (int target, string cursor_key)
  {
    BreakTarget = target;
    BreakCursor = cursor_key;
  }
}
