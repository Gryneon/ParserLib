namespace Parser;
/// <summary>A basic interface for a parser.</summary>
public interface IParser
{
  #region Position
  /// <summary>
  /// The current operation index.
  /// </summary>
  int OpIndex { get; }
  /// <summary>
  /// The next operation index.
  /// </summary>
  int NextOpIndex { get; set; }
  #endregion
  /// <summary>
  /// The stack of cursors. This is for if you have 2 cursors at once or nested loops.
  /// </summary>
  [NotNull] Collection<CursorData> Cursors { get; }
  #region Operation Reference
  /// <summary>
  /// The current operation.
  /// </summary>
  IOperation CurrentOp { get; }
  /// <summary>
  /// The next operation.
  /// </summary>
  IOperation NextOp { get; }
  /// <summary>
  /// All of the operations.
  /// </summary>
  [NotNull] Collection<IOperation>? Operations { get; }
  #endregion
  /// <summary>
  /// The state labels for jump targets.
  /// </summary>
  Dictionary<string, int> Labels { get; }
  #region Result Storage
  /// <summary>
  /// Whether the parser has a result stored or not.
  /// </summary>
  bool HasResult { get; }
  /// <summary>
  /// The result of the parser if it passed.
  /// </summary>
  object? Result { get; }
  #endregion
  /// <summary>
  /// The last operation status.
  /// </summary>
  OpStatus LastStatus { get; }
  /// <summary>
  /// The data that the operations use.
  /// </summary>
  [NotNull] DataDictionary? Data { get; }
  Spec Spec { get; }
  #region Methods
  /// <summary>
  /// Counts the objects in this key.
  /// </summary>
  /// <param name="key">The key to get the count of.</param>
  /// <returns>
  /// -1 if the key does not exist. <br/>
  /// 0 if the key exists but is null, or is a collection of 0. <br/>
  /// 1 if the key is not a collection or is a collection of 1. <br/>
  /// The count if the key is a collection. <br/>
  /// </returns>
  int CountOfKey (string key);
  /// <summary>
  /// Gets the cursor that is operating on the given key.
  /// </summary>
  /// <param name="key">The key to look for.</param>
  /// <returns>The cursor data.</returns>
  CursorData GetCursorByKey (string key);
  /// <summary>
  /// Creates a new cursor on the given key.
  /// </summary>
  /// <param name="key">The key to iterate through.</param>
  void AddCursor (string key);
  OpStatus Parse (string content);
  #endregion
}
