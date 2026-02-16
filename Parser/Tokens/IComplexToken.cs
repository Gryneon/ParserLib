#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IComplexToken : IToken
{
  new string Content { get; }
  IToken? NameToken { get; set; }
  IToken? ValueToken { get; set; }
  IToken? TypeToken { get; set; }
  string IToken.Content => Content;
  IReadOnlyCollection<TPT> PiecesPresent { get; }
  IToken this[TPT piece_type] { get; }
  bool HasPieceType (TPT piece_type);
  void SetPieceType (TPT piece_type, IToken token);
  void AddPieceType (TPT piece_type, IToken token);
  IToken GetPieceToken (TPT piece_type);
  string GetPieceContent (TPT piece_type);
}
