namespace Parser.Ops.Text;

public class EncapsulateOperation<TParent, TChild> (string input_key, string output_key) : Operation(input_key, output_key) where TParent : class, ICanAddChildren<TChild>, new()
{
  protected override void Execute ()
  {
    IEnumerable<TChild> list = [];
    if (WorkData is IEnumerable<TChild> collection)
      list = collection;
    else
      if (WorkData is IDictionary<int, TChild> dict)
        list = dict.Select(item => item.Value);
      else
        Status = Op.ThrowBadInput($"{typeof(IEnumerable<TChild>)}", $"{WorkData?.GetType()}");

    TParent parent = new();

    foreach (TChild item in list)
      parent.Add(item);
    WorkData = parent;
    Status = OpStatus.Pass;
  }
}
