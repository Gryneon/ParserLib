namespace Parser.Ops;

public sealed class ForEachOperation (IEnumerable<IOperation> operations, string input_key) : Operation(input_key, SE), IOperation
{
  public int OpIndex { get; set; }
  public Collection<IOperation> Operations { get; } = [.. operations];
  bool IOperation.EndOperation => false;
  public override OpStatus DoOperation<TParser> (TParser parser_ref)
  {
    parser_ref.NextOpIndex = OpIndex;
    parser_ref.Cursor = 0;
    parser_ref.CursorKey = CursorKey;
    return OpStatus.Pass;
  }
  public string CursorKey { get; } = input_key;
}
