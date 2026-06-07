namespace Parser.Ops;
/// <summary>An operation that logs the contents of the provided key.</summary>
public class DebugToStringOperation : Operation
{
  /// <summary>Constructs an operation that logs the contents of the provided key.</summary>
  /// <param name="input_key">The key to output the contents of.</param>
  public DebugToStringOperation (string input_key)
  {
    LengthKey = input_key;
  }

  public override bool NoOutput => true;

  /// <inheritdoc/>
  /// <remarks>This is a debugging operation and does not store data.</remarks>
  protected override void Execute ()
  {
    switch (Data[LengthKey])
    {
      case string s:
        Log(MsgClass.Debug, s);
        break;
      case IEnumerable<string> strs:
        foreach (string str in strs)
          Log(MsgClass.Debug, str);
        break;
      case IEnumerable<IToken> itokens:
        foreach (IToken it in itokens)
          Log(MsgClass.Debug, it.ToString2() ?? "<null data>");
        break;
      default:
        Log(MsgClass.Debug, WorkData?.ToString2() ?? "<null data>");
        break;
    }
  }

  public override string ToString () => $"DebugToStringOperation Key = \"{LengthKey}\"";
}
