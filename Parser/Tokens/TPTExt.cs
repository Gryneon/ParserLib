#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public static class TPTExt
{
  private static Dictionary<TPT, bool> TokenCollectionByPieceType { get; } = new()
  {
    [TPT.ValueList] = true,
    [TPT.Value] = false,
    [TPT.Name] = false,
    [TPT.Center] = false,
    [TPT.Left] = false,
    [TPT.Right] = false,
    [TPT.Type] = false,
    [TPT.FlagList] = true,
    [TPT.ParameterList] = true,
    [TPT.PropertyList] = true,
    [TPT.StatementList] = true,
  };

  public static bool IsTokenCollection (this TPT type) => TokenCollectionByPieceType[type];
  public static bool IsUsed (this TPT type, Dictionary<TPT, IToken> token_pieces) =>
    token_pieces is not null && token_pieces.TryGetValue(type, out IToken? value) && (!type.IsTokenCollection() || type.IsTokenCollection() && value is TokenCollection tc && tc.Count > 0);
}
