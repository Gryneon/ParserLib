namespace Parser.Ops;

/// <summary>Prompts for input, and stores the input in <paramref name="output_key"/>.
/// Optionally validates the input as well.</summary>
/// <param name="message">The message to prompt.</param>
/// <param name="output_key">The key to store the input in.</param>
/// <param name="validation">An optional validator.</param>
/// <param name="accept_empty">Allow empty strings from the user.</param>
public sealed class PromptOperation (string message, string output_key, Predicate<string>? validation = null, bool accept_empty = false) : Operation(SE, output_key)
{
  public override bool NoInput => true;
  public bool AcceptEmpty { get; } = accept_empty;
  public string Message { get; } = message;
  public Predicate<string>? Validation { get; } = validation;

  protected override void Execute ()
  {
    Console.Write(Message);
    string userInput = Console.ReadLine() ?? SE;

    if (userInput.IsEmpty() && !AcceptEmpty)
    {
      Status = Op.ThrowBadInput("not empty", "empty");
    }
    else if (Validation is null || Validation.Invoke(userInput))
    {
      Status = OpStatus.Pass;
      WorkData = userInput;
    }
    else
    {
      Status = Op.ThrowBadResult("Validation failed.");
    }
  }
}
