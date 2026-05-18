using Common.RegExp;

using OS = Parser.OpStatus;

namespace Parser.Ops.Text;

/// <summary>
/// Takes an input as a collection of <see cref="MatchDataSet"/> objects,
/// and generates a <see cref="Dictionary{TKey, TValue}"/> of items from it,
/// provided that the specified group (or groups) are present.
/// </summary>
/// <typeparam name="TIn">The accepted input.</typeparam>
/// <typeparam name="TOut">The type of object to create.</typeparam>
/// <remarks><code>
/// Inputs: IDictionary&lt;int, MatchData>, IEnumerable&lt;MatchData>
/// Output: <see cref="Dictionary{TKey,TValue}">Dictionary&lt;int, TOutput></see></code>
/// <br/>
/// Statuses:
/// <code>
/// <see cref="OS.Pass"/>: Operation completed successfully.
/// <see cref="OS.Skipped"/>: Operation completed successfully, but no work was done.
/// <see cref="OS.FailOverride"/>: Operation failed, but is allowed to continue.
/// <see cref="OS.FailBadInputType"/>: Operation was provided the wrong type as input.
/// <see cref="OS.FailBadInputNull"/>: The data at the key was <see langword="null"/> or missing.
/// <see cref="OS.FailNoSuchVarName"/>: The key was not found in the <see cref="DataStore"/>.
/// </code>
/// </remarks>
public class GenerateOperation<TIn, TOut> : Operation
{
  protected Dictionary<int, TOut> Results { get; } = [];
  protected Predicate<TIn> Predicate { get; }
  protected Func<TIn, TOut> Function { get; }
  /// <summary>
  /// Generates a <typeparamref name="TOut"/> from a <typeparamref name="TIn"/>.
  /// Validates the <typeparamref name="TOut"/> against the <paramref name="predicate"/> and stores the result in <paramref name="output_key"/>.
  /// </summary>
  /// <param name="func">The generation function.</param>
  /// <param name="predicate">The condition that the generation function requires.</param>
  /// <param name="input_key">The key to pull data from.</param>
  /// <param name="output_key">The key to store the output objects in.</param>
  public GenerateOperation (Func<TIn, TOut> func, Predicate<TIn> predicate, string input_key, string output_key) : base(input_key, output_key)
  {
    Predicate = predicate;
    Function = func;
  }
  public GenerateOperation (Func<TIn, TOut> func, string input_key, string output_key) : base(input_key, output_key)
  {
    Predicate = _ => true;
    Function = func;
  }

  protected override void Execute ()
  {
    if (WorkData is IEnumerable<TIn> mdds)
    {
      Collection<TIn> mddList = [.. mdds];
      for (int i = 0; i < mddList.Count; i++)
      {
        TIn mdd = mddList[i];

        if (Predicate(mdd))
        {
          TOut? result = Function(mdd);
          Results.Add(i, result);
        }
      }
      WorkData = Results;
      Status = OS.Pass;
    }
    else if (WorkData is TIn mdd)
    {
      if (Predicate(mdd))
      {
        TOut? result = Function(mdd);
        WorkData = result;
        Status = OS.Pass;
      }
      else
      {
        Status = OS.Skipped;
      }
    }
    else
    {
      Status = Op.ThrowBadInput($"{typeof(TIn)} or {typeof(IEnumerable<TIn>)}", $"{WorkData?.GetType()}");
    }
  }
}
