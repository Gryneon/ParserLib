//#pragma warning disable IDE0306 // Simplify collection initialization

namespace Parser.Tokens.Chunk;

[Flags]
public enum GroupNameType
{
  None = 0,

  Token = 1,
  Marker = 2,
  Special = 3,

  Basic = 4,
  List = 8,
  Key = 12,
  Value = 16,

  SplitIntoTokens = 32,
  StoreProperties = 64,
  Ignore = 128,
  Reduce = 256,

}
