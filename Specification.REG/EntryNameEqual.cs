namespace Specification.REG;

/// <summary>
/// An <see cref="IEqualityComparer{T}"/> comparer to determine if the name of the <see cref="RegProperty"/> is the same as another.
/// </summary>
public sealed class EntryNameEqual () : IEqualityComparer<RegProperty>
{
  /// <inheritdoc/>
  public bool Equals (RegProperty? x, RegProperty? y) =>
    x is null && y is null || x is not null && y is not null && x.Key.Equals(y.Key, SCOIC);
  /// <inheritdoc/>
  public int GetHashCode (RegProperty obj) => obj.Key.GetHashCode(SCOIC);
}
