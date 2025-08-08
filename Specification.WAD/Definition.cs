#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using static Parser.DefinitionStaticFunctions;

using BRM = Parser.Byte.Ops.ByteReadMode;

namespace Specification.WAD;
/// <summary>
/// A static class containing the WAD, WAD2, and PAK specificiations.
/// </summary>
public static class Definition
{
  private static ByteStartAtOperation _st => new(0);
  private static ByteReadOperation _id => new("identification", 4, BRM.Text);
  public static Spec WAD => new()
  {
    Name = "wad",
    FileInferences = [
      IfN(HeadSt, "IWAD"),
      IfN(HeadSt, "PWAD")
    ],
    Operations = [
      _st,
      _id,
      new ByteReadOperation("numlumps", 4, BRM.Value),
      new ByteReadOperation("diroffset", 4),
      new ByteJumpVarOperation("diroffset"),
      new ByteLoadIndexOperationLoop("directory", "numlumps", [
        new ByteReadOperation ("filepos", 4),
        new ByteReadOperation ("size", 4),
        new ByteReadOperation("name", 8, BRM.Text),
        new ByteSavePosOperation("savepos"),
        new ByteJumpVarOperation("filepos"),
        new ByteReadDataVarNameOperation("data", "size"),
        new ByteRecallOperation("savepos"),
      ])
    ]
  };
  public static Spec PAK => new()
  {
    Name = "pack",
    FileInferences = [
      IfN(HeadSt, "PACK")
    ],
    Operations = [
      _st,
      _id,
      new ByteReadOperation ("diroffset", 4),
      new ByteReadOperation ("dirsize", 4),
      new ByteDivideOperation("entrycount", "dirsize", 64),
      new ByteJumpVarOperation("diroffset"),
      new ByteLoadIndexOperationLoop("directory", "entrycount", [
        new ByteReadOperation("name", 50, BRM.Text),
        new ByteReadOperation ("offset", 4),
        new ByteReadOperation ("size", 4),
        new ByteSavePosOperation("savepos"),
        new ByteJumpVarOperation("offset"),
        new ByteReadDataVarNameOperation("data", "size"),
        new ByteRecallOperation("savepos"),
      ])
    ]
  };
  public static Spec WAD2 => new()
  {
    Name = "wad2",
    FileInferences = [
      IfN(HeadSt, "WAD2")
    ],
    Operations = [
      _st,
      _id,
      new ByteReadOperation ("numlumps", 4),
      new ByteReadOperation ("diroffset", 4),
      new ByteJumpVarOperation("diroffset"),
      new ByteLoadIndexOperationLoop("directory", "numlumps", [
        new ByteReadOperation ("filepos", 4),
        new ByteReadOperation ("dsize", 4),
        new ByteReadOperation ("size", 4),
        new ByteReadOperation("type", 1),
        new ByteReadOperation("cmprs", 1),
        new ByteReadOperation("dummy", 2),
        new ByteReadOperation("name", 16, BRM.Text),
        new ByteSavePosOperation("savepos"),
        new ByteJumpVarOperation("filepos"),
        new ByteReadDataVarNameOperation("data", "dsize"),
        new ByteRecallOperation("savepos"),
      ]),
    ]
  };
}
