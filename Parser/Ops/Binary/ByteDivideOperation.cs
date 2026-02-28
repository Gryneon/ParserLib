#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteDivideOperation : Operation
{
  private int _divisor, _dividend;
  private bool UseVar => _divisor_key is not null;
  private readonly string? _divisor_key;

  public ByteDivideOperation (int divisor, string dividend_key, string output_key) : base(dividend_key, output_key) => _divisor = divisor;

  public ByteDivideOperation (string divisor_key, string dividend_key, string output_key) : base([divisor_key, dividend_key], output_key) => _divisor_key = divisor_key;

  protected override void Execute ()
  {
    if (!CheckInputs(out Collection<int>? inputs))
    {
      Status = FailBadInputType;
      return;
    }

    if (UseVar)
    {
      _divisor = inputs[0];
      _dividend = inputs[1];
    }
    else
    {
      _dividend = inputs[0];
    }

    if (_divisor == 0)
    {
      Status = FailBadOpResult;
      return;
    }

    int quotient = _dividend / _divisor;
    WorkData = quotient;
    Log("ByteDivideOperation", $"{_dividend} / {_divisor} = {quotient}");
    Status = Pass;
  }
}
