namespace Parser;

/// <summary>
/// The interface for all operations in the parser.
/// </summary>
public interface IOperation
{
  /// <summary>
  /// The current status of the operation.
  /// </summary>
  OpStatus Status { get; }
  /// <summary>
  /// Specifies that the operation should not stop the parser upon failure.
  /// </summary>
  bool ContinueOnFail { get; set; }
  /// <summary>
  /// Specifies that the operation should be skipped.
  /// </summary>
  bool SkipOperation { get; set; }
  /// <summary>
  /// Specifies that the operation is the last one.
  /// </summary>
  bool EndOperation { get; }
  /// <summary>
  /// This option is used for debugging purposes, allowing the operation to output debug information, or allowing user input.
  /// </summary>
  bool DebugOperation { get; set; }

  /// <summary>
  /// Calls the operation with from the parser provided.
  /// </summary>
  /// <typeparam name="TParser">The type of parser.</typeparam>
  /// <param name="parser_ref">The parser to pull data from and to store data in.</param>
  /// <returns>An <see cref="OpStatus"/> that represents the result status.</returns>
  OpStatus DoOperation<TParser> (TParser parser_ref) where TParser : IParser;

  void ApplyProperties (bool cont, bool skip, bool debug)
  {
    ContinueOnFail = cont || ContinueOnFail;
    SkipOperation = skip || SkipOperation;
    DebugOperation = debug || DebugOperation;
  }
}
