namespace Parser.Ops;

public sealed class OperationFail : Operation
{
  public override bool NoInput => true;
  public override bool NoOutput => true;
  protected override void Execute ()
  {
    Status = OpStatus.DefinedFail;
  }
}
