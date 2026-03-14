namespace Parser.Ops.Text;

/// <summary>Combines multiple keys into one key. Keys must have a common type or interface.</summary>
/// <typeparam name="TCommon">The common type or interface.</typeparam>
/// <param name="input_keys">The keys to combine.</param>
/// <param name="output_key">The combined keys.</param>
public class ConsolidateOperation<TCommon> (IEnumerable<string> input_keys, string output_key) : Operation(input_keys, output_key)
{
  /// <inheritdoc/>
  protected override void Execute ()
  {
    Collection<TCommon> output_items = [];

    int index = 0;
    int count = MultipleInputValues?.Sum(input_value => input_value is IEnumerable<object> countable ? countable.Count() : 0) ?? 0;
    while (MultipleInputValues is not null && index < count)
    {
      for (int i = 0; i < MultipleInputValues.Count; i++)
      {
        if (MultipleInputValues[i] is IDictionary<int, TCommon> idict && idict.TryGetValue(index, out TCommon? value))
        {
          output_items.Add(value);
          index++;
          break;
        }
        else
        {
          Status = Op.ThrowBadResult($"Type mismatch against {typeof(TCommon)}");
        }
      }
      Log("ConsolidateOperation", "Execute", $"Index {index} Skipped.");
      index++;
    }

    WorkData = output_items;
    Status = OpStatus.Pass;
  }
}

