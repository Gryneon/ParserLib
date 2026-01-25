#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

/// <summary>The type of rule to enforce.</summary>
[Flags]
public enum TokenRuleType
{
  /// <summary>No type, Rule will be ignored.</summary>
  None = 0,
  /// <summary>This Token Rule will exactly match the string provided.</summary>
  TokenExact = 1,
  /// <summary>This Token Rule will match to the regex provided.</summary>
  TokenMatch = 2,
  /// <summary>This Token Rule will split the input at the regex provided, limiting future matches.</summary>
  SplitMatch = 4,
  /// <summary>This Token Rule will split the input at the exact string provided, limiting future matches.</summary>
  SplitExact = 8,
  /// <summary>This Token Rule will store the unmatched data parts that match the regex provided as tokens with this type.</summary>
  StoreExtra = 16,
  /// <summary>This Token Rule will store the unmatched data parts as tokens with this type.</summary>
  StoreOther = 32,
  /// <summary>This token or sequence means the parsed content is not valid.</summary>
  ErrorMatch = 64,
  /// <summary>Extracts and uses the value(s) stored in the group named 'keep'.
  /// If there are multiple captures, they will be stored as additional tokens of this type.</summary>
  /// <remarks>The entire match is exempted if flagged as exempt, but the value of the created tokens will ONLY be what is in the named matching group 'keep'.</remarks>
  TokenExtract = 64 * 2,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenProperty"/>.</summary>
  BuildProperty = 64 * 4,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenArray"/>.</summary>
  BuildArray = 64 * 4 * 2,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenObject"/>.</summary>
  BuildObject = 64 * 16,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenFlag"/>.</summary>
  BuildFlag = 32 * 16 * 4,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenTypedValue"/>.</summary>
  BuildTypedValue = 64 * 16 * 4,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenLabel"/>.</summary>
  BuildLabel = 64 * 8 * 16,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenStatement"/>.</summary>
  BuildStatement = 0x20000000,
  /// <summary>This Token Group Token Code will store the value as the 'Value' in a <see cref="TokenProperty"/>, <see cref="TokenTypedValue"/>, or <see cref="TokenArray"/>.</summary>
  AssignValue = 64 * 64 * 4,
  /// <summary>This Token Group Token Code will store the value as the 'Name' in a <see cref="TokenProperty"/> or <see cref="TokenObject"/>.</summary>
  AssignName = 64 * 64 * 8,
  /// <summary>This Token Group Token Code will store the value as the 'Type' in a <see cref="TokenObject"/> or <see cref="TokenTypedValue"/>.</summary>
  AssignType = 64 * 64 * 8 * 2,
  /// <summary>This Token Group Token Code will store the value as a 'Property' in a <see cref="TokenObject"/>.</summary>
  AddProperty = 64 * 64 * 8 * 4,
  /// <summary>This Token Group Token Code will set <see cref="TokenFlag.AddFlag"/> to <see langword="true"/> in a <see cref="TokenFlag"/>.</summary>
  AddFlag = 64 * 64 * 64,
  /// <summary>This Token Group Token Code will set <see cref="TokenFlag.AddFlag"/> to <see langword="false"/> in a <see cref="TokenFlag"/>.</summary>
  RemFlag = 64 * 64 * 64 * 2,
  /// <summary>This Token Rule will only match from existing tokens.</summary>
  /// <remarks>This is useful for special keywords.</remarks>
  FromTokens = 64 * 64 * 64 * 4,
  /// <summary>This Token Rule will exempt all matches from being checked.</summary>
  /// <remarks>This is useful for strings, comments, and quoted or escaped items.</remarks>
  ExemptAllWithin = 64 * 64 * 64 * 8,
  /// <summary>Flags the created token as ignored.</summary>
  /// <remarks>This is useful for whitespace and comments.</remarks>
  IgnoredToken = 64 * 64 * 64 * 8 * 2,
  /// <summary>Exact matches and regex will ignore case.</summary>
  /// <remarks>This is useful for languages that are not case sensitive.</remarks>
  IgnoreCase = 64 * 64 * 64 * 32,
  /// <summary>All Token Match Rules with this flag will run concurrently and exclusively.</summary>
  /// <remarks>This is useful for strings and comments.</remarks>
  Competitive = 64 * 64 * 64 * 64,
  /// <summary>The rule will execute until no more matches occur.</summary>
  Recursive = 64 * 64 * 64 * 64 * 2,
  /// <summary>This token sequence entry is not required, but will be consumed if present.</summary>
  Opt = 64 * 64 * 64 * 64 * 4,
  /// <summary>This token sequence entry can have additional entries, and will consume them if present.</summary>
  Mult = 64 * 64 * 64 * 64 * 8,
  /// <summary>The bits to remove to get the type correctly.</summary>
  FlagBits = Mult | Opt | Recursive | Competitive | IgnoreCase | IgnoredToken | ExemptAllWithin | FromTokens | ErrorMatch,
}
