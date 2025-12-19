namespace Parser.Ops.Text;

public class EncapsulateOperation<TParent, TChild> (string input_key, string output_key) : Operation(input_key, output_key) where TParent : class, ICanAddChildren<TChild>, new()
{
  protected override void Execute ()
  {
    IEnumerable<TChild> list;
    if (CheckInput(out IEnumerable<TChild>? collection))
    {
      list = collection;
    }
    else if (CheckInput(out IDictionary<int, TChild>? dict))
    {
      list = dict.Select(item => item.Value);
    }
    else
    {
      if (InputKey is null) throw new InvalidOperationException();
      Log("EncapsulateOperation", $"Input was a {Parser.Data[InputKey]}");
      Status = OpStatus.FailBadInputType;
      return;
    }

    TParent parent = new();

    foreach (TChild item in list)
      parent.Add(item);
    WorkToReturn = parent;
    Status = OpStatus.Pass;
  }
}
