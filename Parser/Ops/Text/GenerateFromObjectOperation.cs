namespace Parser.Ops.Text;

/// <summary>
/// Takes an input as a collection of <see cref="MatchDataSet"/> objects,
/// and generates a <see cref="Dictionary{TKey, TValue}"/> of items from it,
/// provided that the specified group (or groups) are present.
/// </summary>
/// <typeparam name="TOutput">The type of object to create.</typeparam>
/// <param name="input_key">The key to get the input from.</param>
/// <param name="output_key">The key to write the output to.</param>
/// <param name="group_name">The group name to check for.</param>
/// <remarks><code>
/// Inputs: IDictionary&lt;int, MatchData>, IEnumerable&lt;MatchData>
/// Output: <see cref="Dictionary{TKey,TValue}">Dictionary&lt;int, TOutput></see></code>
/// <br/>
/// Statuses:
/// <code>
/// <see cref="OpStatus.Pass"/>: Operation completed successfully.
/// <see cref="OpStatus.Skipped"/>: Operation completed successfully, but no work was done.
/// <see cref="OpStatus.FailOverride"/>: Operation failed, but is allowed to continue.
/// <see cref="OpStatus.FailBadInputType"/>: Operation was provided the wrong type as input.
/// <see cref="OpStatus.FailBadInputNull"/>: The data at the key was <see langword="null"/> or missing.
/// <see cref="OpStatus.FailNoSuchVarName"/>: The key was not found in the <see cref="DataDictionary"/>.
/// </code>
/// </remarks>
public class GenerateFromObjectOperation<TInput, TOutput> (string input_key, string output_key, string group_name) : Operation(input_key, output_key)
  where TOutput : IGeneratable<TInput, TOutput>
{
  /// <inheritdoc/>
  protected override void Execute ()
  {
    Dictionary<int, TOutput> output_items = [];
    if (CheckInput(out IDictionary<int, MatchDataSet>? dCasted))
    {
      Dictionary<int, MatchDataSet> dict = [.. dCasted];
      foreach (KeyValuePair<int, MatchDataSet> kvp in dict)
      {
        MatchDataSet mdd = kvp.Value;
        int index = kvp.Key;
        if (mdd.HasGroup(group_name) && TOutput.TryGenerate(mdd, out TOutput? result))
        {
          output_items.Add(index, result);
        }
      }
    }
    else if (CheckInput(out IEnumerable<MatchDataSet>? eCasted))
    {
      Collection<MatchDataSet> iterator = [.. eCasted];
      for (int index = 0; index < iterator.Count; index++)
      {
        MatchDataSet mdd = iterator[index];
        if (mdd.HasGroup(group_name) && TOutput.TryGenerate(mdd, out TOutput? result))
        {
          output_items.Add(index, result);
        }
      }
    }
    else
    {
      Status = OpStatus.FailBadInputType;
      return;
    }

    WorkToReturn = output_items;
    Status = output_items.Count > 0 ? OpStatus.Pass : OpStatus.Skipped;
  }
}
