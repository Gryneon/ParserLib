namespace Common;
/// <summary>An interface specifying that this object can be represented by a <see langword="string"/>.</summary>
public interface ITextSerializer
{
  /// <summary>Creates a lossless <see cref="string"/> representation of the calling object.</summary>
  /// <returns>A lossless <see cref="string"/> representation of the calling object.</returns>
  string Serialize ();
  /// <summary>Creates a lossless <see cref="string"/> representation of the calling object.</summary>
  /// <returns>A lossless <see cref="string"/> representation of the calling object.</returns>
  string? ToString () => Serialize();
}
