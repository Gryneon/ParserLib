namespace Parser.Ops;

/// <summary>
/// The interface for all operations in the parser.
/// </summary>
public interface IOperation
{
  #region Operation Flags
  /// <summary>
  /// Specifies that the operation should not stop the parser upon failure.
  /// </summary>
  bool ContinueOnFail { get; set; }
  /// <summary>
  /// Specifies that the operation should be skipped.
  /// </summary>
  bool SkipOperation { get; set; }
  /// <summary>
  /// Whether or not this operation loads any data.
  /// </summary>
  bool IgnoreAllLoads { get; }
  /// <summary>
  /// Set this in a child operation if the <see cref="DoOperation"/> method should never execute.
  /// </summary>
  virtual bool NeverExecutes => false;
  #endregion
  #region Loop Positions
  /// <summary>
  /// Specifies the break target. This is the position that the loop will go to after it concludes.
  /// </summary>
  int LoopBreak { get; set; }
  /// <summary>
  /// Specifies the beginning of the loop. This is the position it will start the next iteration at.
  /// </summary>
  int LoopStart { get; set; }
  #endregion
  /// <summary>
  /// Calls the operation with the parser provided.
  /// </summary>
  /// <param name="parser_ref">The parser to pull data from and to store data in.</param>
  /// <returns>An <see cref="OpStatus"/> that represents the result status.</returns>
  OpStatus DoOperation (IParser parser_ref);

  IOperation ApplyProperties (bool cont, bool skip)
  {
    ContinueOnFail = cont || ContinueOnFail;
    SkipOperation = skip || SkipOperation;
    return this;
  }
  IOperation SetLoopInfo (int nextloop, int breakloop)
  {
    LoopBreak = breakloop;
    LoopStart = nextloop;
    return this;
  }
}
