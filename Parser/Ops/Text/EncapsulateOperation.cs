namespace Parser.Ops.Text;

public class EncapsulateOperation<TParent, TChild> (string input_key, string output_key) : Operation(input_key, output_key) where TParent : class, ICanAddChildren<TChild>, new()
{
  protected override void Execute ()
  {
    IEnumerable<TChild> list = WorkToReturn is IEnumerable<TChild> collection
      ? collection
      : WorkToReturn is IDictionary<int, TChild> dict
        ? dict.Select(item => item.Value)
        : throw new OperationBadInputTypeException($"{typeof(IEnumerable<TChild>)}", $"{WorkToReturn?.GetType()}");
    TParent parent = new();

    foreach (TChild item in list)
      parent.Add(item);
    WorkToReturn = parent;
    Status = OpStatus.Pass;
  }
}
