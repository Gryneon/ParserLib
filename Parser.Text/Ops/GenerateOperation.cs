using OS = Parser.OpStatus;

namespace Parser.Text.Ops;

/// <summary>
/// Text Parser Operation
/// <para>Performs a conditional conversion on an object, which can be a <see cref="MatchData"/>.</para>
/// </summary>
/// <typeparam name="TOut">The end result of generation.</typeparam>
/// <param name="output_key">The key to store the output objects in.</param>
/// <param name="func">The generation function.</param>
/// <param name="predicate">The condition that the generation function requires.</param>
/// <param name="input_key">The key to pull data from.</param>
public class GenerateOperation<TOut> (Func<MatchData, TOut> func, Predicate<IMatchItem> predicate, string input_key, string output_key) : TextOperation(input_key, output_key)
{
  /// <inheritdoc/>
  public override OS DoOperation (TextParser parser)
  {
    CheckOperationFlags();
    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;
    Initialize(parser);
    CheckInputNull();
    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    if (CheckInput(out IEnumerable<MatchData>? mdds))
    {
      foreach (MatchData mdd in mdds)
        Invoke(mdd);
    }
    else if (CheckInput(out MatchData? mdd))
      Invoke(mdd);
    else
      return OS.FailBadInputType;
    return OS.Pass;
  }

  protected virtual void Invoke (MatchData mdd)
  {
    if (predicate(mdd))
    {
      TOut output = func(mdd);

      if (output is null)
        return;

      AssignResult<TOut>(DictionaryMode.MakeList);
    }
  }
}
