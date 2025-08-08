//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using DM = Common.Extensions.DictionaryMode;

namespace Common.Extensions;

public enum DictionaryMode
{
  Overwrite,
  Ignore,
  MakeList
}

public static class IDictionaryExtensions
{
  public static bool ContainsKey (this IDictionary<string, object> dic, IEnumerable<string> list) =>
    list.Any(dic.ContainsKey);
  public static bool ContainsKey (this IDictionary<string, string> dic, IEnumerable<string> list) =>
    list.Any(dic.ContainsKey);
  public static bool ContainsKey<TKey, TValue> (this IDictionary<TKey, TValue> dic, IEnumerable<TKey> list) =>
    list.Any(dic.ContainsKey);
  public static IDictionary<TKey, TValue> Concat<TKey, TValue> (this IDictionary<TKey, TValue> current, IDictionary<TKey, TValue> addition, bool overwrite = false) where TKey : notnull
  {
    Dictionary<TKey, TValue> result = [];

    foreach (KeyValuePair<TKey, TValue> kvp in current)
      result[kvp.Key] = kvp.Value;
    foreach (KeyValuePair<TKey, TValue> kvp in addition)
      if (overwrite || !result.ContainsKey(kvp.Key))
        result[kvp.Key] = kvp.Value;

    return result;
  }
  public static IDictionary<TKey, object> Concat<TKey> (this IDictionary<TKey, object> current, IDictionary<TKey, object> addition, DM mode = DM.Overwrite) where TKey : notnull
  {
    Dictionary<TKey, object> result = [];

    foreach (KeyValuePair<TKey, object> kvp in current)
      result[kvp.Key] = kvp.Value;
    foreach (KeyValuePair<TKey, object> kvp in addition)
      if (mode is DM.Overwrite || !result.TryGetValue(kvp.Key, out object? value))
        result[kvp.Key] = kvp.Value;
      else if (mode is DM.MakeList)
        result[kvp.Key] = new Collection<object>() { value, kvp.Value };
    return result;
  }
}
