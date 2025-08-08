//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class CollectionExtensions
{
  public static void AddRange<T> (this Collection<T> list, IEnumerable<T> additions)
  {
    foreach (T item in additions)
      list.Add(item);
  }
  public static void RemoveCount<T> (this Collection<T> list, int count, int startat = 0)
  {
    for (int i = 0; i <= count; i++)
      list.RemoveAt(startat);
  }
  public static void RemoveLast<T> (this Collection<T> list) => list.RemoveAt(list.Count - 1);

  public static void TrimEmpty (this Collection<string> list)
  {
    while (list.Remove(SE)) { }
  }

  public static void AddRange<TKey, TValue> (this KeyedCollection<TKey, TValue> list, IEnumerable<TValue> additions) where TKey : notnull
  {
    foreach (TValue item in additions)
      list.Add(item);
  }
}
