namespace Parser.Ops.Text;

public class TrimOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  protected override void Execute ()
  {
    Status = OpStatus.Pass;
    WorkData = WorkData switch
    {
      string s => s.Trim(),
      IEnumerable<string> ien => ien.Select(x => x.Trim()).ToCollection(),
      _ => WorkData
    };

    if (WorkData is not string and not IEnumerable<string>)
      Status = Op.ThrowBadInput("string or list", $"{WorkData?.GetType()}");
  }
}
