namespace Parser.Ops;

/// <summary>
/// The interface for all operations in the parser.
/// </summary>
public interface IOperation
{
  /// <summary>
  /// The current status of the operation.
  /// </summary>
  OpStatus Status { get; protected set; }
  /// <summary>
  /// Specifies that the operation should not stop the parser upon failure.
  /// </summary>
  bool ContinueOnFail { get; init; }
  /// <summary>
  /// Specifies that the operation should be skipped.
  /// </summary>
  bool SkipOperation { get; init; }
  /// <summary>
  /// Specifies that the operation is the last one.
  /// </summary>
  bool EndOperation { get; init; }
  /// <summary>
  /// Specifies that this is the first operation to be executed in the parser.
  /// </summary>
  bool StartOperation { get; init; }
  /// <summary>
  /// This option is used for debugging purposes, allowing the operation to output debug information, or allowing user input.
  /// </summary>
  bool DebugOperation { get; init; }

  /// <summary>
  /// Calls the operation with the provided data.
  /// </summary>
  /// <param name="data">The data to be operated on.</param>
  /// <returns>An <see cref="OpStatus"/> that represents the result status.</returns>
  OpStatus DoOperation (ref object data);
}
