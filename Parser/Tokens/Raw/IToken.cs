#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public interface IToken<T> : IIndexSortable, IComparable<IToken<T>> where T : notnull
{
  T? Type { get; }
  [MemberNotNullWhen(true, nameof(Type))]
  bool HasType { get; }
  bool Exempt { get; }
  TokenCollection<T> Children { get; init; }
  virtual string Content => Children.Select(static t => t.Content).TextJoin();
}
