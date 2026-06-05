namespace Parser.Ops.Text;

public class EncapsulateOperation<TParent, TChild> (string input_key, string output_key) : Operation(input_key, output_key) where TParent : class, ICanAddChildren<TChild>, new()
{
  protected override void Execute ()
  {
    IEnumerable<TChild> list = [];
    list = WorkData switch
    {
      IEnumerable<TChild> col => col,
      IDictionary<int, TChild> dic => dic.Select(item => item.Value),
      _ => Err.ThrowBadInput("IEnumerable<TChild> or IDictionary<int,TChild>", WorkData.TypeName)
    };
    TParent parent = new();

    foreach (TChild item in list)
      parent.Add(item);
    WorkData = parent;
    Status = OpStatus.Pass;
  }
}
