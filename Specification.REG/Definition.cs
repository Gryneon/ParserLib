using Parser;
using Parser.Ops.Text;

using static Parser.DefinitionStaticFunctions;

using RTT = Specification.REG.RegTokenType;

namespace Specification.REG;

[Flags]
public enum RegTokenType
{
  None = 0,
  Head = 1,
  KeyPart = 2,
  AddVal = 3,
  SubVal = 4,
  AddDefVal = 5,
  SubDefVal = 6,
  DWordHex = 7,
  QWordHex = 8,
  HexPair = 9,
  HexOne = 10,
  AInt = 11,
  Co = 14, // ':'
  Eq = 15, // '='
  Sl = 16, // '/'
  Cm = 17, // ','
  Bo = 18, // '['
  Bc = 19, // ']'
  Po = 20, // '('
  Pc = 21, // ')'
  At = 22, // '@'
  Mn = 23, // '-'
  TypeName = 24, // dword, hex, qword
  Key = 25,
  AddKey = 27,
  RemKey = 28,
  AddProp = 29,
  RemProp = 30,

};

/// <summary>Defines the registry spec.</summary>
[DefinitionExport]
public static class Definition
{
  //private static readonly RxS
  //Head = Nm("header", Header.DefaultRx),
  //Remkey = Nm("remkey", @"\-"),
  //Remval = Nm("remval", @"\-"),
  //Sectchars = Rx(@"[ \w\\*-]"),
  //Sect = @"\[" + Remkey.Opt + Nm("section", Sectchars) + @"\]",
  //Def = Nm("default", @"\@").Opt,
  //Strprop = Nm("key", RX.CString),
  //Strval = Nm("value", RX.CString),
  //Hexval = Nm("type", Or(Nm("hex"), Nm("qword"), Nm("dword"))) + Gp(@"\(" + Nm("hsize", @"\d+") + @"\)").Opt + @"\:" + Nm("value", @"[0-9a-f, ]+"),
  //Prop = Nm("property", Or(Def, Strprop) + @"\=" + Or(Remval, Strval, Hexval));
  /// <summary>Defines the Registry spec.</summary>
  [Export("reg")]
  public static Spec Spec => new()
  {
    Name = "reg",
    FileInferences = [IfN(ExtIs, "reg")],
    Operations = [
      new ReplaceOperation([
        (@"\;.*$", ""), //Remove line comments
        (@"^\s+", ""), //Remove beginning ws
        (@"\s+$", ""), //Remove ending ws
        (@"\s*\=\s*", "="), //Remove ws around eq sign
        (@"\\" + RX.LnEnd, "") //Remove escaped newlines
      ], "text", "text"),
      new TokenizeOperation<RTT>("text", "tokens"),
      new TokenAssembleOperation<RTT>("tokens", "tokens_assembled")
      ],
    TokenRules = [
      new(RT.TokenExact, RTT.Eq, "=")
    ],
    RegexBasicTokens = ["section", "property", "header"],
    IsTextFile = true,
    RxOpt = ROIPW | ROEC | ROML,
    SC = SCOIC,
  };
}
