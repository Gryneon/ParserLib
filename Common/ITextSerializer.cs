using System.Diagnostics.CodeAnalysis;

namespace Common;
/// <summary>
/// An interface specifying that this object can be represented by a <see langword="string"/>.
/// </summary>
public interface ITextSerializer
{
  /// <summary>
  /// Creates a lossless representation of the calling object.
  /// </summary>
  /// <returns>A <see langword="string"/> representation of the calling object.</returns>
  string Serialize ();
  /// <summary>
  /// Creates a lossless representation of the calling object.
  /// </summary>
  /// <returns>A <see langword="string"/> representation of the calling object.</returns>
  string? ToString () => Serialize();
}

/// <summary>
/// An object that retains a reference to the object it was created from.
/// </summary>
public interface IRetainOrigin
{
  object Origin { get; }

  /// <summary>
  /// Attempts to save a reference to a given object.
  /// </summary>
  /// <param name="origin">The object to store reference to.</param>
  /// <returns>Returns <see langword="true"/> if the reference was stored, <see langword="false"/> otherwise.</returns>
  [MemberNotNullWhen(true, nameof(Origin))]
  bool TrySaveOrigin ([NotNullWhen(true)] object origin);
}

public interface IRetainOrigin<T> : IRetainOrigin
{
  /// <summary>
  /// Gets a typed reference to the origin object.
  /// </summary>
  /// <returns>A reference to the origin object, casted to <typeparamref name="T"/>.</returns>
  T GetOrigin ();
}
