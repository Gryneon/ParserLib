#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public interface IToken<T> : IIndexSortable, IComparable<IToken<T>>
{
  T? Type { get; }
  [MemberNotNullWhen(true, nameof(Type))]
  bool HasType { get; }
  IList<IToken<T>> Children { get; init; }
  virtual string Content => Children.TextJoin();
}
