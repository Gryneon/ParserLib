
using static Common.Debug;

namespace Parser.Text.Ops;
/// <summary>
/// An operation that logs the contents of the provided key.
/// </summary>
public class DebugToStringOperation : TextOperation
{
  private static string GetCaller (object? type) =>
    $"DebugToStringOperation.Execute()<{type?.GetType()}>";
  private static string GetCaller (string? type) =>
    $"DebugToStringOperation.Execute()<{type}>";
  /// <summary>
  /// Constructs an operation that logs the contents of the provided key.
  /// </summary>
  /// <param name="input_key">The key to output the contents of.</param>
  public DebugToStringOperation (string input_key) : base(input_key, EmptyString) =>
    DebugOperation = true;

  /// <inheritdoc/>
  /// <remarks>This is a debugging operation and does not store data.</remarks>
  protected override void Execute ()
  {
    if (WorkToReturn is string s)
      Log(GetCaller(s), s);
    else if (WorkToReturn is IEnumerable<string> strs)
      foreach (string str in strs)
        Log(GetCaller(str), str);
    else if (WorkToReturn is IEnumerable<MatchDataSet> mdds)
      foreach (MatchDataSet mdd in mdds)
        Log(GetCaller(mdds), mdd.ToString());
    else if (WorkToReturn is IEnumerable<IToken> itokens)
      foreach (IToken it in itokens)
        Log(GetCaller("Token"), it.ToString() ?? "<null data>");
    else
      Log(GetCaller(WorkToReturn), WorkToReturn?.ToString() ?? "<null data>");
  }
}
