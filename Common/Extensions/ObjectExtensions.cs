//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class ObjectExtensions
{
  public static bool IsCollection (this object o) => o.IsCollection<object>();
  public static bool IsCollection<T> (this object o) => o is IEnumerable<T>;
  /// <summary>
  /// Returns the object as a collection.
  /// </summary>
  /// <param name="o">The object to convert.</param>
  /// <returns>A collection from the object given, or an empty collection if the object cannot be translated.</returns>
  public static Collection<object> AsCollection (this object o) => o.AsCollection<object>();
  /// <summary>
  /// Returns the object as a collection.
  /// </summary>
  /// <typeparam name="T">The type of collection expected.</typeparam>
  /// <param name="o">The object to convert.</param>
  /// <returns>A collection from the object given, or an empty collection if the object cannot be translated.</returns>
  public static Collection<T> AsCollection<T> (this object o) => [.. (o as IEnumerable<T>) ?? []];
  public static string? ToString (this object obj) => obj switch
  {
    null => "null",
    string s => s,
    ITextSerializer t => t.Serialize(),
    _ => obj.ToString(),
  };
}
