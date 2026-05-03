using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteRecallOperation (string input_key, string cursor_key) : Operation(input_key, SE)
{
  public override bool NoOutput => true;

  private readonly string _cursor_key = cursor_key;

  protected override void Execute ()
  {
    DebugIn(nameof(ByteRecallOperation), nameof(Execute));
    if (WorkData is int pos)
    {
      Parser.SetCursorByKey(_cursor_key, pos);
      _ = Parser.Data.Remove(InputKey);
      Log(MsgClass.GreenInfo, $"Position recalled {pos}, deleted '{InputKey}'.");
      Status = Pass;
    }
    else
    {
      Status = FailBadInputType;
    }
    DebugOut();
  }
}
