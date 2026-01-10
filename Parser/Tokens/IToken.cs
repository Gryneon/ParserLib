#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IToken : IIndexSortable, IComparable<IToken>
{
  string Type { get; set; }
  [MemberNotNullWhen(true, nameof(Type))]
  bool HasType { get; }
  bool Exempt { get; }
  bool Ignored { get; }
  TokenCollection Children { get; init; }
  virtual string Content => Children.Select(static t => t.Content).TextJoin();
}

public interface IToken<T> : IToken, IIndexSortable, IComparable<IToken<T>> where T : struct
{
  new T Type { get; set; }
  new TokenCollection<T> Children { get; init; }
}
