#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IToken : IIndexSortable, IComparable<IToken>, IEquatable<IToken>
{
  /// <summary>Assigned at match, cleared if match fails, kept if passes.</summary>
  TokenRef? AssignTo { get; set; }
  string Type { get; set; }
  bool HasType { get; }
  IList<IToken> Children { get; set; }
  string Content => Children.Select(static t => t.Content).TextJoin(" ");
  string ContentNoNewLine { get; }
  string ToString (int indent);
  void Print (int indent);
}
