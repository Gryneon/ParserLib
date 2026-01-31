#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IToken : IIndexSortable, IComparable<IToken>, IEquatable<IToken>
{
  string Type { get; set; }
  bool HasType { get; }
  bool Exempt { get; set; }
  bool Ignored { get; }
  IReadOnlyList<IToken> Children { get; init; }
  string Content => Children.Select(static t => t.Content).TextJoin();
}

public interface ITypeToken
{
  string? ObjType { get; }
  IToken? TypeToken { get; init; }
}

public interface INameToken
{
  string? Name { get; }
  IToken? NameToken { get; init; }
}

public interface IValueToken
{
  string? Value { get; }
  IToken? ValueToken { get; init; }
}
