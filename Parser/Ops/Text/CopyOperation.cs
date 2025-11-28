namespace Parser.Ops.Text;

public class CopyOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  protected override void Execute () => Status = OpStatus.Pass;
}

public class StoreOperation (string input, string output_key) : Operation(SE, output_key)
{
  protected override void Execute ()
  {
    WorkToReturn = input;
    Status = OpStatus.Pass;
  }
}
