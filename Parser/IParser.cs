using Common.Extensions;

namespace Parser;

public interface IParser
{
  int OpIndex { get; }
  int NextOpIndex { get; set; }
  int OpCount { get; }
  int Cursor { get; set; }
  string? CursorKey { get; set; }
  IOperation CurrentOp => Operations![OpIndex];
  IOperation NextOp => Operations![NextOpIndex];
  [NotNull] Collection<IOperation>? Operations { get; }
  bool HasResult => Result is not null;
  object? Result { get; }
  OpStatus LastStatus { get; }
  IDictionary<string, object> Work { get; }
  Spec Spec { get; }
  DictionaryMode Mode { get; set; }
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
}
