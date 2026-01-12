#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenArray : TokenBase, IReadOnlyCollection<IToken>
{
  // Tokens Kept
  public required TokenCollection Items { get; init; }
  public override int Count => Items.Count;
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
}
