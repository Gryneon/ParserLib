//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Diagnostics.CodeAnalysis;

namespace Common.Extensions;

public static class ObjectExtensions
{
  public static bool IsCollection ([NotNullWhen(true)] this object o) => o is IEnumerable;
  public static bool IsCollection<T> ([NotNullWhen(true)] this object o) => o is IEnumerable<T>;
  /// <summary>Returns the object as a collection.</summary>
  /// <param name="o">The object to convert.</param>
  /// <returns>A collection from the object given, or an empty collection if the object cannot be translated.</returns>
  public static Collection<object> AsCollection (this object o) => o.AsCollection<object>();
  /// <summary>Returns the object as a collection.</summary>
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
    IEnumerable<object> col => col.TextJoin("\n"),
    IReadOnlyProperty<string> prp => $"IProperty \"{prp.Key}\" : \"{prp.Value}\"",
    _ => obj.ToString(),
  } ?? SE;
  public static void ThrowIfNull ([NotNull] this object? obj, string? msg = null)
  {
    if (msg is not null && obj is null)
      throw new InvalidOperationException(msg);
    else
      throw new InvalidOperationException("ThrowIfNull threw the null.");
  }
  public static T? ThrowIfFalse<T> (this object? _, bool condition, string? msg = null)
  {
    if (condition)
      return default;

    if (msg is not null)
      throw new InvalidOperationException(msg);
    throw new InvalidOperationException();
  }
  [DoesNotReturn]
  public static T NotSupported<T> ([NotNull] this object? _, string? msg = null)
  {
    if (msg is not null)
      throw new NotSupportedException(msg);
    throw new NotSupportedException();
  }
  [DoesNotReturn]
  public static T NotImplemented<T> ([NotNull] this object? _, string? msg = null)
  {
    if (msg is not null)
      throw new NotImplementedException(msg);
    throw new NotImplementedException();
  }
  public static void DoNothing (this object? _) { }

  public static string DecodeAsEnum (this object value, Type enumType)
  {
    ANEx.ThrowIfNull(enumType);
    ANEx.ThrowIfNull(value);
    return !enumType.IsEnum
      ? throw new ArgumentException("Type must be an enum.", nameof(enumType))
      : Enum.GetName(enumType, value) ?? value.ToString()!;
  }
}
