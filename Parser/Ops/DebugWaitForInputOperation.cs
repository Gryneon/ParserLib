namespace Parser.Ops;

/// <include file="operation.xml" path="/doc/members/member[@name=&quot;T:Parser.Ops.Operation&quot;]"></include>
/// <summary>Constructs an operation that waits for a specific key press or the enter key.</summary>
/// <param name="key">The key to wait for.</param>
public class DebugWaitForInputOperation (ConsoleKey key = ConsoleKey.Enter) : Operation()
{
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute ()
  {
    ConsoleKeyInfo keyInfo;
    Log(MsgClass.Prompt, $"Press the {key} key to continue.");
    do keyInfo = Console.ReadKey(true);
    while (keyInfo.Key != key);
  }
}
