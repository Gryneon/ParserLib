namespace Parser.Ops;

public sealed class OperationFail : Operation
{
  protected override void Execute ()
  {
    Status = OpStatus.DefinedFail;
  }
}
