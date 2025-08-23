using System.Collections;

using Parser.Text.Ops;

using static Common.Debug;

namespace Parser.Text;

public sealed class TextDataDictionary : IReadOnlyDictionary<string, object>
{
  /// <summary>
  /// Common keys
  /// <list type="bullet">
  /// <item><c>initial:</c> The original file text as a <see langword="string"/>.</item>
  /// <item><c>text:</c> The working text as a <see langword="string"/></item>
  /// <item><c>matches:</c> The <see cref="Collection{T}"/> of <see cref="MatchData"/> objects that <see cref="DictionaryOperation"/> creates.</item>
  /// <item><c>tokens:</c> The <see cref="Collection{T}"/> of <see cref="IToken"/> objects that <see cref="TokenizeOperation"/> creates.</item>
  /// <item><c>results:</c> The end result.</item>
  /// </list>
  /// </summary>
  /// <remarks>Any string may be used as a key.</remarks>
  internal Dictionary<string, object> Properties = [];
  internal Collection<string> DataOrder = [];
  internal bool HasData => Properties.Count > 0;
  internal string? LastKeySaved;

  public TextDataDictionary (string initial)
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
  public bool Save<T> (string key, object data, DictionaryMode mode = DictionaryMode.Overwrite)
  {
    if (mode is DictionaryMode.Overwrite)
    {
      if (!ContainsKey(key))
        DataOrder.Add(key);
      Properties[key] = data;
      LastKeySaved = key;
      return true;
    }
    else if (mode is DictionaryMode.MakeList)
    {
      if (ContainsKey(key) && Properties[key] is IEnumerable<T> list && data is T typed)
      {
        Properties[key] = list.Append(typed).ToCollection();
        LastKeySaved = key;
        return true;
      }
      else if (ContainsKey(key) && Properties[key] is T first && data is T second)
      {
        Properties[key] = new Collection<T>() { first, second };
        LastKeySaved = key;
        return true;
      }
      else if (!ContainsKey(key) && data is T init)
      {
        DataOrder.Add(key);
        Properties[key] = init;
        LastKeySaved = key;
        return true;
      }
      else
      {
        Log("TextDataDictionary.Save<T>", "Make list selected, but type incorrect.");
        return false;
      }
    }
    else
    {
      if (ContainsKey(key) && Properties[key] is not null)
      {
        Log("TextDataDictionary.Save<T>", "Ignore selected, but key exists and value is not null.");
        return false;
      }
      else if (ContainsKey(key))
      {
        Log("TextDataDictionary.Save<T>", "Ignore selected, and value was null on existing key, added the value.");
        Properties[key] = data;
        LastKeySaved = key;
        return true;
      }
      else
      {
        Log("TextDataDictionary.Save<T>", "Ignore selected, and key was not present, added the value.");
        DataOrder.Add(key);
        Properties[key] = data;
        LastKeySaved = key;
        return true;
      }
    }
  }
  public bool Save (string key, object data, DictionaryMode mode = DictionaryMode.Overwrite)
  {
    if (mode is DictionaryMode.Overwrite)
    {
      if (!ContainsKey(key))
        DataOrder.Add(key);
      Properties[key] = data;
      LastKeySaved = key;
      return true;
    }
    else if (mode is DictionaryMode.MakeList)
    {
      if (ContainsKey(key) && Properties[key] is IEnumerable<object> list)
      {
        Properties[key] = list.Append(data).ToCollection();
        LastKeySaved = key;
        return true;
      }
      else if (ContainsKey(key) && Properties[key] is not null)
      {
        Properties[key] = new Collection<object>() { Properties[key], data };
        LastKeySaved = key;
        return true;
      }
      else
      {
        DataOrder.Add(key);
        Properties[key] = new Collection<object>() { data };
        LastKeySaved = key;
        return true;
      }
    }
    else
    {
      if (ContainsKey(key) && Properties[key] is not null)
      {
        Log("TextDataDictionary.Save", "Ignore selected, but key exists and value is not null.");
        return false;
      }
      else if (ContainsKey(key))
      {
        Log("TextDataDictionary.Save", "Ignore selected, and value was null on existing key, added the value.");
        Properties[key] = data;
        LastKeySaved = key;
        return true;
      }
      else
      {
        Log("TextDataDictionary.Save", "Ignore selected, and key was not present, added the value.");
        DataOrder.Add(key);
        Properties[key] = data;
        LastKeySaved = key;
        return true;
      }
    }
  }
  public int GetCount (string key) =>
    !ContainsKey(key) ? 0 :
    Properties[key] is IEnumerable<object> list ? list.Count() :
    1;

  #region IReadOnlyDictionary<string, object>
  /// <inheritdoc/>
  public int Count => Properties.Count;
  public IEnumerable<string> Keys => Properties.Keys;
  public IEnumerable<object> Values => Properties.Values;

  public object this[string key] => Properties[key];

  public bool ContainsKey (string key) => ((IDictionary<string, object>) Properties).ContainsKey(key);
  public IEnumerator<KeyValuePair<string, object>> GetEnumerator () => Properties.GetEnumerator();
  public bool TryGetValue (string key, [MaybeNullWhen(false)] out object value) => Properties.TryGetValue(key, out value);
  IEnumerator IEnumerable.GetEnumerator () => Properties.GetEnumerator();
  #endregion
}
