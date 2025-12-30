namespace Parser.Ops;

public class DebugWaitForInputOperation : Operation
{
  private readonly ConsoleKey? _key;
  private const string Area = "DebugWaitForInputOperation";

  /// <summary>
  /// Constructs an operation that waits for a specific key press or the enter key.
  /// </summary>
  /// <param name="key">The key to wait for.</param>
  public DebugWaitForInputOperation (ConsoleKey? key = null) : base()
  {
    _key = key;
  }

  protected override void Execute ()
  {
    ConsoleKeyInfo keyInfo;

    if (_key is null)
    {
      Log(Area, "Execute", "Press the enter key to continue.");
      keyInfo = Console.ReadKey(true);
      while (keyInfo.Key != ConsoleKey.Enter)
        keyInfo = Console.ReadKey(true);
    }
    else
    {
      Log(Area, "Execute", $"Press the {_key.Value} key to continue.");
      do
      {
        keyInfo = Console.ReadKey(true);
      } while (keyInfo.Key != _key);
    }
  }
}
