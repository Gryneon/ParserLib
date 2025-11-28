using OS = Parser.OpStatus;

namespace Parser.Ops.Text;

/// <summary>
/// Text Parser Operation
/// <para>Performs a conditional conversion on an object, which is a <see cref="MatchDataSet"/>.</para>
/// </summary>
/// <typeparam name="TOut">The end result of generation.</typeparam>
public class GenerateOperation<TOut> : Operation
{
  protected Dictionary<int, TOut> Results { get; } = [];
  protected Func<MatchDataSet, bool> Predicate { get; }
  protected Func<MatchDataSet, TOut> Function { get; }
  /// <summary>
  /// TODO: Document this operation.
  /// </summary>
  /// <param name="output_key">The key to store the output objects in.</param>
  /// <param name="func">The generation function.</param>
  /// <param name="predicate">The condition that the generation function requires.</param>
  /// <param name="input_key">The key to pull data from.</param>
  public GenerateOperation (Func<MatchDataSet, TOut> func, Func<MatchDataSet, bool> predicate, string input_key, string output_key) : base(input_key, output_key)
  {
    Predicate = predicate;
    Function = func;
  }
  public GenerateOperation (Func<MatchDataSet, TOut> func, string input_key, string output_key) : base(input_key, output_key)
  {
    Predicate = item => true;
    Function = func;
  }

  /// <inheritdoc/>
  protected override void Execute ()
  {
    if (CheckInput(out IEnumerable<MatchDataSet>? mdds))
    {
      Collection<MatchDataSet> mddList = mdds.ToCollection();
      for (int i = 0; i < mddList.Count; i++)
      {
        MatchDataSet mdd = mddList[i];

        if (Predicate(mdd))
        {
          TOut? result = Function(mdd);
          Results.Add(i, result);
        }
      }
      WorkToReturn = Results;
      Status = OS.Pass;
    }
    else if (CheckInput(out MatchDataSet? mdd))
    {
      if (Predicate(mdd))
      {
        TOut? result = Function(mdd);
        WorkToReturn = result;
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
