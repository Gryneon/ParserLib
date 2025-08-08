//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class DictionaryExtensions
{
  // Dictionary <TKey, TValue>
  public static void Add<TKey, TValue> (this Dictionary<TKey, TValue> dic, IDictionary<TKey, TValue> other) where TKey : notnull
  {
    foreach (KeyValuePair<TKey, TValue> item in other)
    {
      dic.Add(item);
    }
  }
  public static void Add<TKey, TValue> (this Dictionary<TKey, TValue> dic, (TKey Key, TValue Value) item) where TKey : notnull =>
    dic.Add(item.Key, item.Value);
  public static void Add<TKey, TValue> (this Dictionary<TKey, TValue> dic, KeyValuePair<TKey, TValue> item) where TKey : notnull =>
    dic.Add(item.Key, item.Value);
  public static void Add<TKey, TValue> (this SortedDictionary<TKey, TValue> dic, KeyValuePair<TKey, TValue> item) where TKey : notnull =>
    dic.Add(item.Key, item.Value);
  public static void AddRange<TKey, TValue> (this Dictionary<TKey, TValue> dic, IDictionary<TKey, TValue> items) where TKey : notnull =>
    dic.Add(items);
  public static void AddRange<TKey, TValue> (this Dictionary<TKey, TValue> dic, IEnumerable<KeyValuePair<TKey, TValue>> items) where TKey : notnull
  {
    foreach (KeyValuePair<TKey, TValue> item in items)
    {
      dic.Add(item);
    }
  }
  public static void AddRange<TKey, TValue> (this Dictionary<TKey, TValue> dic, IEnumerable<(TKey Key, TValue Value)> items) where TKey : notnull
  {
    foreach ((TKey Key, TValue Value) item in items)
    {
      dic.Add(item);
    }
  }
}