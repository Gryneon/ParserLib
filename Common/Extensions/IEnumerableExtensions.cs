//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Common.Regexp;

namespace Common.Extensions;

public static class IEnumerableExtensions
{
  /// <summary>Allows indexing for ienumerable types.</summary>
  /// <typeparam name="T">The object type</typeparam>
  /// <param name="list">The list.</param>
  /// <param name="index">The index to retrieve.</param>
  /// <returns>The object at position <paramref name="index"/>.</returns>
  public static T At<T> (this IEnumerable<T> list, int index) => list.ToArray()[index];
  public static Collection<T> ToCollection<T> (this IEnumerable<T> list) => [.. list];
  public static string TextJoin<T> (this IEnumerable<T> list, string separator = EmptyString)
  {
    string result = string.Empty;

    if (list is null)
      return result;

    foreach (T? item in list)
    {
      string? itemString = item?.ToString();
      if (string.IsNullOrEmpty(itemString))
        continue;

      if (result.IsNotEmpty())
        result += separator;
      result += itemString;
    }
    return result;
  }
  public static int LastIndex<T> (this IEnumerable<T> list) => list is null ? -1 : list.Count() - 1;
  public static int LastIndex<T> (this IReadOnlyCollection<T> list) => list is null ? -1 : list.Count - 1;
  public static Collection<T> Condense<T> (this IEnumerable<IEnumerable<T>> listlist) =>
  [.. listlist.Aggregate((list1, list2) => list1.Concat(list2))];
  // IEnumerable
  public static int Count (this IEnumerable list)
  {
    IEnumerator? iter = list?.GetEnumerator();
    int i = 0;
    while (iter?.MoveNext() == true) { i++; }
    return i;
  }
  public static string TextJoin (this IEnumerable list, string separator = EmptyString)
  {
    if (list is null)
      return SE;

    string result = SE;
    foreach (object item in list)
    {
      string? itemString = item?.ToString();
      if (string.IsNullOrEmpty(itemString))
        continue;
      if (result.IsNotEmpty())
        result += separator;
      result += itemString;
    }
    return result;
  }
  public static Collection<string> ToStringCollection (this IEnumerable list) => list.AsCollection<string>();
  public static bool IsEmpty ([NotNullWhen(true)] this IEnumerable? list) => list is null || list.Count() == 0;

  // IEnumerable<string>
  public static RxS AggregateRegex (this IEnumerable<string> list) => list.TextJoin("|");
}
