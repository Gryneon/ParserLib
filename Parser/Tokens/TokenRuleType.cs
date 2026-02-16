#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

/// <summary>The type of rule to enforce.</summary>
[Flags]
public enum TokenRuleType : long
{
  /// <summary>No type, Rule will be ignored.</summary>
  None = 0,
  /// <summary>This Token Rule will exactly match the string provided.</summary>
  TokenExact = 0x1,
  /// <summary>This Token Rule will match to the regex provided.</summary>
  TokenMatch = 0x2,
  /// <summary>This Token Rule will split the input at the regex provided, limiting future matches.</summary>
  SplitMatch = 0x4,
  /// <summary>This Token Rule will split the input at the exact string provided, limiting future matches.</summary>
  SplitExact = 0x8,
  /// <summary>This Token Rule will store the unmatched data parts that match the regex provided as tokens with this type.</summary>
  StoreExtra = 0x10,
  /// <summary>This Token Rule will store the unmatched data parts as tokens with this type.</summary>
  StoreOther = 0x20,
  /// <summary>This token or sequence means the parsed content is not valid.</summary>
  ErrorMatch = 0x40,
  /// <summary>Extracts and uses the value(s) stored in the group named 'keep'.
  /// If there are multiple captures, they will be stored as additional tokens of this type.</summary>
  /// <remarks>The entire match is exempted if flagged as exempt, but the value of the created tokens will ONLY be what is in the named matching group 'keep'.</remarks>
  TokenExtract = 0x80,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenProperty"/>.</summary>
  BuildProperty = 0x100,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenArray"/>.</summary>
  BuildArray = 0x200,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenObject"/>.</summary>
  BuildObject = 0x400,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenFlag"/>.</summary>
  BuildFlag = 0x800,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenTypedValue"/>.</summary>
  BuildTypedValue = 0x1000,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenLabel"/>.</summary>
  BuildLabel = 0x2000,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenStatement"/>.</summary>
  BuildStatement = 0x4000,
  /// <summary>This Token Group Token Code will store the value as the 'Value' in a <see cref="TokenProperty"/>, <see cref="TokenTypedValue"/>, or <see cref="TokenArray"/>.</summary>
  AssignValue = 0x8000,
  /// <summary>This Token Group Token Code will store the value as the 'Name' in a <see cref="TokenProperty"/> or <see cref="TokenObject"/>.</summary>
  AssignName = 0x10000,
  /// <summary>This Token Group Token Code will store the value as the 'Type' in a <see cref="TokenObject"/> or <see cref="TokenTypedValue"/>.</summary>
  AssignType = 0x20000,
  /// <summary>This Token Group Token Code will store the value as a 'Property' in a <see cref="TokenObject"/>.</summary>
  AddProperty = 0x40000,
  /// <summary>This Token Group Token Code will set <see cref="TokenFlag.State"/> to <see langword="true"/> in a <see cref="TokenFlag"/>.</summary>
  AddFlag = 0x80000,
  /// <summary>This Token Group Token Code will set <see cref="TokenFlag.State"/> to <see langword="false"/> in a <see cref="TokenFlag"/>.</summary>
  SubFlag = 0x100000,
  /// <summary>This Token Rule will only match from existing tokens.</summary>
  /// <remarks>This is useful for special keywords.</remarks>
  FromTokens = 0x200000,
  /// <summary>This Token Rule will exempt all matches from being checked.</summary>
  /// <remarks>This is useful for strings, comments, and quoted or escaped items.</remarks>
  ExemptAllWithin = 0x400000,
  /// <summary>Flags the created token as ignored.</summary>
  /// <remarks>This is useful for whitespace and comments.</remarks>
  IgnoredToken = 0x800000,
  /// <summary>Exact matches and regex will ignore case.</summary>
  /// <remarks>This is useful for languages that are not case sensitive.</remarks>
  IgnoreCase = 0x1000000,
  /// <summary>All Token Match Rules with this flag will run concurrently and exclusively.</summary>
  /// <remarks>This is useful for strings and comments.</remarks>
  Competitive = 0x2000000,
  /// <summary>The rule will execute until no more matches occur.</summary>
  Recursive = 0x4000000,
  /// <summary>This token sequence entry is not required, but will be consumed if present.</summary>
  Opt = 0x8000000,
  /// <summary>This token sequence entry can have additional entries, and will consume them if present.</summary>
  Mult = 0x10000000,
  Any = Opt | Mult,
  /// <summary>The bits to remove to get the type correctly.</summary>
  FlagBits = Mult | Opt | Recursive | IgnoreCase | IgnoredToken | ExemptAllWithin | FromTokens | ErrorMatch,
  /// <summary>This Token Group Rule will assemble a <see cref="TokenExpression"/> differently than normal. It will store the left hand, the operator, and the right hand values.</summary>
  BuildExpression = 0x20000000,
  /// <summary>This token sequence entry will supply any fields not already filled by other definitions from its own respective values.</summary>
  /// <remarks>If a <see cref="TokenObject"/> was passed to any field of a <see cref="TokenObject"/> with this flag assigned, it would copy all of its fields to the parent <see cref="TokenObject"/>.
  /// These properties would be overwritten by any defined token sequence entries.</remarks>
  Descendant = 0x40000000,
  /// <summary>This flag means a character was not recognized.</summary>
  Error = 0x80000000L,
  /// <summary>This flag means a character was not recognized.</summary>
  AssignLeft = 0x100000000L,
  /// <summary>This flag means a character was not recognized.</summary>
  AssignRight = 0x200000000L,
  /// <summary>This flag means a character was not recognized.</summary>
  AssignCenter = 0x400000000L,
  AddParameter = 0x800000000,
  AddStatement = 0x1000000000,
  AssignCustomProp = 0x2000000000,
}
