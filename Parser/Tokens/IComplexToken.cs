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
  IReadOnlyCollection<TPT> PiecesPresent { get; }
  IToken this[TPT piece_type] { get; }
  bool HasPieceType (TPT piece_type);
  void SetPieceType (TPT piece_type, IToken token);
  void AddPieceType (TPT piece_type, IToken token);
  IToken GetPieceToken (TPT piece_type);
  string GetPieceContent (TPT piece_type);
}
