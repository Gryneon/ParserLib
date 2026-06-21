namespace Common;

/// <summary>An <see cref="IEqualityComparer{T}"/> comparer to determine if the name of the <see cref="IProperty{T}"/> is the same as another.</summary>
public sealed class EntryNameEqual<T> () : IEqualityComparer<IProperty<T>>
{
  public bool Equals (IProperty<T>? x, IProperty<T>? y) =>
    (x is null && y is null) || (x is not null && y is not null && x.Key.Equals(y.Key, SCO));
  public int GetHashCode (IProperty<T> obj) => obj?.Key.GetHashCode(SCO) ?? 0;
}
