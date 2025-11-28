#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Parser.Ops.Binary;

using static Parser.DefinitionStaticFunctions;
using static Parser.Ops.Binary.ByteReadOperation;

namespace Specification.WAD;
/// <summary>
/// A static class containing the WAD, WAD2, and PAK specificiations.
/// </summary>
[DefinitionExport(true)]
public static class Definition
{
  [Export("wad")]
  public static Spec WAD => new()
  {
    Name = "wad",
    FileInferences = [
      IfNAnd([IfN(HeadSt, "IWAD"), IfN(ExtIs, "wad") ]),
      IfN(HeadSt, "PWAD")
    ],
    Operations = [
      Operation.CreateCursor("bytes", 0),
      ReadString("identification", 4),
      ReadInt("numlumps"),
      ReadInt("diroffset"),
      new ByteJumpVarOperation("diroffset"),
      Operation.ForCount([
        ReadInt("filepos"),
        ReadInt("size"),
        ReadString("name", 8),
        new ByteSavePosOperation("savepos"),
        new ByteJumpVarOperation("filepos"),
        ReadBinary("size", "data"),
        new ByteRecallOperation("savepos"),
      ], "numlumps")
    ]
  };
  [Export("pack")]
  public static Spec PAK => new()
  {
    Name = "pack",
    FileInferences = [
      IfN(HeadSt, "PACK")
    ],
    Operations = [
      Operation.CreateCursor("bytes", 0),
      ReadString("identification", 4),
      ReadInt("diroffset"),
      ReadInt("dirsize"),
      new ByteDivideOperation(64, "dirsize", "entrycount"),
      new ByteJumpVarOperation("diroffset"),
      Operation.ForCount( [
        ReadString("name", 50),
        ReadInt("offset"),
        ReadInt("size"),
        new ByteSavePosOperation("savepos"),
        new ByteJumpVarOperation("offset"),
        ReadBinary("size", "data"),
        new ByteRecallOperation("savepos"),
      ], "entrycount")
    ]
  };
  [Export("wad2")]
  public static Spec WAD2 => new()
  {
    Name = "wad2",
    FileInferences = [
      IfN(HeadSt, "WAD2")
    ],
    Operations = [
      Operation.CreateCursor("bytes", 0),
      ReadString("identification", 4),
      ReadInt("numlumps"),
      ReadInt("diroffset"),
      new ByteJumpVarOperation("diroffset"),
      Operation.ForCount([
        ReadInt("filepos"),
        ReadInt("dsize"),
        ReadInt("size"),
        ReadByte("type"),
        ReadByte("cmprs"),
        ReadShort("dummy"),
        ReadString("name", 16),
        new ByteSavePosOperation("savepos"),
        new ByteJumpVarOperation("filepos"),
        ReadBinary("dsize", "data"),
        new ByteRecallOperation("savepos"),
      ], "numlumps"),
    ]
  };
}
