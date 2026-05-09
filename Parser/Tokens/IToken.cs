#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IToken : IIndexSortable, IEquatable<IToken>
{
  Spec Spec { get; }
  /// <summary>Assigned at match, cleared if match fails, kept if passes.</summary>
  TokenRef? AssignTo { get; set; }
  string Type { get; set; }
  bool HasType { get; }
  IToken? Parent { get; set; }
  IList<IToken> Children { get; set; }
  string Content => Children.Select(static t => t.Content).TextJoin(" ");
  string ContentNoNewLine => Content.Replace(["\n", "\r"], ["<LF>", "<CR>"]);
  string ToString (int indent);
  void Print (int indent);
}
