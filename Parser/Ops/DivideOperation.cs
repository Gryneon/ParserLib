#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops;

/// <summary>Divides 2 integers to find the quotient.</summary>
public class DivideOperation : Operation
{
  public string? DivisorKey { get; init; }
  public string? DividendKey { get; init; }
  public string? OutputKey { get; init; }

  public int Divisor { get; set; }
  public int Dividend { get; set; }

  protected override void Execute ()
  {
    if (DivisorKey is not null)
    {
      if (Data[DivisorKey] is not int)
        Status = Err.ThrowBadInput("int", $"{Data[DivisorKey].GetType()}");
      else Divisor = (int) Data[DivisorKey];
    }

    if (Data[DividendKey] is not int)
      Status = Err.ThrowBadInput("int", $"{Data[DividendKey].GetType()}");
    else
      Dividend = (int) Data[DividendKey];

    if (Divisor == 0)
      Status = Err.ThrowBadResult("Cannot divide by zero.");

    int quotient = Dividend / Divisor;
    Data[OutputKey] = quotient;
    Log(MsgClass.Debug, $"{Dividend} / {Divisor} = {quotient}", this);
    Status = Pass;
  }
}
