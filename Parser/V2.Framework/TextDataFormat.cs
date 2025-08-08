
using System.Text.RegularExpressions;

using Common.Regex;

using OT = Parser.V2.Framework.OperationType;
using RO = System.Text.RegularExpressions.RegexOptions;

namespace Parser.V2.Framework;

public class TextDataFormat (string name) : DataFormat(name)
{
  public override DataFormatType Type => DataFormatType.Text;

  public RxSList Replacer { get; set; }
  public RxSList Splitter { get; set; }
  public RxSList Matcher { get; set; }

  public RO RegexOptions { get; protected set; }

  public bool IgnoreCase
  {
    get => RegexOptions.HasFlag(RO.IgnoreCase);
    set => RegexOptions |= RO.IgnoreCase;
  }
  public bool IgnorePatternWhitespace
  {
    get => RegexOptions.HasFlag(RO.IgnorePatternWhitespace);
    set => RegexOptions |= RO.IgnorePatternWhitespace;
  }
  public bool RightToLeft
  {
    get => RegexOptions.HasFlag(RO.RightToLeft);
    set => RegexOptions |= RO.RightToLeft;
  }
  public bool ExplicitCapture
  {
    get => RegexOptions.HasFlag(RO.ExplicitCapture);
    set => RegexOptions |= RO.ExplicitCapture;
  }
  public bool Multiline
  {
    get => RegexOptions.HasFlag(RO.Multiline);
    set => RegexOptions |= RO.Multiline;
  }
}

public class DecorateFile
{

}

public class MapInfoFile
{

}

public class MapInfoGameInfoObj
{

}

public class MapInfoSkillObj
{

}

public class MapInfoIntermissionObj
{

}

public static class Definition
{
  public static TextDataFormat DecorateFormat { get; } = new("zdoom.decorate")
  {
    ExpectedResult = typeof(DecorateFile),
    IgnoreCase = true,
    ExplicitCapture = true,
    Operations = [
      new ReplaceOperation("",""),
      ]
  };
  public static TextDataFormat MapInfoFormat { get; } = new("zdoom.mapinfo")
  {
    ExpectedResult = typeof(DecorateFile),
    IgnoreCase = true,
    ExplicitCapture = true,
    Operations = [
      new ReplaceOperation("","") { OpType = OT.TransformData|OT.DebugOnly },
      ]
  };
}