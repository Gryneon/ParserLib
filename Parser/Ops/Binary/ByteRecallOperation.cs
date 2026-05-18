using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteRecallOperation (string input_key, string cursor_key) : Operation(input_key, SE)
{
  public override bool NoOutput => true;

  private string CursorKey { get; } = cursor_key;

  protected override void Execute ()
  {
    DebugIn(nameof(ByteRecallOperation), nameof(Execute));
    if (WorkData is int pos)
    {
      Data.GetCursorByKey(CursorKey).Index = (int) Data[InputKey];
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
