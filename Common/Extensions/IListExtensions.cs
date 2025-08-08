//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class IListExtensions
{
  public static bool IsEmpty<T> (this IList<T>? list) => list is null || list.Count == 0;
}
