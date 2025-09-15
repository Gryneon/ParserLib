#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Parser.Text.Ops;
using Parser.Text.Tokens;

using static Common.Chars;
using static Parser.DefinitionStaticFunctions;

namespace Specification.IPL;

public class Definition
{
  /// <summary>
  /// <para>Command Splitter Regex</para>
  /// Build from https://regex101.com/r/WJUiK8/2
  /// </summary>
  public static RxSList Splits => [
    Rx(@";?<ETX> [\s\S]*? <STX>"),
    Rx($@"(?<!<STX>)(?=<CR>|<FS>.*?<FS>|{Can}|{Esc}|{Etb})"),
    Rx(@"(?:;\s*)? <STX>"),
    Rx(@"<ETX>|" + ETX),
    Rx(@";"),
    Rx(@"<NUL>|\0"),
    Rx(@"<LF>|\n")
  ];
  protected static readonly RxS
    Can = Gp($"{CAN}|<CAN>"),
    Etb = $"{ETB}|<ETB>",
    Eot = $"{EOT}|<EOT>",
    Esc = $"{ESC}|<ESC>",
    Ack = $"{ACK}|<ACK>",
    Si = $"{SI}|<SI>",
    Cr = $"{CR}|<CR>",
    Us = $"{US}|<US>",
    Rs = $"{RS}|<RS>",
    Escape = Nm("escape", Gp(Esc)),
    EscapeOpt = Escape + "?",
    Shift = Nm("shift", Gp(Si)),
    ShiftOpt = Shift + "?",
    Value = Nm("value", @"[0-9.]+"),
    Letter = Nm("letter", @"[A-Za-z]"),
    TextD3 = Nm("d3", Nm("letter", "d") + Nm("value", "3")),
    LetterF = Nm("letter", "F"),
    TextPart = Nm("text", ".*"),
    CharPart = Nm("char", @"[a-zA-Z]|<[A-Z]{2,3}>");

  /// <summary>
  /// <para>Command Reader Regex</para>
  /// Built from https://regex101.com/r/dGDxqF/1
  /// </summary>
  public static readonly RxS
    Qty = $"^{Nm("qty", $"{Us}|{Rs}")}{Value}$",
    Simple = $"^{Nm("simple", $"{Can}|{Etb}|{Ack}|{Cr}")}$",
    Adv = $"{Nm("adv", Eot)}{CharPart}",
    Field = Nm("field", Escape + LetterF + Gp(Value + "|" + Rx(@"""[^""]"""))),
    Text = TextD3 + "," + TextPart,
    Default = EscapeOpt + ShiftOpt + Letter + Gp(Value + $"(?:,{Value})*").Opt,
    FieldText = Nm("fieldtext", @".+");
  public static RxSList Regex => [Qty, Adv, Simple, Text, Field, Default, FieldText];
  public static Regex OpRegex => new(Regex, TokenOptions.All);

  public static TextSpec Spec => new()
  {
    Name = "ipl",
    Operations = [
      new SplitRegexOperation(Splits, "initial", "splits"),
      new DictionaryOperation(Regex, true, "splits", "matches"),
      new GenerateOperation<CommandData>(CommandData.Generate, item => true, "matches", "commands"),
      new IPLCommandOperation("commands", "result"),
    ],
    FileInferences = [
      IfN(ExtIs, "ipl"),
      IfN(ExtIs, "pr1"),
      IfN(InferenceType.FileContent, "<STX>")
      ],
    IgnorePatternWhitespace = true,
    TokenLookup = [
      "escape",
      "shift",
      "value",
      "letter",
      "text",
      "fieldtext",
      "char",
    ]
  };
}
