//#pragma warning disable IDE0306 // Simplify collection initialization

using Parser.Tokens.Chunk;

namespace Parser.Tokens;

public static class MatchDataSetExtensions
{
  public static bool IsMarker (this GroupDataSet gds) => gds?.Name.StartsWith(TokenChunk.MarkerID, SCOIC) ?? false;
}
