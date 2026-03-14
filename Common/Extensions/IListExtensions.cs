//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Diagnostics.CodeAnalysis;

namespace Common.Extensions;

public static class IListExtensions
{
  //IList<T>
  /// <summary>Checks if an <see cref="IList{T}"/> is <see langword="null"/> or empty.</summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="list"></param>
  /// <returns></returns>
  public static bool IsEmpty<T> (this IList<T>? list) => list is null || list.Count == 0;
  public static void AddRange<T> ([NotNull] this IList<T> list, IEnumerable<T> additions)
  {
    list ??= [];
    if (additions is null)
      return;
    foreach (T item in additions)
      list.Add(item);
  }
  public static void RemoveCount<T> (this IList<T> list, int count, int startat = 0)
  {
    if (list is null)
      return;

    for (int i = 0; i <= count; i++)
      list.RemoveAt(startat);
  }
  public static void InsertMany<T> ([NotNull] this IList<T> list, int index, IEnumerable<T> additions)
  {
    list ??= [];

    if (additions is null)
      return;

    Action<T> doThing = list.Count <= index ? list.Add : i => list.Insert(index, i);

    foreach (T item in additions)
      doThing.Invoke(item);
  }
  public static void Replace<T> (this IList<T> list, int atIndex, IEnumerable<T> replaceWith)
  {
    if (list is null)
      return;

    list.RemoveAt(atIndex);
    list.InsertMany(atIndex, replaceWith);
  }
  public static void RemoveLast<T> ([NotNull] this IList<T> list)
  {
    list ??= [];
    list.RemoveAt(list.Count - 1);
  }

  public static void TrimEmpty ([NotNull] this IList<string> list)
  {
    list ??= [];
    while (list.Remove(SE)) { }
  }

  // Stack Functions for IList<T>

  /// <summary>Performs a 'pop' action, but discards the popped item.</summary>
  /// <typeparam name="T">The type of item.</typeparam>
  /// <param name="list">The list to perform the action on.</param>
  public static void Drop<T> (this IList<T>? list)
  {
    list?.RemoveAt(list.Count - 1);
  }
  public static T Pop<T> ([NotNull] this IList<T> list)
  {
    list.ThrowIfNull();

    T item = list.Last();
    list.RemoveAt(list.Count - 1);
    return item;
  }
  public static T Peek<T> ([NotNull] this IList<T> list) where T : class
  {
    // Return an empty string if it is peeking at an empty string collection.
    if (typeof(T).Name.Is("String") && list.IsEmpty())
      return (SE as T)!;

    list.ThrowIfNull();
    return list.Last();
  }

  // Queue Functions for IList<T>

  public static void Enqueue<T> ([NotNull] this IList<T> list, T item)
  {
    list.ThrowIfNull();
    list.Add(item);
  }
  public static T Dequeue<T> ([NotNull] this IList<T> list)
  {
    list.ThrowIfNull();

    if (list.IsEmpty())
      throw new ArgumentOutOfRangeException(nameof(list), "Cannot Dequeue, list is empty.");

    T item = list[0];
    list.RemoveAt(0);
    return item;
  }

  public static IList<T?> Nullify<T> ([NotNull] this IList<T> list) where T : struct
  {
    list.ThrowIfNull();
    return list.IsEmpty() ? [] : [.. from item in list let item2 = (T?) item select item2];
  }

  public static IList<T> DeNullify<T> ([NotNull] this IList<T?> list) where T : struct
  {
    list.ThrowIfNull();
    return list.IsEmpty() ? [] : [.. from item in list let item2 = item is null ? default : item.Value select item2];
  }
}
