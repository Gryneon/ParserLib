#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Parser.Inference;
using Parser.Text.Ops;

using static Common.Chars;
using static Parser.DefinitionStaticFunctions;

namespace Specification.IPL;

public static class Definition
{
  /// <summary>
  /// <para>Command Splitter Regex</para>
  /// Build from https://regex101.com/r/WJUiK8/2
  /// </summary>
  public static RxSCollection Splits => [
    Gp($@";?{Etx} {AnyLazy} {Stx}"),
    Gp($@"(?<!{Stx})(?={Cr}|{Fs}{AnyLazy}{Fs}|{Can}|{Esc}|{Etb})"),
    Gp($@"(?:;\s*)?{Stx}"),
    Or(Etx, Nul, ";"),
    Gp(@"<LF>|\n")
  ];
  private static readonly RxS
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
    Fs = Gp($"{FS}|<FS>"),
    Escape = Nm("escape", Esc),
    Shift = Nm("shift", Si),
    Value = Nm("value", @"[-0-9.]+"),
    Letter = Nm("letter", @"[A-Za-z]"),
    TextD3 = Nm("d3", Nm("letter", "d") + Nm("value", "3")),
    LetterF = Nm("letter", "F"),
    TextPart = Nm("text", ".*"),
    CharPart = Nm("char", @"[a-zA-Z]|<[A-Z]{2,3}>"),
    AnyLazy = Rx(@"[\s\S]*?");

  /// <summary>
  /// <para>Command Reader Regex</para>
  /// Built from https://regex101.com/r/dGDxqF/1
  /// </summary>
  public static readonly RxSCollection Regex = [
    $"^{Nm("qty", $"{Us}|{Rs}")}{Value}$",
    $"^{Nm("simple", $"{Can}|{Etb}|{Ack}|{Cr}")}$",
    $"{Nm("adv", Eot)}{CharPart}",
    Nm("field", Escape + LetterF + Gp(Value + "|" + Rx(@"""[^""]"""))),
    TextD3 + "," + TextPart,
    Nm("standard", Escape.Opt + Shift.Opt + Letter + Gp($"{Value}(?:,{Value})*")),
    Nm("standard", Escape.Opt + Shift.Opt + Letter),
    Nm("fieldtext", @".+")
  ];
  public static Regex OpRegex => new(Regex, Spec.RxOpt);

  public static TextSpec Spec => new()
  {
    Name = "ipl",
    Operations = [
      new SplitRegexOperation(Splits, "initial", "splits"),

      new DictionaryOperation(Regex, true, "splits"),
      new GenerateOperation<CommandDataSet>(CommandDataSet.Generate, item => item.Len > 0, "matches", "commands"),
      new IPLCommandOperation("commands", "result"),
      //new ValidateOperation<CommandData>(false)
    ],
    FileInferences = [
      IfN(ExtIs, "ipl"),
      IfN(ExtIs, "pr1"),
      IfN(InferenceType.FileContent|InferenceType.Contains, "<STX>")
      ],
    IgnorePatternWhitespace = true,
    ExplicitCapture = true,
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
