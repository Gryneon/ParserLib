using Parser.Text.Ops;

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
  public static TextSpec Spec => new()
  {
    Name = "reg",
    FileInferences = [IfN(ExtIs, "reg")],
    Operations = [
      new ReplaceRegexOperation("initial", "text", [
        new(@"\;.*$", ""), //Remove line comments
        new(@"^\s+", ""), //Remove beginning ws
        new(@"\s+$", ""), //Remove ending ws
        new(@"\s*\=\s*", "="), //Remove ws around eq sign
        new(@"\\" + RX.LnEnd, "") //Remove escaped newlines
      ]),
      new DictionaryOperation([Sect, Prop, Head]),
      //new GenerateOperation()
      ],
    RegexBasicTokens = ["section", "property", "header"]
  };
}
