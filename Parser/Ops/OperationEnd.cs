namespace Parser.Ops;

/// <summary>This operation ends the operation sequence with a success.</summary>
public sealed class OperationEnd() : Operation
{
  protected override void Execute ()
  {
    Parser.SetNextOperationIndex(-1);
    Status = OpStatus.EndCommand;
  }
}
