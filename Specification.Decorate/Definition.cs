#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

using System;

using Common;

using static Parser.DefinitionStaticFunctions;

namespace Specification.Decorate;

//Decorate Prototype
//https://regex101.com/r/YtlFqj/1
//https://regex101.com/r/mTwORe/2

[DefinitionExport]
public static class Definition
{
  private static RxSCollection Reader { get; } = [
    Nm("state_label", Nm("name", @"\w+.*?") + @"\:"),
    Nm("frame_line", Gp(@"\w{4}|"".{4}""") + @"\s+" + Rx(@"\w+")),
    Nm("block_close", @"\}"),
    Nm("states_head", Rx(@"\bstates\s*?" + Nm("block_open", @"\{"))),

  ];

  public const RegexOptions RxOpt = ROML | ROIPW | ROIC | ROEC;
  [Export("zdoom.decorate")]
  public static Spec Spec => new()
  {
    FileInferences = [],
    RxOpt = RxOpt,
    RegexBasicTokens = [],
    WhitespaceTokens = ["ws", "lncomment", "blkcomment"],
    Name = "zdoom.decorate",
    Operations = [
      new SplitOperation(),
      new DictionaryOperation(Reader, RxOpt, false, "textparts"),
      new TokenizeOperation<string>(),
      new TokenTemplateOperation([]),
      //TemplateToObjectOperation
      Operation.End
    ]
  };
}

public class GenericProperty : IProperty<string>
{
  public required string Key { get; set; }
  public string? Value { get; set; }

  public int CompareTo (IProperty<string>? other) => Key.CompareTo(other?.Key, SCO);
  public bool Equals (IProperty<string>? other) => Key.Equals(other?.Key, SCO) && (Value is not null && Value.Equals(other.Value, SCO) || other.Value is null && Value is null);

  public override bool Equals (object? obj) => ReferenceEquals(this, obj) || obj is not null && obj is IProperty<string> iprop && (Value is not null
        ? Key.Equals(iprop.Key, SCO) && Value.Equals(iprop.Value, SCO)
        : Key.Equals(iprop.Key, SCO) && iprop.Value is null);

  public override int GetHashCode () => HashCode.Combine(Key, Value);

  public static bool operator == (GenericProperty left, GenericProperty right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (GenericProperty left, GenericProperty right) => !(left == right);
  public static bool operator < (GenericProperty left, GenericProperty right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (GenericProperty left, GenericProperty right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (GenericProperty left, GenericProperty right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (GenericProperty left, GenericProperty right) => left is null ? right is null : left.CompareTo(right) >= 0;
}

public class NewParser
{
  public int Index { get; set; }
  public string Assembly { get; set; } = SE;
}