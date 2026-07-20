#pragma warning disable CA1822 // Mark members as static

using Parser.Inference;

namespace Parser;

/// <summary>A class containing the operations, requirements, and instructions for reading a file.</summary>
/// <remarks>This class is immutable, and should never be altered during operation, aside from the <see langword="static"/> property <see cref="Active"/>.</remarks>
public class Spec
{
  #region Static Members
  /// <summary>The currently active specififcation. Used for objects that cannot see the parser.</summary>
  public static Spec Active { get => field ?? XParser.Lib["unknown"]; protected set; }
  #endregion
  /// <summary>The name of this Spec.</summary>
  public required string Name { get; init; }
  /// <summary>A <see cref="Collection{IOperation}"/> of <see cref="Operation"/> objects that are executed in order to produce the result.</summary>
  public ReadOnlyCollection<IOperation> Operations { get; init; } = [];
  /// <summary>A <see cref="Collection{InferenceNode}"/> of <see cref="InferenceNode"/> objects that specify what files use this specification.</summary>
  public ReadOnlyCollection<InferenceNode> FileInferences { get; init; } = [];
  /// <summary>Determines whether to use a byte parser or a text one.</summary>
  public bool IsTextFile { get; init; }
  /// <summary>The default regex options to use.</summary>
  public RegexOptions RxOpt
  {
    get => field | (SC == SCOIC ? ROIC : RON);
    init;
  }
  /// <summary>The type of token type to use.</summary>
  public Type TokenType { get; init; } = typeof(string);
  /// <summary>The default string comparison type to use.</summary>
  /// <remarks>This will automatically set <see cref="RegexOptions.IgnoreCase"/> if this is set to <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
  public StringComparison SC { get; init; } = SCO;

  /// <summary>Define the names of equilivent groups of tokens.</summary>
  public Dictionary<dynamic, Collection<dynamic>> TokenCompatLookup { get; init; } = [];
  /// <summary>The default rule that is added to every rule definition.</summary>
  /// <remarks>Useful for adding ignore case to the entire defintion.</remarks>
  public RT DefaultRuleSet { get; init; } = RT.None;
  /// <summary>Token rules for the tokenize operation.</summary>
  public TokenRuleCollection TokenRules { get; init; } = [];
  /// <summary>Group token rules for the token assembly operation.</summary>
  public TokenRuleCollection GroupTokenRules { get; init; } = [];
  /// <summary>Marks this instance as the active specification.</summary>
  /// <remarks>Subsequent operations that depend on the active object will reference this instance after calling
  /// this method. If another instance was previously active, it will be replaced.</remarks>
  public void SetAsActive () => Active = this;
  /// <summary>Gets the <see cref="Enum"/> or value of the token type.</summary>
  /// <param name="tokenType">The name of the token type.</param>
  /// <remarks>This is dependent on defining <see cref="TokenType"/> in the defined <see cref="Spec"/>.</remarks>
  /// <returns>The value of the token type.</returns>
  /// <exception cref="ArgumentException">Tokentype is not valid.</exception>
  /// <exception cref="InvalidOperationException"></exception>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="OverflowException"></exception>
  public dynamic GetTokenTypeValue (string tokenType) => TokenType == typeof(string) || TokenType is null
      ? tokenType
      : (dynamic) (TokenType.IsEnum ? Enum.Parse(TokenType, tokenType) : throw new ArgumentException("TokenType is not valid."));
  /// <summary>Gets the name of the token type.</summary>
  /// <param name="tokenType">The value of the token type.</param>
  /// <returns>The name of the token type.</returns>
  /// <exception cref="InvalidCastException"></exception>
  /// <exception cref="SpecNotDefinedException"><see cref="TokenType"/> was not defined in the <see cref="Spec"/>.</exception>
  public string GetTokenTypeString (dynamic tokenType) => TokenType == typeof(string)
      ? (string) tokenType
      : TokenType is null ? throw new SpecNotDefinedException("TokenType was not defined in the Spec.") : $"{tokenType}";
}
