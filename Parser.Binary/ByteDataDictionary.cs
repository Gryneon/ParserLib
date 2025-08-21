namespace Parser.Binary;

public class ByteDataDictionary : IDictionary<string, object>
{
  private Dictionary<string, object> _dict = [];

  public ByteDataDictionary () { }
  public ByteDataDictionary (IDictionary<string, object> dic) => _dict = [.. dic];

  public static implicit operator ByteDataDictionary (Dictionary<string, object> other) => [.. other];
  public static explicit operator Dictionary<string, object> (ByteDataDictionary other) => [.. other];

  public object this[string key]
  {
    get => _dict[key];
    set => _dict[key] = value;
  }

  public ICollection<string> Keys => [.. _dict.Keys];

  public int Count => _dict.Count;
  public bool IsValid => Count > 0;
  public string DebugOutput => $"ByteDataDictionary Count: {Count}";

  public static ByteDataDictionary operator + (ByteDataDictionary left, IDictionary<string, object> right) =>
    [.. left.Concat(right, DictionaryMode.MakeList)];

  public void Add (IDictionary<string, object> idict, DictionaryMode mode = DictionaryMode.MakeList) =>
    _dict = [.. _dict.Concat(idict, mode)];
  public void Add (string key, object value, DictionaryMode mode) =>
    _dict = [.. _dict.Concat(new Dictionary<string, object>() { (key, value) }, mode)];
  public void Add (KeyValuePair<string, object> item) =>
    Add(item.Key, item.Value, DictionaryMode.MakeList);
  public void Add (string key, object value) =>
    Add(key, value, DictionaryMode.MakeList);
  public void Clear () => _dict.Clear();
  public bool Contains (KeyValuePair<string, object> item) => _dict.Contains(item);
  public bool ContainsKey (string key) => _dict.ContainsKey(key);
  public IEnumerator<KeyValuePair<string, object>> GetEnumerator () => _dict.GetEnumerator();
  public bool Remove (string key) => _dict.Remove(key);
  public bool Remove (KeyValuePair<string, object> item) => _dict.Remove(item.Key);
  public bool TryGetValue (string key, [MaybeNullWhen(false)] out object value) => _dict.TryGetValue(key, out value);
  #region Explicit Interfaces
  bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  ICollection<object> IDictionary<string, object>.Values => [.. _dict.Values];
  void ICollection<KeyValuePair<string, object>>.CopyTo (KeyValuePair<string, object>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, object>>) _dict).CopyTo(array, arrayIndex);

  #endregion
}
