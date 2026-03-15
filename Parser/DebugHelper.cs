namespace Parser;

public record class StackLoc (string ClassName, string Method);

public static class DebugHelper
{
  private static Collection<StackLoc> CallStack { get; } = [];
  private static string ThisClass => CallStack.Peek().ClassName;
  private static string ThisMethod => CallStack.Peek().Method;
  public static void LogAdv (MsgClass base_cls, string message_data)
  {
    // message
    // message {[MsgClass]colored message} base color
    //
    //
  }
  public static void Log (MsgClass cls, string message) => Debug.Log(cls, ThisClass, ThisMethod, message);
  public static void DebugIn (string method) => CallStack.Add(new(ThisClass, method));
  public static void DebugOut () => CallStack.Drop();
  public static void DebugIn (string classname, string method) => CallStack.Add(new(classname, method));
}
