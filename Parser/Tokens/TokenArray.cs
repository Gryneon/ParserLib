#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenArray<T> : TokenBase<T>, IReadOnlyCollection<IToken<T>> where T : notnull
{
  // Tokens Kept
  public required TokenCollection<T> Items { get; init; }
  public override int Count => Items.Count;
  public bool Equals (IReadOnlyCollection<IToken<T>>? other) => other is not null && Items.SequenceEqual(other);
  public override bool Equals (object? obj) => obj switch
  {
    IReadOnlyCollection<IToken<T>> iroc => Equals(iroc),
    IEnumerable<IToken<T>> ie => ie.SequenceEqual(this.AsEnumerable()),
    _ => false
  };
  public override int GetHashCode () => Items.GetHashCode();
  public IEnumerator<IToken<T>> GetEnumerator () => Items.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
