using System.Collections;

using Parser.Text.Ops;

using DM = Common.Extensions.DictionaryMode;

namespace Parser.Text;

public sealed class TextDataDictionary : IDictionary<string, object>
{
  /// <summary>
  /// Common keys
  /// <list type="bullet">
  /// <item><c>initial:</c> The original file text as a <see langword="string"/>.</item>
  /// <item><c>text:</c> The working text as a <see langword="string"/></item>
  /// <item><c>matches:</c> The <see cref="Collection{T}"/> of <see cref="MatchDataSet"/> objects that <see cref="DictionaryOperation"/> creates.</item>
  /// <item><c>tokens:</c> The <see cref="Collection{T}"/> of <see cref="IToken"/> objects that <see cref="TokenizeOperation"/> creates.</item>
  /// <item><c>result:</c> The end result.</item>
  /// </list>
  /// </summary>
  /// <remarks>Any string may be used as a key.</remarks>
  internal Dictionary<string, object> Properties = [];
  /// <summary>
  /// The order that keys have been created. You can get the last key in this collection to get the last stored key.
  /// </summary>
  internal Collection<string> DataOrder = [];
  internal bool HasData => Properties.Count > 0;
  internal string? LastKeySaved;

  public TextDataDictionary (string? initial)
  {
    if (initial is not null)
      Initialize(initial);
  }
  public TextDataDictionary () { }
  public void Initialize ([NotNull] string initial)
  {
    _ = Save<string>("initial", initial);
    _ = Save<string>("text", initial);
    _ = Save<int>("file_size", initial.Length);
  }
  public bool CanLoad<T> (string key) =>
    CanLoad(key) &&
    Properties[key] is T;
  public bool CanLoad (string key) =>
    ContainsKey(key) &&
    Properties[key] != null;
  public bool IsArray (string key) =>
    ContainsKey(key) &&
    Properties[key] is IEnumerable<object>;
  public bool IsArray<T> (string key) =>
    ContainsKey(key) &&
    Properties[key] is IEnumerable<T>;
  public bool TryLoad<T> (string key, [NotNullWhen(true)][MaybeNullWhen(false)] out T data)
  {
    bool result = ContainsKey(key) && Properties[key] is T;
    data = result ? (T) Properties[key] : default;
    return result;
  }
  public bool TryLoad (string key, [NotNullWhen(true)][MaybeNullWhen(false)] out object data)
  {
    bool result = ContainsKey(key);
    data = result ? Properties[key] : default;
    return result;
  }
  public bool TryLoadArray<T> (string key, [NotNullWhen(true)][MaybeNullWhen(false)] out IEnumerable<T> data)
  {
    bool result = ContainsKey(key) && Properties[key] is IEnumerable<T>;
    data = result ? (IEnumerable<T>) Properties[key] : default;
    return result;
  }
  private bool DoSave<T> (string key, object data, DM mode)
  {
    if (data is null)
      return false;

    if (data is not T casted)
      return false;

    bool result = false;

    Collection<T> doMakeList (IEnumerable<T>? list = null) => list is null ? [casted] : [.. list, casted];

    switch (mode)
    {
      case DM.Overwrite:
        Properties[key] = casted;
        goto Success;
      case DM.Ignore:
        if (Properties.ContainsKey(key))
          break;
        Properties[key] = casted;
        goto Success;
      case DM.MakeList:
        if (Properties.TryGetValue(key, out object? value) && value is T existing)
          Properties[key] = new Collection<T>() { existing, casted };
        else if (Properties.TryGetValue(key, out object? value2) && value2 is IEnumerable<T> existing2)
          Properties[key] = doMakeList(existing2);
        else if (Properties.TryGetValue(key, out object? _))
          goto default;
        else if (!Properties.TryGetValue(key, out object? _))
          Properties[key] = doMakeList();
        goto Success;
      Success:
        result = true;
        LastKeySaved = key;
        DataOrder.Add(key);
        break;
      default:
        result = false;
        break;
    }

    return result;
  }
  public bool Save<T> (string key, object data, DM mode = DM.Overwrite) => DoSave<T>(key, data, mode);
  public bool Save (string key, object data, DM mode = DM.Overwrite) => DoSave<object>(key, data, mode);
  public int GetCountOfKey (string key) =>
    !ContainsKey(key) ? 0 :
    Properties[key] is IEnumerable<object> list ? list.Count() :
    1;
  public bool GetLastSavedKey ([NotNullWhen(true)][MaybeNullWhen(false)] out string key_name, [NotNullWhen(true)][MaybeNullWhen(false)] out object key_value)
  {
    key_value = null;
    key_name = null;
    if (DataOrder.Count > 0)
    {
      key_name = DataOrder[^1];
      key_value = Properties[key_name];
      return true;
    }
    else
      return false;
  }
  #region IDictionary<string, object>
  /// <inheritdoc/>
  public int Count => Properties.Count;

  public ICollection<string> Keys => [.. Properties.Keys];
  public ICollection<object> Values => [.. Properties.Values];
  bool ICollection<KeyValuePair<string, object>>.IsReadOnly { get; }

  public object this[string key]
  {
    get => TryLoad(key, out object? result) ? result : throw new AbsentGroupException();
    set
    {
      if (value is not null)
        _ = Save(key, value);
      else
        return;
    }
  }

  public bool ContainsKey (string key) => Properties.ContainsKey(key);
  public IEnumerator<KeyValuePair<string, object>> GetEnumerator () => Properties.GetEnumerator();
  public bool TryGetValue (string key, [MaybeNullWhen(false)][NotNullWhen(true)] out object? value) => TryLoad(key, out value);
  IEnumerator IEnumerable.GetEnumerator () => Properties.GetEnumerator();
  public bool Remove (string key) => Properties.Remove(key);
  void ICollection<KeyValuePair<string, object>>.Add (KeyValuePair<string, object> item) => throw new NotImplementedException();
  public void Clear () => Properties.Clear();
  public bool Contains (KeyValuePair<string, object> item) => Properties.ContainsKey(item.Key) && Properties[item.Key] == item.Value;
  void ICollection<KeyValuePair<string, object>>.CopyTo (KeyValuePair<string, object>[] array, int arrayIndex) => throw new NotImplementedException();
  bool ICollection<KeyValuePair<string, object>>.Remove (KeyValuePair<string, object> item) => throw new NotImplementedException();
  public void Add (string key, object value) => Save(key, value);
  #endregion
}
