using Parser;
using Parser.Ops.Text;

using static Parser.DefinitionStaticFunctions;

namespace Specification.REG;

/// <summary>
/// Defines the Registry spec.
/// </summary>
public static class Definition
{
  private static readonly RxS
    Head = Nm("header", Header.DefaultRx),
    Remkey = Nm("remkey", @"\-"),
    Remval = Nm("remval", @"\-"),
    Sectchars = Rx(@"[ \w\\*-]"),
    Sect = @"\[" + Remkey.Opt + Nm("section", Sectchars) + @"\]",
    Def = Nm("default", @"\@").Opt,
    Strprop = Nm("key", RX.CString),
    Strval = Nm("value", RX.CString),
    Hexval = Nm("type", Or(Nm("hex"), Nm("qword"), Nm("dword"))) + Gp(@"\(" + Nm("hsize", @"\d+") + @"\)").Opt + @"\:" + Nm("value", @"[0-9a-f, ]+"),
    Prop = Nm("property", Or(Def, Strprop) + @"\=" + Or(Remval, Strval, Hexval));
  /// <summary>
  /// Defines the Registry spec.
  /// </summary>
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
      ], "initial", "text"),
      new DictionaryOperation([Sect, Prop, Head]),
      //new GenerateOperation()
      ],
    RegexBasicTokens = ["section", "property", "header"]
  };
}
