namespace Parser.Text.Ops;

public class CopyOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute () => Status = OpStatus.Pass;
}
