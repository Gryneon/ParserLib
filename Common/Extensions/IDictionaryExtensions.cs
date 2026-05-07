//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class IDictionaryExtensions
{
  extension(IDictionary<string, object> dic)
  {
    public bool ContainsKey (IEnumerable<string> list) =>
      dic is not null && list?.Any(dic.ContainsKey) == true;
  }
  extension<TKey, TValue> (IDictionary<TKey, TValue> dic) where TKey : notnull
  {
    public bool ContainsKey (IEnumerable<TKey> list) =>
      dic is not null && list?.Any(dic.ContainsKey) == true;
    public IDictionary<TKey, TValue> Concat (IDictionary<TKey, TValue> addition, bool overwrite = true)
    {
      Dictionary<TKey, TValue> result = [];

      if (dic is not null)
      {
        foreach (KeyValuePair<TKey, TValue> kvp in dic)
          result[kvp.Key] = kvp.Value;
      }

      if (addition is not null)
      {
        foreach (KeyValuePair<TKey, TValue> kvp in addition)
        {
          if (overwrite || !result.ContainsKey(kvp.Key))
            result[kvp.Key] = kvp.Value;
        }
      }

      return result;
    }
  }
}
