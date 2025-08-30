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
public class GenerateOperation<TOut> (Func<MatchData, TOut> func, Func<IMatchItem, bool> predicate, string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected Dictionary<int, TOut> _results = [];
  /// <inheritdoc/>
  protected override void Execute ()
  {
    if (CheckInput(out IEnumerable<MatchData>? mdds))
    {
      Collection<MatchData> mddList = mdds.ToCollection();
      for (int i = 0; i < mddList.Count; i++)
      {
        MatchData mdd = mddList[i];

        if (predicate(mdd))
        {
          TOut? result = func(mdd);
          _results.Add(i, result);
        }
      }
      _workToReturn = _results;
      Status = OS.Pass;
    }
    else if (CheckInput(out MatchData? mdd))
    {
      if (predicate(mdd))
      {
        TOut? result = func(mdd);
        _workToReturn = result;
        Status = OS.Pass;
      }
      else
      {
        Status = OS.Skipped;
      }
    }
    else
      Status = OS.FailBadInputType;
  }
}
