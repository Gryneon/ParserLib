namespace Parser;

/// <summary>
/// Tokenizer tokens.
/// </summary>
public readonly struct TokenType : IEquatable<TokenType>, IComparable<TokenType>, ICloneable
{
  public readonly string Name
  {
    get => field.ToLowerInvariant();
    init => field = value.ToLowerInvariant();
  }
  public int Flags { get; init; }
  public TokenType () : this(SE, 0) { }
  public TokenType (string name) : this(name, 0) { }
  public TokenType (string name, int flags)
  {
    Name = name;
    Flags = flags;
  }
  public TokenType (TokenType token) : this(token.Name, token.Flags) { }
  public readonly bool Equals (TokenType other) => Name == other.Name && Flags == other.Flags;
  public static bool operator == (TokenType left, TokenType right) => left.Equals(right);
  public static bool operator != (TokenType left, TokenType right) => !(left == right);
  public static TokenType operator | (TokenType left, int right) => new() { Name = left.Name, Flags = left.Flags | right };
  public static TokenType operator & (TokenType left, int right) => new() { Name = left.Name, Flags = left.Flags & right };
  public override bool Equals (object? obj) => obj is string str && Equals(str) || obj is TokenType tt && Equals(tt);
  public override int GetHashCode () => HashCode.Combine(Name, Flags);
  public int CompareTo (TokenType other) => Name.CompareTo(other.Name, SCO);
  public object Clone () => new TokenType() { Name = Name, Flags = Flags };

  public static implicit operator TokenType (string name) => new(name);
  public static implicit operator string (TokenType type) => type.Name;
  public static TokenType Any => new();
}

#if false
public enum TokenType
{
  #region Token Masks
  T_RemoveFlags = TM_RemoveFlags,
  #endregion
  #region Token Flags
  /// <summary>
  /// Token cannot be reduced.
  /// </summary>
  T_Final = TF_Final,
  /// <summary>
  /// Optionally repeat this token.
  /// </summary>
  T_OneOrMany = TF_OneOrMany,
  /// <summary>
  /// A required token, whenExtIs normally optional.
  /// </summary>
  T_Required = TF_Required,
  /// <summary>
  /// An optional token, whenExtIs typically required.
  /// </summary>
  T_Optional = TF_Optional,
  /// <summary>
  /// Unparsed content.
  /// </summary>
  T_Unparsed = TF_Unparsed,
  /// <summary>
  /// Ignore this content.
  /// </summary>
  T_Ignore = TF_Ignore,
  /// <summary>
  /// The entire document.
  /// </summary>
  T_All = TF_All,
  /// <summary>
  /// No type specified.
  /// </summary>
  T_NoType = 0,
  #endregion
  #region Basic Concepts
  T_BlockHead,
  T_BlockStart,
  T_BlockEnd,
  /// <summary>
  /// Operators and Symbols
  /// </summary>
  T_Symbol,
  /// <summary>
  /// The end of the block or script.
  /// </summary>
  T_Fin,
  /// <summary>
  /// The start of a statement.
  /// </summary>
  T_SStart,
  /// <summary>
  /// The end of a statement.
  /// </summary>
  T_SEnd,
  T_DataType,
  #endregion
  #region Control Codes
  T_Escape,
  T_Shift,
  T_Substitute,
  #endregion
  #region Symbols
  /// <summary>
  /// The left bracket character.
  /// </summary>
  T_LBracket,
  /// <summary>
  /// The right bracket character.
  /// </summary>
  T_RBracket,
  /// <summary>
  /// The left brace character.
  /// </summary>
  T_LBrace,
  /// <summary>
  /// The right brace character.
  /// </summary>
  T_RBrace,
  /// <summary>
  /// The left parenthesis character.
  /// </summary>
  T_LParen,
  /// <summary>
  /// The right parenthesis character.
  /// </summary>
  T_RParen,
  T_LAngle,
  T_RAngle,
  /// <summary>
  /// The equals character.
  /// </summary>
  T_Equals,
  /// <summary>
  /// The semicolon character.
  /// </summary>
  T_SemiColon,
  T_Colon,
  T_Comma,
  #endregion
  #region Generic Components
  /// <summary>
  /// Horizontal whitespace.
  /// </summary>
  T_WS,
  /// <summary>
  /// New line character(s).
  /// </summary>
  T_NL,
  /// <summary>
  /// A name, with no quotes, and no whitespace.
  /// </summary>
  T_Name,
  /// <summary>
  /// A keyword in the language.
  /// </summary>
  T_Keyword,
  /// <summary>
  /// A non-decimal numeric value.
  /// </summary>
  T_Int,
  /// <summary>
  /// A decimal numeric value.
  /// </summary>
  T_Dec,
  /// <summary>
  /// A boolean constant.
  /// </summary>
  T_Bool,
  /// <summary>
  /// A char value.
  /// </summary>
  T_Char,
  /// <summary>
  /// A single quoted string value.
  /// </summary>
  T_SinQt,
  /// <summary>
  /// A string value.
  /// </summary>
  T_String,
  /// <summary>
  /// A double quoted string value.
  /// </summary>
  T_DblQt,
  /// <summary>
  /// An object class or data type.
  /// </summary>
  T_Operator,
  /// <summary>
  /// An object.
  /// </summary>
  T_Object,
  /// <summary>
  /// An array.
  /// </summary>
  T_Array,
  T_ArrayItem,
  #endregion
  #region Complex Components
  /// <summary>
  /// A heading describing a section.
  /// </summary>
  T_Heading,
  /// <summary>
  /// A heading that is to be removed.
  /// </summary>
  T_RemHeading,
  /// <summary>
  /// A property or stored data.
  /// </summary>
  T_Property,
  T_RemProperty,
  T_Statement,
  T_Expression,
  /// <summary>
  /// A variable declaration.
  /// </summary>
  T_Declaration,
  T_Assignment,
  T_FunctionCall,
  T_Modifier,
  T_Block,
  T_Param,
  /// <summary>
  /// A parameter list.
  /// </summary>
  T_ParamList,
  /// <summary>
  /// A comment that extends to the next newline character.
  /// </summary>
  T_LnComment,
  T_BlkComment,
  T_PreProc,
  T_Class,
  T_TagOpen,
  T_TagClose,
  T_TagSingle,
  T_Header,
  T_Content,
  T_Key,
  T_DefaultKey,
  T_Value,
  T_AddFlag,
  T_RemFlag,
  T_StrRef,
  T_StateLabel,
  T_FlagCombo,
  T_Sector,
  T_LineDef,
  T_SideDef,
  T_ThingDef,
  T_PointDef,
  T_StateCmd,
  T_FrameDecl,
  T_VarDecl,
  T_VarRef,
  #endregion
}
#endif
