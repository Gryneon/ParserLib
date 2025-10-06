//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

/// <summary>
/// Extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
  public static void AddRange<T> (this Collection<T> list, IEnumerable<T> additions)
  {
    ANEx.ThrowIfNull(list);
    if (additions is null)
      return;
    foreach (T item in additions)
      list.Add(item);
  }
  public static void RemoveCount<T> (this Collection<T> list, int count, int startat = 0)
  {
    ANEx.ThrowIfNull(list);
    for (int i = 0; i <= count; i++)
      list.RemoveAt(startat);
  }
  public static void InsertMany<T> (this Collection<T> list, int index, IEnumerable<T> additions)
  {
    ANEx.ThrowIfNull(list);
    if (additions is null)
      return;
    foreach (T item in additions)
      list.Insert(index, item);
  }
  public static void Replace<T> (this Collection<T> list, int atIndex, IEnumerable<T> replaceWith)
  {
    ANEx.ThrowIfNull(list);
    list.RemoveAt(atIndex);
    list.InsertMany(atIndex, replaceWith);
  }
  public static void RemoveLast<T> (this Collection<T> list)
  {
    ANEx.ThrowIfNull(list);
    list.RemoveAt(list.Count - 1);
  }

  public static void TrimEmpty (this Collection<string> list)
  {
    ANEx.ThrowIfNull(list);
    while (list.Remove(SE)) { }
  }

  public static void AddRange<TKey, TValue> (this KeyedCollection<TKey, TValue> list, IEnumerable<TValue> additions) where TKey : notnull
  {
    ANEx.ThrowIfNull(list);
    if (additions is null)
      return;
    foreach (TValue item in additions)
      list.Add(item);
  }
}
