#pragma warning disable CA1822 // Mark members as static

using Parser.Inference;
using Parser.Tokens.Raw;

namespace Parser;

/// <summary>A class containing the operations, requirements, and instructions for reading a file.</summary>
public class Spec
{
  #region Static Members
  /// <summary>The currently active specififcation. Used for objects that cannot see the parser.</summary>
  public static Spec Active { get => field ?? DefaultSpec.Unknown; protected set; }
  /// <summary>Use this in operations that support it to load the values defined in the <see cref="Spec"/>.</summary>
  public const int LoadFromSpec = 0x7fffffff;
  #endregion
  /// <summary>The name of this Spec.</summary>
  public required string Name { get; init; }
  /// <summary>A <see cref="Collection{IOperation}"/> of <see cref="Ops.Operation"/> objects that are executed in order to produce the result.</summary>
  public required Collection<IOperation> Operations { get; init; }
  /// <summary>A <see cref="Collection{IInferenceNode}"/> of <see cref="InferenceNode"/> objects that specify what files use this specification.</summary>
  public required ReadOnlyCollection<IInferenceNode> FileInferences { get; init; }
  /// <summary>Determines whether to use a byte parser or a text one.</summary>
  public bool IsTextFile { get; init; }
  /// <summary>The default regex options to use.</summary>
  public RegexOptions RxOpt
  {
    get => field | (SC == SCOIC ? ROIC : RON);
    init;
  }
  /// <summary>The default string comparison type to use.</summary>
  public StringComparison SC { get; init; } = SCO;
  /// <summary>Define the names for tranlating the string based rules.</summary>
  public Dictionary<string, dynamic> TokenTypeLookup { get; init; } = [];
  /// <summary>Define the names of equilivent groups of tokens.</summary>
  public Dictionary<dynamic, Collection<dynamic>> TokenCompatLookup { get; init; } = [];
  /// <summary>Token rules for the tokenize operration..</summary>
  public Collection<TokenRule<dynamic>> TokenRules { get; init; } = [];
  /// <summary>Group token rules for the tokenize operration..</summary>
  public Collection<TokenGroupRule<dynamic>> GroupTokenRules { get; init; } = [];
  /// <summary>Token types that are basic building blocks.</summary>
  public Collection<string> RegexBasicTokens { get; init; } = [];
  /// <summary>Token types to ignore.</summary>
  public Collection<string> WhitespaceTokens { get; init; } = [];
  /// <summary>All token types handled by this specification.</summary>
  public Collection<string> AllTokens => RegexBasicTokens.Concat(WhitespaceTokens).ToCollection();
  /// <summary>Marks this instance as the active specification.</summary>
  /// <remarks>Subsequent operations that depend on the active object will reference this instance after calling
  /// this method. If another instance was previously active, it will be replaced.</remarks>
  public void SetAsActive () => Active = this;
  /// <summary>This casts the TokenRules to a specific TokenType.</summary>
  /// <typeparam name="T">The Token Type to cast the rules to. This must be a string or an enum.</typeparam>
  /// <returns>The casted rule collection.</returns>
  public Collection<TokenRule<T>> FromDynamic<T> () where T : notnull, new() => [.. TokenRules.Select(static rule => new TokenRule<T>()
  {
    TypeToAssign = (T) rule.TypeToAssign,
    Type = rule.Type,
    RuleStringData = rule.RuleStringData
  })];
}
