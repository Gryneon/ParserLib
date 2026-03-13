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
  /// <remarks>Use <c>error_pos</c> to specify the character position to point at in the error report.</remarks>
  ErrorMatch = 0x40,
  /// <summary>This Token Rule will throw away the matched tokens.</summary>
  ThrowMatch = TokenMatch | IgnoredToken,
  /// <summary>Extracts and uses the value(s) stored in the group named 'keep'.
  /// If there are multiple captures, they will be stored as additional tokens of this type.</summary>
  /// <remarks>The entire match is exempted if flagged as exempt, but the value of the created tokens will ONLY be what is in the named matching group 'keep'.</remarks>
  TokenExtract = 0x80,
  /// <summary>Flags the created token as ignored.</summary>
  /// <remarks>This is useful for whitespace and comments.</remarks>
  IgnoredToken = 0x100,
  /// <summary>Exact matches and regex will ignore case.</summary>
  /// <remarks>This is useful for languages that are not case sensitive.</remarks>
  IgnoreCase = 0x200,
  /// <summary>All Token Match Rules with this flag will run concurrently and exclusively.</summary>
  /// <remarks>This is useful for strings and comments.</remarks>
  Competitive = 0x400,
  /// <summary>The rule will execute until no more matches occur.</summary>
  Recursive = 0x800,
  /// <summary>This token sequence entry is not required, but will be consumed if present.</summary>
  Opt = 0x1000,
  /// <summary>This token sequence entry can have additional entries, and will consume them if present.</summary>
  Mult = 0x2000,
  /// <summary>This token sequence entry can have no entries, or additional entries, and will consume them if present.</summary>
  Any = Opt | Mult,
  /// <summary>The bits to remove to get the type correctly.</summary>
  FlagBits = Any | Recursive | IgnoreCase | IgnoredToken | Error,
  /// <summary>This flag means a character was not recognized.</summary>
  Error = 0x4000,
  /// <summary>This Token Rule alias is short for <see cref="Competitive"/> and <see cref="IgnoredToken"/>.</summary>
  TokenComment = Competitive | IgnoredToken,
}
