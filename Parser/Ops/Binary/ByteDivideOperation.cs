#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

/// <summary>Divides 2 integers to find the quotient.</summary>
public class ByteDivideOperation : Operation
{
  [MemberNotNullWhen(true, nameof(_divisor_key))]
  private bool UseVar => _divisor_key is not null;
  private readonly string? _divisor_key;

  private int _divisor, _dividend;

  public ByteDivideOperation (int divisor, string dividend_key, string output_key) : base(dividend_key, output_key) => _divisor = divisor;
  public ByteDivideOperation (string divisor_key, string dividend_key, string output_key) : base([divisor_key, dividend_key], output_key) => _divisor_key = divisor_key;

  protected override void Execute ()
  {
    DebugIn("ByteDivideOperation", "Execute");
    if (UseVar)
    {
      if (MultipleInputValues[0] is not int divisor)
        Status = Op.ThrowBadInput("int", $"{MultipleInputValues[0].GetType()}");
      else
        _divisor = divisor;

      if (MultipleInputValues[1] is not int dividend)
        Status = Op.ThrowBadInput("int", $"{MultipleInputValues[1].GetType()}");
      else
        _dividend = dividend;
    }
    else if (WorkData is not int dividend)
      Status = Op.ThrowBadInput("int", $"{WorkDataType}");
    else
      _dividend = dividend;

    if (_divisor == 0)
      Status = Op.ThrowBadResult("Cannot divide by zero.");

    int quotient = _dividend / _divisor;
    WorkData = quotient;
    Log(MsgClass.Debug, $"{_dividend} / {_divisor} = {quotient}");
    Status = Pass;
    DebugOut();
  }
}
