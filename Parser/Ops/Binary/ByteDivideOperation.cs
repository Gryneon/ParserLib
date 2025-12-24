#pragma warning disable IDE0060 // Remove unused parameter

using static Parser.OpStatus;

namespace Parser.Ops.Binary;

public class ByteDivideOperation : Operation
{
  private readonly int _divisor;
  private bool UseVar => _divisor_key is not null;
  private readonly string? _divisor_key;
  private readonly string? _dividend_key;

  public ByteDivideOperation (int divisor, string dividend_key, string output_key) : base(dividend_key, output_key)
  {
    _divisor = divisor;
    _dividend_key = dividend_key;
  }

  public ByteDivideOperation (string divisor_key, string dividend_key, string output_key) : base(divisor_key, output_key)
  {
    _divisor_key = divisor_key;
    _dividend_key = dividend_key;
  }

  protected override void Execute ()
  {
    if (!CheckInput(out int? dividend))
    {
      Status = FailBadInputType;
      return;
    }
    if (_divisor == 0)
    {
      Status = FailBadOpDefinition;
      return;
    }
    int quotient = dividend.Value / _divisor;
    WorkToReturn = quotient;
    Log("ByteDivideOperation", $"{dividend} / {_divisor} = {quotient}");
    Status = Pass;
  }
}
