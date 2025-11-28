namespace Parser.Ops.Text;
/// <summary>
/// An operation that logs the contents of the provided key.
/// </summary>
public class DebugToStringOperation : Operation
{
  private static string GetCaller (object? type) =>
    $"DebugToStringOperation.Execute()<{type?.GetType()}>";
  private static string GetCaller (string? type) =>
    $"DebugToStringOperation.Execute()<{type}>";
  /// <summary>
  /// Constructs an operation that logs the contents of the provided key.
  /// </summary>
  /// <param name="input_key">The key to output the contents of.</param>
  public DebugToStringOperation (string input_key) : base(input_key, EmptyString) { }

  /// <inheritdoc/>
  /// <remarks>This is a debugging operation and does not store data.</remarks>
  protected override void Execute ()
  {
    switch (WorkToReturn)
    {
      case string s:
        Log(GetCaller(s), s);
        break;
      case IEnumerable<string> strs:
        foreach (string str in strs)
          Log(GetCaller(str), str);
        break;
      case IEnumerable<MatchDataSet> mdds:
        foreach (MatchDataSet mdd in mdds)
          Log(GetCaller(mdds), mdd.ToString());
        break;
      case IEnumerable<IToken> itokens:
        foreach (IToken it in itokens)
          Log(GetCaller("Token"), it.ToString() ?? "<null data>");
        break;
      default:
        Log(GetCaller(WorkToReturn), WorkToReturn?.ToString() ?? "<null data>");
        break;
    }
  }
}
