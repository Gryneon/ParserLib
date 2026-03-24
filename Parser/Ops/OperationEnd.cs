namespace Parser.Ops;

public sealed class OperationEnd() : Operation
{
  protected override void Execute ()
  {
    Parser.SetNextOperationIndex(-1);
  }
}
