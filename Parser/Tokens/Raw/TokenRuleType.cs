#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

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
  SplitMatch = 3,
  /// <summary>This Token Rule will split the input at the exact string provided, limiting future matches.</summary>
  SplitExact = 4,
  /// <summary>This Token Rule will store the unmatched data parts that match the regex provided as tokens with this type.</summary>
  StoreExtra = 5,
  /// <summary>This Token Rule will store the unmatched data parts as tokens with this type.</summary>
  StoreOther = 6,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenProperty{T}"/>.</summary>
  BuildProperty = 7,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenArray{T}"/>.</summary>
  BuildArray = 8,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenObject{T}"/>.</summary>
  BuildObject = 9,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenFlag{T}"/>.</summary>
  BuildFlag = 10,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenTypedValue{T}"/>.</summary>
  BuildTypedValue = 11,
  /// <summary>This Token Group Token Code will store the value as the 'Value' in a <see cref="TokenProperty{T}"/> or <see cref="TokenArray{T}"/>.</summary>
  AssignValue = 12,
  /// <summary>This Token Group Token Code will store the value as the 'Name' in a <see cref="TokenProperty{T}"/> or <see cref="TokenObject{T}"/>.</summary>
  AssignName = 13,
  /// <summary>This Token Group Token Code will store the value as the 'Type' in a <see cref="TokenObject{T}"/>.</summary>
  AssignType = 14,
  /// <summary>This Token Group Token Code will store the value as a 'Property' in a <see cref="TokenObject{T}"/>.</summary>
  AddProperty = 15,
  /// <summary>This Token Group Token Code will set <see cref="TokenFlag{T}.AddFlag"/> to <see langword="true"/> in a <see cref="TokenFlag{T}"/>.</summary>
  AddFlag = 16,
  /// <summary>This Token Group Token Code will set <see cref="TokenFlag{T}.AddFlag"/> to <see langword="false"/> in a <see cref="TokenFlag{T}"/>.</summary>
  RemFlag = 17,
  /// <summary>This Token Rule will only match from existing tokens.</summary>
  /// <remarks>This is useful for special keywords.</remarks>
  FromTokens = 0x10000,
  /// <summary>This Token Rule will exempt all matches from being checked.</summary>
  /// <remarks>This is useful for strings, comments, and quoted or escaped items.</remarks>
  ExemptAllWithin = 0x20000,
  /// <summary>Flags the created token as ignored.</summary>
  /// <remarks>This is useful for whitespace and comments.</remarks>
  IgnoredToken = 0x40000,
  /// <summary>Exact matches and regex will ignore case.</summary>
  /// <remarks>This is useful for languages that are not case sensitive.</remarks>
  IgnoreCase = 0x80000,
  /// <summary>All Token Match Rules with this flag will run concurrently and exclusively.</summary>
  /// <remarks>This is useful for strings and comments.</remarks>
  Competitive = 0x100000,
  /// <summary>The rule will execute until no matches occur.</summary>
  Recursive = 0x1000000,
  /// <summary>This token sequence entry is not required, but will be consumed if present.</summary>
  Opt = 0x2000000,
  /// <summary>This token sequence entry can have additional entries, and will consume them if present.</summary>
  Mult = 0x4000000,
  /// <summary>The bits to remove to get the type correctly.</summary>
  FlagBits = Mult | Opt | Recursive | Competitive | IgnoreCase | IgnoredToken | ExemptAllWithin | FromTokens,
}
