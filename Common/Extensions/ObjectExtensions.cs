//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Diagnostics.CodeAnalysis;

namespace Common.Extensions;

public static class ObjectExtensions
{
  public static bool IsCollection ([NotNullWhen(true)] this object o) => o is IEnumerable;
  public static bool IsCollection<T> ([NotNullWhen(true)] this object o) => o is IEnumerable<T>;
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
  public static Collection<T> AsCollection<T> (this object o)
  {
    if (o is not IEnumerable enumerable)
      return [];

    IEnumerable<T> result = enumerable.OfType<T>();
    return [.. result];
  }

  public static string ToString2 (this object? obj) => obj switch
  {
    null => "<null>",
    string s => s,
    char c => c.ToString(),
    ITextSerializer t => t.Serialize(),
    IConvertible ic => ic.ToString(CIIC),
    ICollection<object> col => col.TextJoin("\n"),
    _ => obj.ToString(),
  } ?? SE;
  public static void ThrowIfNull ([NotNull] this object? obj, string? msg = null)
  {
    if (msg is not null && obj is null)
      throw new InvalidOperationException(msg);
    ANEx.ThrowIfNull(obj);
  }
}
