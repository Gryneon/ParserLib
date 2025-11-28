#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Diagnostics.CodeAnalysis;

using Parser.Inference;
using Parser.Ops.Text;

using static Common.Chars;
using static Parser.DefinitionStaticFunctions;

namespace Specification.IPL;

[DefinitionExport]
public static class Definition
{
  /// <summary>
  /// <para>Command Splitter Regex</para>
  /// Build from https://regex101.com/r/WJUiK8/2
  /// </summary>
  public static RxSCollection Splits => [
    Rx($@"(;?{Etx}[\S\s]*?{Stx})|((?<!{Stx})(?=\r|{Cr}|{FS}.*?{FS}|{Can}|{Esc}|{Etb}))|(^[^><]*{Stx})|{Etx}|{Nul}|;|{Lf}")
  ];
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

  private static RxS Letter (string c = "[a-zA-Z]") => Nm("letter", Gp(c));
  private static RxS Value ([StringSyntax("regex")] string c = "[-0-9.]+") => Nm("value", Gp(c));
  private static RxS Cmd (string name, [StringSyntax("regex")] string c) => Nm(name, $"^{c}$");
  private static RxS Qt ([StringSyntax("regex")] string s) => QT + s + QT;

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
  [Export("ipl")]
  public static ISpec Spec => new Spec()
  {
    Name = "ipl",
    Operations = [
      new SplitOperation(Splits, "initial", "splits"),
      new DebugToStringOperation("splits"),
      new DictionaryOperation(Regex, ROML | ROIPW | ROEC | ROSL, false, "splits"),
      new GenerateOperation<CommandDataSet>(CommandDataSet.Generate, item => item.Len > 0, "matches", "commands"),
      new IPLCommandOperation("commands", "result"),
      //new ValidateOperation<CommandData>(false)
    ],
    FileInferences = [
      IfN(ExtIs, "ipl"),
      IfN(ExtIs, "pr1"),
      IfN(InferenceType.FileContent|InferenceType.Contains, "<STX>")
      ],
    RxOpt = ROML | ROIPW | ROEC | ROSL,
    RegexBasicTokens = [
      "qty",
      "simple",
      "adv",
      "field",
      "d3",
      "standard",
      "fieldtext",
    ]
  };
}
