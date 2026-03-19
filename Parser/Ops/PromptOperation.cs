namespace Parser.Ops;

/// <summary>Prompts for input, and stores the input in <paramref name="output_key"/>.
/// Optionally validates the input as well.</summary>
/// <param name="message">The message to prompt.</param>
/// <param name="output_key">The key to store the input in.</param>
/// <param name="validation">An optional validator.</param>
public sealed class PromptOperation (string message, string output_key, Predicate<string>? validation = null) : Operation(SE, output_key)
{
  public string Message { get; } = message;
  public override bool NoInput => true;
  public Predicate<string>? Validation { get; } = validation;

  protected override void Execute ()
  {
    Console.Write(Message);
    string? userInput = Console.ReadLine();

    if (userInput is null && Validation is not null)
      goto OpFailInput;
    else if (userInput is null)
      goto OpFailDef;
    else if (Validation is null || Validation(userInput))
      goto OpPass;
    else
      goto OpFail;

  OpFail:
    Status = Op.ThrowBadResult("Validation Operation Failed.");
    return;
  OpPass:
    Status = OpStatus.Pass;
    WorkData = userInput;
    return;
  OpFailInput:
    Status = Op.ThrowBadInput("string", "null");
    return;
  OpFailDef:
    Status = Op.ThrowBadDef("Validation is null and input was also null. Undefined.");
    return;
  }
}
