namespace Parser.Ops.Text;

/// <summary>
/// Takes an input as a collection of <see cref="MatchDataSet"/> objects,
/// and generates a <see cref="Dictionary{TKey, TValue}"/> of items from it,
/// provided that the specified group (or groups) are present.
/// </summary>
/// <typeparam name="TInput">The type of object to accept as input.</typeparam>
/// <typeparam name="TOutput">The type of object to create.</typeparam>
/// <param name="input_key">The key to get the input from.</param>
/// <param name="output_key">The key to write the output to.</param>
/// <param name="group_name">The group name to check for.</param>
/// <remarks><code>
/// Inputs: IDictionary&lt;int, MatchData>, IEnumerable&lt;MatchData>
/// Output: <see cref="Dictionary{TKey,TValue}">Dictionary&lt;int, TOutput></see></code>
/// </remarks>
/// <exception cref="OperationBadInputTypeException"/>
/// <exception cref="OperationNoSuchVarException"/>
public class GenerateFromObjectOperation<TInput, TOutput> (string input_key, string output_key, string group_name) : Operation(input_key, output_key)
  where TOutput : IGeneratable
{
  /// <inheritdoc/>
  protected override void Execute ()
  {
    Dictionary<int, TOutput> output_items = [];
    if (WorkData is IDictionary<int, MatchDataSet> dCasted)
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
    else if (WorkData is IEnumerable<MatchDataSet> eCasted)
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
      Status = Op.ThrowBadInput("IDictionary<int, MatchDataSet> or IEnumerable<MatchDataSet>", $"{WorkData?.GetType()}");
      return;
    }

    WorkData = output_items;
    Status = output_items.Count > 0 ? OpStatus.Pass : OpStatus.Skipped;
  }
}
