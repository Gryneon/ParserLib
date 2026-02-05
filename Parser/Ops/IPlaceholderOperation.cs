namespace Parser.Ops;

/// <summary>For operations that require some work by the parser before they can be iterated through.</summary>
public interface IPlaceholderOperation
{
  /// <summary>Unpacks the operation into a flat structure.</summary>
  /// <param name="operations">The operation list.</param>
  /// <param name="index">The index of the operation sequencer.</param>
  /// <param name="parser_ref">The parser reference.</param>
  /// <returns>The operation count after unpacking.</returns>
  int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null);
}
