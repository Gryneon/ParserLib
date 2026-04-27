namespace Parser.Ops;

/// <include file="operation.xml" path="/doc/members/member[@name=&quot;T:Parser.Ops.Operation`1.DoOperation(`0)&quot;]"></include>
/// <summary>Constructs an operation that waits for a specific key press or the enter key.</summary>
/// <param name="key">The key to wait for.</param>
public class DebugWaitForInputOperation (ConsoleKey? key = null) : Operation()
{
  private readonly ConsoleKey? _key = key;
  private const string Area = "DebugWaitForInputOperation";

  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    ConsoleKeyInfo keyInfo;

    if (_key is null)
    {
      Log(MsgClass.Critical, Area, "Execute", "Press the enter key to continue.");
      do keyInfo = Console.ReadKey(true);
      while (keyInfo.Key != ConsoleKey.Enter);
    }
    else
    {
      Log(MsgClass.Critical, Area, "Execute", $"Press the {_key.Value} key to continue.");
      do keyInfo = Console.ReadKey(true);
      while (keyInfo.Key != _key);
    }
  }
}
