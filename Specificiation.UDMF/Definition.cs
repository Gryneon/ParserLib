using System.Collections.ObjectModel;
using System.Linq;

using Common;
using Common.Extensions;

using Parser;
using Parser.Inference;
using Parser.Ops.Text;

using static Common.Names;
using static Parser.DefinitionStaticFunctions;

namespace Specification.UDMF;

public static class Definition
{
  public static ISpec Spec { get; } = new Spec()
  {
    Name = "zdoom.udmf",
    FileInferences = [IfN(InferenceType.Ext | InferenceType.Like, "udmf")],
    WhitespaceTokens = ["ws"],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    Operations = [
      new DictionaryOperation(Nm("vertex", @"vertex\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "vertex_matches"),
      new GenerateOperation<ZVertex>(ZVertex.Generate, ZVertex.CanGenerate, "vertex_matches", "vertex"),

      new DictionaryOperation(Nm("thing", @"thing\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "thing_matches"),
      new GenerateOperation<ZThing>(ZThing.Generate, ZThing.CanGenerate, "thing_matches", "thing"),

      new DictionaryOperation(Nm("linedef", @"linedef\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "linedef_matches"),
      new GenerateOperation<ZLineDef>(ZLineDef.Generate, ZLineDef.CanGenerate, "linedef_matches", "linedef"),

      new DictionaryOperation(Nm("sidedef", @"sidedef\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "sidedef_matches"),
      new GenerateOperation<ZSideDef>(ZSideDef.Generate, ZSideDef.CanGenerate, "sidedef_matches", "sidedef"),

      new DictionaryOperation(Nm("sector", @"sector\s*\{(\s*(?'prop'\w+)\s*\=\s*(?'value'\w+);)*\s*\}"), ROML | ROIPW | ROIC | ROEC, false, "text", "sector_matches"),
      new GenerateOperation<ZSector>(ZSector.Generate, ZSector.CanGenerate, "sector_matches", "sector"),
    ]
  };
}

public class ZVertex : IGeneratable<MatchDataSet, ZVertex>
{
  public Collection<IProperty<int>> Properties { get; } = [];
  public int X => Properties.Single(item => item.Key.Like("x")).Value;
  public int Y => Properties.Single(item => item.Key.Like("y")).Value;

  public static ZVertex Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return input.HasGroup("vertex");
  }
}

public class ZThing : IGeneratable<MatchDataSet, ZThing>
{
  public static ZThing Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return input.HasGroup("thing");
  }
}

public class ZLineDef : IGeneratable<MatchDataSet, ZLineDef>
{
  public static ZLineDef Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return input.HasGroup("linedef");
  }
}

public class ZSideDef : IGeneratable<MatchDataSet, ZSideDef>
{
  public static ZSideDef Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return input.HasGroup("sidedef");
  }
}

public class ZSector : IGeneratable<MatchDataSet, ZSector>
{
  public static ZSector Generate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (MatchDataSet input)
  {
    input.ThrowIfNull();
    return input.HasGroup("sector");
  }
}

