namespace Parser;

/// <summary>
/// Predefined keys
/// <list type="bullet">
/// <item><c>initial:</c> The original file data.</item>
/// <item><c>result:</c> The end result.</item>
/// <item><c>text:</c> If the loaded file was text, then this is the text contents.</item>
/// <item><c>bytes:</c> If the loaded file was binary, then this is the raw content.</item>
/// <item><c>file_size:</c> The number of bytes or characters in the file.</item>
/// </list>
/// </summary>
/// <remarks>Any string may be used as a key.</remarks>
public sealed class DataDictionary : IDictionary<string, object>
{
  private const string Area = "DataDictionary";
  private readonly Dictionary<string, object> _dict = [];
  public required XParser Parser { get; init; }
  public bool HasData => Count > 0;
  public ICollection<string> Keys => _dict.Keys;
  public ICollection<object> Values => _dict.Values;
  public int Count => _dict.Count;
  public bool IsReadOnly => false;
  public string LastKeySaved => DataOrder[^1];
  /// <summary>
  /// The order that keys have been saved to. You can get the last key in this collection to get the last stored key.
  /// </summary>
  public Collection<string> DataOrder { get; } = [];

  /// <summary>
  /// Gets or sets data to a given key.
  /// </summary>
  /// <param name="key">The key to assign to or look up.
  /// Prefixing this string with a "+" when assigning will cause it to make a list instead of overrwriting.</param>
  /// <returns>The data assigned to the given key, or <see langword="null"/> if the key is not present.</returns>
  /// <exception cref="ArgumentException"/>
  [NotNull]
  [MaybeNull]
  public object this[string key]
  {
    get => TryLoad(key, out object? value) ? value : null;
    set
    {
      key.ThrowIfNull();
      bool hasKey = _dict.ContainsKey(key);
      if (key.StartsWith('+') && hasKey)
      {
        key = key[1..];
        bool success = DoSave<object>(key, value, DM.MakeList);
        if (!success)
        {
          Log(Area, $"Accessor tried to make list, but failed. Value:{value} & Key:{key} & Current: {_dict[key]}.");
        }
      }
      else
      {
        _ = DoSave<object>(key, value, DM.Overwrite);
      }
    }
  }

  // TODO: Implement These
  private bool DoSave<T> (string key, object data, DM mode)
  {
    if (data is null)
      return false;

    if (data is not T casted)
      return false;

    Collection<T> doMakeList (IEnumerable<T>? list = null) => list is null ? [casted] : [.. list, casted];

    switch (mode)
    {
      case DM.Overwrite:
        _dict[key] = casted;
        goto Success;
      case DM.Ignore:
        if (ContainsKey(key))
          break;
        _dict[key] = casted;
        goto Success;
      case DM.MakeList:
        if (TryLoad(key, out object? value) && value is T existing)
          _dict[key] = new Collection<T>() { existing, casted };
        else if (TryLoad(key, out object? value2) && value2 is IEnumerable<T> existing2)
          _dict[key] = doMakeList(existing2);
        else if (CanLoad(key))
          goto default;
        else if (!CanLoad(key))
          _dict[key] = doMakeList();
        goto Success;
      Success:
        DataOrder.Add(key);
        break;
      default:
        return false;
    }

    return true;
  }
  public void Initialize<T> ([NotNull] T initial)
  {
    initial.ThrowIfNull();
    _ = Save("initial", initial);

    if (initial is string s)
    {
      _ = Save("text", s);
      _ = Save<int>("file_size", s.Length);
    }
    else if (initial is IEnumerable<byte> bytes)
    {
      Collection<byte> list = [.. bytes];
      _ = Save("bytes", list);
      _ = Save<int>("file_size", list.Count);
    }
    else if (initial is IEnumerable list)
    {
      Log("DataDictionary", "Initialization of an unknown list.");
      Collection<object> coll = [.. list.OfType<object>()];
      _ = Save("list", coll);
      _ = Save<int>("list_size", coll.Count);
    }
  }
  public bool IsArray (string key) =>
    ContainsKey(key) && _dict[key] is IEnumerable;
  public bool CanLoad (string key) =>
    _dict.ContainsKey(key) && _dict[key] != null;
  public bool TryLoad ([NotNullWhen(true)] string key, [NotNullWhen(true)][MaybeNullWhen(false)] out object data)
  {
    data = CanLoad(key) ? _dict[key] : null;
    return data is not null;
  }
  public bool TryLoadArray ([NotNullWhen(true)] string key, [NotNullWhen(true)][MaybeNullWhen(false)] out IEnumerable data)
  {
    data = CanLoad<IEnumerable>(key) ? this[key] as IEnumerable : null;
    return data is not null;
  }
  public object Load (string key) => CanLoad(key) ? this[key] : throw new ArgumentException($"Key {key} not found in data dictionary.", nameof(key));
  public bool Save (string key, object data, DM mode = DM.Overwrite) => DoSave<object>(key, data, mode);
  public bool GetLastSavedKey ([NotNullWhen(true)][MaybeNullWhen(false)] out string key_name, [NotNullWhen(true)][MaybeNullWhen(false)] out object key_value)
  {
    key_name = HasData ? LastKeySaved : null;
    key_value = key_name is null ? null : _dict[key_name];
    return key_value is not null;
  }
  public bool IsArray<T> ([NotNullWhen(true)] string key) =>
    ContainsKey(key) && _dict[key] is IEnumerable<T>;
  public bool CanLoad<T> ([NotNullWhen(true)] string key) =>
    CanLoad(key) && _dict[key] is T;
  public bool TryLoad<T> ([NotNullWhen(true)] string key, [NotNullWhen(true)][MaybeNullWhen(false)] out T data) where T : allows ref struct
  {
    data = ContainsKey(key) && _dict[key] is T casted ? casted : default;
    return data is not null;
  }
  public bool TryLoadArray<T> ([NotNullWhen(true)] string key, [NotNullWhen(true)][MaybeNullWhen(false)] out IEnumerable<T> data)
  {
    data = CanLoad<IEnumerable<T>>(key) ? this[key] as IEnumerable<T> : null;
    return data is not null;
  }
  public bool Save<T> ([NotNull] string key, object data, DM mode = DM.Overwrite) => DoSave<T>(key, data, mode);
  public int GetCountOfKey (string key) =>
    !ContainsKey(key) ? 0 :
    _dict[key] is IEnumerable<object> list ? list.Count() :
    1;
  // Standard Keys
  public int FileSize => TryLoad<int>("file_size", out int data) ? data : 0;
  public object Initial => TryLoad("initial", out object? data) ? data : throw new InvalidOperationException();
  public string Text => TryLoad("text", out string? data) ? data : throw new InvalidOperationException();
  public Span<byte> Bytes => TryLoad("bytes", out Span<byte> data) ? data : throw new InvalidOperationException();
  // IDictionary Interface
  public void Add (string key, object? value) => _ = value is null ? Remove(key) : Save(key, value);
  public bool ContainsKey (string key) => CanLoad(key);
  public bool Remove (string key) => _dict.Remove(key);
  public bool TryGetValue (string key, [NotNullWhen(true)] out object? value) => TryLoad(key, out value);
  public void Add (KeyValuePair<string, object> item) => Add(item.Key, item.Value);
  public void Clear () => _dict.Clear();
  public bool Contains (KeyValuePair<string, object> item) => CanLoad(item.Key);
  public bool Remove (KeyValuePair<string, object> item) => Remove(item.Key);
  public IEnumerator<KeyValuePair<string, object>> GetEnumerator () => _dict.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  void ICollection<KeyValuePair<string, object>>.CopyTo (KeyValuePair<string, object>[] array, int arrayIndex) => throw new NotSupportedException();
}
