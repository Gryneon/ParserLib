namespace Parser.Ops;

public sealed class OperationLabel (string name) : IOperation
{
  public string Name { get; } = name;
  public OpStatus Status { get; } = OpStatus.Pass;
  public bool ContinueOnFail { get; set; }
  public bool SkipOperation { get; set; }
  public bool EndOperation { get; init; }
  public bool StartOperation { get; init; }
  public bool DebugOperation { get; set; }

  /// <summary>
  /// Always passes, does no checks.
  /// </summary>
  /// <typeparam name="TParser">The parser type.</typeparam>
  /// <param name="parser_ref">The parser object.</param>
  /// <returns>Always returns <see cref="OpStatus.Pass"/>.</returns>
  OpStatus IOperation.DoOperation<TParser> (TParser parser_ref) => OpStatus.Pass;
}
