#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenArray : TokenBase, ICollection<IToken>, ITypeToken
{
  // Tokens Kept
  public string? ObjType => TypeToken?.Content;
  public IToken? TypeToken { get; set; }
  public required TokenCollection Items { get; init; }
  public override int Count => Items.Count;

  public bool IsReadOnly => Items.IsReadOnly;

  public bool Equals (IReadOnlyCollection<IToken>? other) => other is not null && Items.SequenceEqual(other);
  public override bool Equals (object? obj) => obj switch
  {
    IReadOnlyCollection<IToken> iroc => Equals(iroc),
    IEnumerable<IToken> ie => ie.SequenceEqual(this.AsEnumerable()),
    _ => false
  };
  public override int GetHashCode () => Items.GetHashCode();
  public IEnumerator<IToken> GetEnumerator () => Items.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public void Add (IToken item) => Items.Add(item);
  public void Clear () => Items.Clear();
  public bool Contains (IToken item) => Items.Contains(item);
  public void CopyTo (IToken[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);
  public bool Remove (IToken item) => Items.Remove(item);
}
