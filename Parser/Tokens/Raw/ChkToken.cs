#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class ChkToken<T> : IEquatable<IToken<T>> where T : notnull
{
  public Collection<T> AllowedTypes { get; init; } = [];

  public bool Equals (IToken<T>? other) =>
    other is not null && other.Type is not null && AllowedTypes.Contains(other.Type);
}
