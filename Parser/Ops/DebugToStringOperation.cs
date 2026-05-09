using Common.Regexp;

namespace Parser.Ops;
/// <summary>An operation that logs the contents of the provided key.</summary>
/// <remarks>Constructs an operation that logs the contents of the provided key.</remarks>
/// <param name="input_key">The key to output the contents of.</param>
public class DebugToStringOperation (string input_key) : Operation(input_key, SE)
{
  public override bool NoOutput => true;

  /// <inheritdoc/>
  /// <remarks>This is a debugging operation and does not store data.</remarks>
  protected override void Execute ()
  {
    DebugIn("DebugToStringOperation", "Execute");
    switch (WorkData)
    {
      case string s:
        Log(MsgClass.Debug, s);
        break;
      case IEnumerable<string> strs:
        foreach (string str in strs)
          Log(MsgClass.Debug, str);
        break;
      case IEnumerable<MatchDataSet> mdds:
        foreach (MatchDataSet mdd in mdds)
          Log(MsgClass.Debug, mdd.ToString2());
        break;
      case IEnumerable<IToken> itokens:
        foreach (IToken it in itokens)
          Log(MsgClass.Debug, it.ToString2() ?? "<null data>");
        break;
      default:
        Log(MsgClass.Debug, WorkData?.ToString2() ?? "<null data>");
        break;
    }
    DebugOut();
  }

  public override string ToString () => $"DebugToStringOperation Key = \"{InputKey}\"";
}
