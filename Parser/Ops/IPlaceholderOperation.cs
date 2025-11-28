namespace Parser.Ops;

public interface IPlaceholderOperation
{
  int Unpack ([NotNull] Collection<IOperation> operations, int index, IParser? parser_ref = null);
}
