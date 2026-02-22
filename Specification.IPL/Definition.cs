#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Parser.Inference;
using Parser.Ops;
using Parser.Ops.Text;

using static Common.Chars;
using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;

namespace Specification.IPL;

[DefinitionExport]
public static class Definition
{
  #region Old Op Data
  internal static readonly SplitOperation SplitOp = new(Splits, ROML | ROIPW | ROEC | ROSL, "text", "textparts");
  /// <summary>
  /// <para>Command Splitter Regex</para>
  /// Build from https://regex101.com/r/WJUiK8/2
  /// </summary>
  public static RxSCollection Splits => [
    Rx($@";?{Etx}[\S\s]*?{Stx}"),
    Rx($@"(?<!{Stx})(?=\r|{Cr}|{FS}.*?{FS}|{Can}|{Esc}|{Etb})"),
    $"(^[^><]*{Stx})",
    Etx,
    ";",
    Nul,
    Lf
  ];
  internal static Collection<string> Tokens = [
      "qty",
      "simple",
      "adv",
      "field",
      "d3",
      "standard",
      "fieldtext"];
  private static readonly RxS
    AVal = Gp($",?{Value()}"),
    MVal = AVal.Any,
    Can = Gp($"{CAN}|<CAN>"),
    Etb = Gp($"{ETB}|<ETB>"),
    Eot = Gp($"{EOT}|<EOT>"),
    Esc = Gp($"{ESC}|<ESC>"),
    Ack = Gp($"{ACK}|<ACK>"),
    Etx = Gp($"{ETX}|<ETX>"),
    Stx = Gp($"{STX}|<STX>"),
    Nul = Gp($"{NUL}|<NUL>"),
    //Sub = Gp($"{SUB}|<SUB>"),
    Si = Gp($"{SI}|<SI>"),
    Cr = Gp($"{CR}|<CR>"),
    Us = Gp($"{US}|<US>"),
    Rs = Gp($"{RS}|<RS>"),
    //Fs = Gp($"{FS}|<FS>"),
    Lf = Gp(@"<LF>|\n"),
    Escape = Nm("escape", Esc),
    Shift = Nm("shift", Si),
    CharPart = Or(ALetter, "<[A-Z]{2,3}>"),
    AnyLazy = Gp(@".*?"),
    ALetter = Letter();//,
                       //WS = Gp(@"\s*");

  private static RxS Letter (string c = "[a-zA-Z]") => Nm("t_letter", Gp(c));
  private static RxS Value ([SS("regex")] string c = "[-0-9.]+") => Nm("value", Gp(c));
  private static RxS Cmd (string name, [SS("regex")] string c) => Nm($"m_{name}", $"^{c}$");
  private static RxS Qt ([SS("regex")] string s) => QT + s + QT;

  /// <summary>
  /// <para>Command Reader Regex</para>
  /// Built from https://regex101.com/r/dGDxqF/1
  /// </summary>
  public static readonly RxSCollection Regex = [
    Cmd("qty", Or(Us, Rs) + AVal),
    Cmd("simple", Or(Can, Etb, Ack, Cr)),
    Cmd("adv", Eot + CharPart),
    Cmd("field", Escape + Letter("F") + Or(AVal, Qt(Or("[^\"]+", "\\\"")))),
    Cmd("d3", $"{Letter("d")}{Value("3")},{AnyLazy}"),
    Cmd("standard", Escape.Opt + Shift.Opt + ALetter + MVal),
    Cmd("fieldtext", AnyLazy)
  ];
  public static Regex OpRegex => new(Regex, Spec.RxOpt);
  #endregion
  [DefinitionExport]
  public static Spec Spec => new()
  {
    Name = "ipl",
    Operations = [
      new TokenizeOperation(),
      new DebugToStringOperation("tokens"),
      new DebugWaitForInputOperation(),
      new TokenAssembleOperation()],
    FileInferences = [
      IfN(ExtIs, "ipl"),
      IfN(ExtIs, "pr1"),
      IfN(InferenceType.FileContent|InferenceType.Contains, "<STX>")],
    RxOpt = ROML | ROIPW | ROEC | ROSL,
    IsTextFile = true,
    DefaultRuleSet = ExemptAllWithin,
    TokenType = typeof(ITT),
    TokenRules = [
      new(TokenMatch | IgnoredToken, ITT.None, $"(?<={Etx}).*?(?={Stx})"),
      new(TokenMatch | IgnoredToken, ITT.None, $@"\A.*?(?={Stx})"),
      new(TokenMatch, ITT.Stx, $"{Stx}"),
      new(TokenMatch, ITT.Etx, $"{Etx}"),
      new(TokenMatch, ITT.Esc, $"{Esc}"),
      new(TokenMatch, ITT.Si, $"{Si}"),
      new(TokenMatch, ITT.Sc, $";"),
      new(TokenMatch, ITT.Lf, $"{Lf}"),
      new(TokenMatch, ITT.Etb, $"{Etb}"),
      new(TokenMatch, ITT.Rs, $"{Rs}"),
      new(TokenMatch, ITT.Us, $"{Us}"),
      new(TokenMatch, ITT.Can, $"{Can}"),
      new(TokenMatch, ITT.Fs, $"{FS}|<FS>"),
      new(TokenMatch, ITT.TextProp, Rx(@$"d3,.*?(?=$|;|{Etx})")),
      new(TokenMatch, ITT.Letter, @"(?<=\>|;|<LF>|\n)\b[A-Z]\b(?=[0-9])"),
      new(TokenMatch, ITT.Letter, @"(?<=\>|;|<LF>|\n)\b[A-Z]\b(?=\< | ;|<LF>|\n)"),
      new(TokenMatch, ITT.Value, @"(?<=\b[A-Za-z])[0-9]+(?=,|;|\<)"),
      new(TokenMatch, ITT.Value, @"(?<=,)[0-9]+(?=,|;|\<)"),
    ],
    SC = SCO,
    TokenCompatLookup = new()
    {
      [ITT.Cmd] = [ITT.Etb, ITT.Stx, ITT.Etx, ITT.Can],
      [ITT.Break] = [ITT.Sc, ITT.Lf, ITT.Etx]
    },
    GroupTokenRules = [
      new(ITT.Prop, "n:Letter{o} p:Value, x:Cm p:Value"),
      new(ITT.Prop, "n:Letter{l} p:Value"),
      new(ITT.Prop, "n:Letter{w} p:Value"),
      new(ITT.Fmt, "n:Letter{E|F} p:Value"),
      new(ITT.Line, "n:Letter t:Value pa:(TextProp|Prop)")
    ]
  };
}
