//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable RCS1263 // extension parameters

namespace Common.Extensions;

public static class ObjectExtensions
{

  extension([NotNull] object? obj)
  {
    public void ThrowIfNull (string? msg = null)
    {
      if (msg is not null && obj is null)
      {
        throw new ANEx(nameof(msg), msg);
      }
      else if (obj is null)
      {
        throw new ANEx(nameof(msg), "No Message Defined");
      }
    }
  }
  /// <summary>Extensions for <see cref="object"/>.</summary>
  /// <param name="obj">The object reference.</param>
  extension(object? obj)
  {
    public string GetTypeName () => obj?.GetType().Name ?? "null";
    /// <summary>Returns the object as a collection.</summary>
    /// <returns>A collection from the object given, or an empty collection if the object cannot be translated.</returns>
    public Collection<object> AsCollection () => obj.AsCollection<object>();
    /// <summary>Returns the object as a collection.</summary>
    /// <typeparam name="T">The type of collection expected.</typeparam>
    /// <returns>A collection from the object given, or an empty collection if the object cannot be translated.</returns>
    public Collection<T> AsCollection<T> ()
    {
      if (obj is not IEnumerable enumerable)
        return [];

      IEnumerable<T> result = enumerable.OfType<T>();
      return [.. result];
    }
    /// <summary>Checks if the object is a type of collection or array.</summary>
    /// <returns><see langword="true"/> if the object is an <see cref="IEnumerable"/>, <see langword="false"/> otherwise.</returns>
    public bool IsCollection () => obj is IEnumerable;
    public bool IsCollection<T> () => obj is IEnumerable<T>;
    /// <summary>An alternatative string output method.</summary>
    /// <returns>A string representation of the object.</returns>
    public string ToString2 () => obj switch
    {
      null => "<null>",
      string s => s,
      char c => $"{c}",
      ITextSerializer t => t.Serialize(),
      IConvertible ic => ic.ToString(CIIC),
      IEnumerable<object> col => col.TextJoin("\n"),
      IReadOnlyProperty<string> prp => $"IProperty \"{prp.Key}\" : \"{prp.Value}\"",
      _ => obj.ToString(),
    } ?? SE;
    public string DecodeAsEnum (Type enumType)
    {
      ANEx.ThrowIfNull(enumType);
      ANEx.ThrowIfNull(obj);
      return !enumType.IsEnum
        ? throw new ArgumentException("Type must be an enum.", nameof(enumType))
        : Enum.GetName(enumType, obj) ?? obj.ToString()!;
    }
  }
  extension(object)
  {
    public static T? ThrowIfFalse<T> (bool condition, string? msg = null)
    {
      if (condition)
        return default;

      if (msg is not null)
        throw new InvalidOperationException(msg);
      throw new InvalidOperationException();
    }
    [DoesNotReturn]
    public static T NotSupported<T> (string? msg = null)
    {
      if (msg is not null)
        throw new NotSupportedException(msg);
      throw new NotSupportedException();
    }
    /// <summary>Does Nothing.</summary>
    public static void DoNothing () { }
  }
  extension(StreamReader? reader)
  {
    /// <summary>Resets the <see cref="StreamReader"/> to the beginning of the stream.</summary>
    public void Reset () => reader?.BaseStream.Position = 0;
  }
  extension(NumberStyles styles)
  {
    /// <summary>Determines if a <see cref="NumberStyles"/> object contains the binary flag.</summary>
    /// <returns><see langword="true"/> if <paramref name="styles"/> contains <see cref="NumberStyles.AllowBinarySpecifier"/>, otherwise <see langword="false"/>.</returns>
    public bool IsBinary () => styles.HasFlag(NumberStyles.AllowBinarySpecifier);
  }
}
