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
public sealed class DataStore
{
  private const string Area = "DataStore";
  private readonly Dictionary<string, object> _dict = [];
  public required XParser Parser { get; init; }
  public bool HasData => Count > 0;
  public int Count => _dict.Count;
  public ReadOnlyCollection<string> Keys => [.. _dict.Keys];
  /// <summary>Gets or sets data to a given key.</summary>
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
      if (key.StartsWith('+', SCO) && hasKey)
      {
        key = key[1..];
        bool success = DoSave<object>(key, value, DM.Overwrite | DM.AddToCollection | DM.MakeCollection | DM.MergeCollection);
        if (!success)
        {
          Log(MsgClass.Error, Area, $"Accessor tried to make list, but failed. Value:{value} & Key:{key} & Current: {_dict[key]}.");
        }
      }
      else
      {
        _ = DoSave<object>(key, value, DM.Overwrite);
      }
    }
  }

  public override string ToString ()
  {
    string alldata = "\n";

    foreach (KeyValuePair<string, object> kvp in _dict)
    {
      alldata += $"[{kvp.Key}] = {kvp.Value}\n";
    }

    return alldata;
  }

  // TODO: Implement These
  private bool DoSave<T> (string key, object data, DM mode)
  {
    if (data is null)
      return false;

    if (mode.HasFlag(DM.MergeCollection) && TryLoadArray(key, out IEnumerable<T>? existing_to_merge) && data is IEnumerable<T> new_list)
    {
      Collection<T> big_list = [.. existing_to_merge, .. new_list];
      _dict[key] = big_list;
      return true;
    }
    else if (mode.HasFlag(DM.AddToCollection) && TryLoadArray(key, out IEnumerable<T>? existing_to_add) && data is T new_item)
    {
      Collection<T> big_list = [.. existing_to_add, new_item];
      _dict[key] = big_list;
      return true;
    }
    else if (mode.HasFlag(DM.MakeCollection) && TryLoad(key, out T? lonely_item) && data is T friend_item)
    {
      _dict[key] = new Collection<T>() { lonely_item, friend_item };
      return true;
    }
    else if (mode.HasFlag(DM.Overwrite) || mode == DM.None)
    {
      _dict[key] = data;
      return true;
    }
    else if (mode.HasFlag(DM.Ignore) && !ContainsKey(key))
    {
      _dict[key] = data;
      return true;
    }
    else
    {
      return false;
    }
  }
  public void Initialize<T> ([NotNull] T initial)
  {
    initial.ThrowIfNull();
    Save("initial", initial);

    if (initial is string s)
    {
      Save("text", s);
      Save<int>("file_size", s.Length);
    }
    else if (initial is IEnumerable<byte> bytes)
    {
      Memory<byte> list = bytes.ToArray().AsMemory();
      Save("bytes", list);
      Save<int>("file_size", list.Length);
    }
    else if (initial is IEnumerable list)
    {
      Log("DataStore", "Initialization of an unknown list.");
      Collection<object> coll = [.. list.OfType<object>()];
      Save("list", coll);
      Save<int>("list_size", coll.Count);
    }
  }
  public bool CanLoad ([NotNullWhen(true)] string key) =>
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
  public void Save (string key, object data, DM mode = DM.Overwrite) => DoSave<object>(key, data, mode);
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
  public void Save<T> ([NotNull] string key, object data, DM mode = DM.Overwrite)
  {
    bool saved = DoSave<T>(key, data, mode);

    if (!saved)
      throw new OperationException("DATA NOT SAVED");
  }
  public int GetCountOfKey (string key) =>
    !ContainsKey(key) ? 0 :
    _dict[key] is IEnumerable<object> list ? list.Count() :
    1;
  // IDictionary Interface
  public void Add (string key, object? value)
  {
    if (value is null)
      _ = Remove(key);
    else
      Save(key, value);
  }
  public bool ContainsKey (string key) => CanLoad(key);
  public bool Remove (string key) => _dict.Remove(key);
  public bool TryGetValue (string key, [NotNullWhen(true)] out object? value) => TryLoad(key, out value);
  public IEnumerator<KeyValuePair<string, object>> GetEnumerator () => _dict.GetEnumerator();
}
