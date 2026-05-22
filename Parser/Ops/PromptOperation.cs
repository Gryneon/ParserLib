namespace Parser.Ops;

/// <summary>Prompts for input, and stores the input.
/// Optionally validates the input as well.</summary>
public sealed class PromptOperation : Operation
{
  public required string Message { get; init; }
  public string? UserKey { get; init; }
  public override bool NoInput => true;
  public Predicate<string>? Validation { get; init; }

  /// <summary>Constructor for <see cref="PromptOperation"/>.</summary>
  /// <param name="message">The message to prompt.</param>
  /// <param name="user_key">The key to store the input in.</param>
  /// <param name="validation">An optional validator.</param>
  public PromptOperation (string message, string? user_key = null, Predicate<string>? validation = null)
  {
    Message = message;
    Validation = validation;
    UserKey = user_key;
  }
  public PromptOperation () { }
  protected override void Execute ()
  {
    Log(MsgClass.Prompt, Message);
    string userInput = Console.ReadLine()!;

    if (Validation?.Invoke(userInput) is null or true)
    {
      Status = OpStatus.Pass;
      Data[UserKey] = userInput;
      return;
    }

    Status = Err.ThrowBadResult("Validation Operation Failed.");
  }
}
