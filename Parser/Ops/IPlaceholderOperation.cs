namespace Parser.Ops;

public interface IPlaceholderOperation
{
  int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null);
}
