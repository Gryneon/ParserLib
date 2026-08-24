namespace Common;

/// <summary>An <see cref="IEqualityComparer{T}"/> comparer to determine if the name of the <see cref="IProperty{T}"/> is the same as another.</summary>
public sealed class EntryNameEqual<T> () : IEqualityComparer<IProperty<T>>, IEqualityComparer<KeyValuePair<string, T>>
{
  public bool Equals (IProperty<T>? x, IProperty<T>? y) =>
    (x is null && y is null) || (x?.Key.Equals(y?.Key, SCO) ?? false);
  public bool Equals (KeyValuePair<string, T> x, KeyValuePair<string, T> y) => x.Key.Equals(y.Key, SCO);
  public int GetHashCode (IProperty<T> obj) => obj?.Key.GetHashCode(SCO) ?? 0;
  public int GetHashCode (KeyValuePair<string, T> obj) => obj.Key.GetHashCode(SCO);
}
