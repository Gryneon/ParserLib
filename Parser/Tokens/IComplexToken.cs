#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IFactory
{
  object Produce (IToken input);
  Collection<object> Produce (IEnumerable<IToken> input)
  {
    Collection<object> result = [];

    if (input is null)
      return result;

    foreach (IToken item in input)
      result.Add(Produce(item));

    return result;
  }
}

public interface IComplexToken : IToken, ICloneable
{
  new string Content { get; }
  string IToken.Content => Content;
  IReadOnlyCollection<TokenRef> PiecesPresent { get; }
  IToken this[TokenRef piece_type] { get; }
  bool HasPieceType (TokenRef piece_type);
  void SetPieceType (TokenRef piece_type, IToken token);
  void AddPieceType (TokenRef piece_type, IToken token);
  IToken GetPieceToken (TokenRef piece_type);
  string GetPieceContent (TokenRef piece_type);
}
