namespace Parser.Ops;

/// <summary>For operations that require some work by the parser before they can be iterated through.</summary>
public interface IPlaceholderOperation
{
  int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null);
}
