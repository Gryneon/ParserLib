//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class IListExtensions
{
  //IList<T>
  /// <summary>Checks if an <see cref="IList{T}"/> is <see langword="null"/> or empty.</summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="list"></param>
  /// <returns></returns>
  public static bool IsEmpty<T> (this IList<T>? list) => list is null || list.Count == 0;
  public static void AddRange<T> (this IList<T> list, IEnumerable<T> additions)
  {
    list ??= [];
    if (additions is null)
      return;
    foreach (T item in additions)
      list.Add(item);
  }
  public static void RemoveCount<T> (this IList<T> list, int count, int startat = 0)
  {
    ANEx.ThrowIfNull(list);
    for (int i = 0; i <= count; i++)
      list.RemoveAt(startat);
  }
  public static void InsertMany<T> (this IList<T> list, int index, IEnumerable<T> additions)
  {
    ANEx.ThrowIfNull(list);
    if (additions is null)
      return;
    foreach (T item in additions)
      list.Insert(index, item);
  }
  public static void Replace<T> (this IList<T> list, int atIndex, IEnumerable<T> replaceWith)
  {
    ANEx.ThrowIfNull(list);
    list.RemoveAt(atIndex);
    list.InsertMany(atIndex, replaceWith);
  }
  public static void RemoveLast<T> (this IList<T> list)
  {
    ANEx.ThrowIfNull(list);
    list.RemoveAt(list.Count - 1);
  }

  public static void TrimEmpty (this IList<string> list)
  {
    ANEx.ThrowIfNull(list);
    while (list.Remove(SE)) { }
  }

  // Stack Functions for IList<T>

  /// <summary>Performs a 'pop' action, but discards the popped item.</summary>
  /// <typeparam name="T">The type of item.</typeparam>
  /// <param name="list">The list to perform the action on.</param>
  public static void Drop<T> (this IList<T> list)
  {
    list.ThrowIfNull();
    list.RemoveAt(list.Count - 1);
  }
  public static T Pop<T> (this IList<T> list)
  {
    list.ThrowIfNull();

    T item = list.Last();
    list.RemoveAt(list.Count - 1);
    return item;
  }
  public static T Peek<T> (this IList<T> list)
  {
    list.ThrowIfNull();
    return list.Last();
  }

  // Queue Functions for IList<T>

  public static void Enqueue<T> (this IList<T> list, T item)
  {
    list.ThrowIfNull();
    list.Add(item);
  }
  public static T Dequeue<T> (this IList<T> list)
  {
    list.ThrowIfNull();

    if (list.IsEmpty())
      throw new ArgumentOutOfRangeException(nameof(list), "Cannot Dequeue, list is empty.");

    T item = list[0];
    list.RemoveAt(0);
    return item;
  }
}
