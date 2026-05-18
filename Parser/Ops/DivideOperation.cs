#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops;

/// <summary>Divides 2 integers to find the quotient.</summary>
public class DivideOperation : Operation
{
  [MemberNotNullWhen(true, nameof(DivisorKey))]
  private bool UseVar => DivisorKey is not null;
  private string? DivisorKey { get; init; }
  private string DividendKey { get; init; }
  private new string OutputKey { get; init; }

  private int Divisor { get; set; }
  private int Dividend { get; set; }

  public DivideOperation (int divisor, string dividend_key, string output_key)
  {
    Divisor = divisor;
    DividendKey = dividend_key;
    OutputKey = output_key;
  }

  public DivideOperation (string divisor_key, string dividend_key, string output_key)
  {
    DivisorKey = divisor_key;
    DividendKey = dividend_key;
    OutputKey = output_key;
  }

  protected override void Execute ()
  {
    DebugIn("ByteDivideOperation", "Execute");

    if (UseVar)
    {
      if (Data[DivisorKey] is not int)
        Status = Op.ThrowBadInput("int", $"{Data[DivisorKey].GetType()}");
      else Divisor = (int) Data[DivisorKey];
    }

    if (Data[DividendKey] is not int)
      Status = Op.ThrowBadInput("int", $"{Data[DividendKey].GetType()}");
    else
      Dividend = (int) Data[DividendKey];

    if (Divisor == 0)
      Status = Op.ThrowBadResult("Cannot divide by zero.");

    int quotient = Dividend / Divisor;
    Data[OutputKey] = quotient;
    Log(MsgClass.Debug, $"{Dividend} / {Divisor} = {quotient}");
    Status = Pass;
    DebugOut();
  }
}
