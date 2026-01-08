namespace Parser.Ops;

/// <summary>Prompts for input, and stores the input in <paramref name="output_key"/>.
/// Optionally validates the input as well.</summary>
/// <param name="message">The message to prompt.</param>
/// <param name="output_key">The key to store the input in.</param>
/// <param name="validation">An optional validator.</param>
public sealed class PromptOperation (string message, string output_key, Predicate<string>? validation = null) : Operation
{
  public string Message { get; } = message;
  public Predicate<string>? Validation { get; } = validation;

  protected override void Execute ()
  {
    Console.Write(Message);
    string? userInput = Console.ReadLine();

    if (userInput is null)
    {
      Status = OpStatus.FailBadInputNull;
      return;
    }

    if (Validation is null || Validation(userInput))
    {
      Data[output_key] = userInput;
      Status = OpStatus.Pass;
      WorkToReturn = userInput;
      return;
    }
    else
    {
      Status = OpStatus.FailBadOpResult;
      return;
    }
  }
}
