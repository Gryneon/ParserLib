namespace Parser.Ops;

/// <summary>Prints a key that implements <see cref="IPrintable"/>.</summary>
/// <param name="input_key">The key to print.</param>
/// <remarks>If the key does not implement <see cref="IPrintable"/>, nothing happens, and the operation returns <see cref="OpStatus.Skipped"/>.</remarks>
public class DebugPrintKeyOperation (string input_key) : Operation(input_key, SE)
{
  public override bool NoOutput => true;

  /// <inheritdoc/>
  /// <remarks>This is a debugging operation and does not store data.</remarks>
  protected override void Execute ()
  {
    if (WorkData is IPrintable printable)
    {
      printable.Print();
      NewLine();
      Status = OpStatus.Pass;
    }
    else
    {
      Status = OpStatus.Skipped;
    }
  }

  public override string ToString () => $"DebugPrintKeyOperation Key = \"{LengthKey}\"";
}
