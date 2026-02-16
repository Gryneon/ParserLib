#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1720 // Identifier contains type name

namespace Specification.ACS;

public enum ACSTokenType
{
  None,
  Str,
  Char,
  Int,
  Fixed,
  Bool,
  Name,
  FunctionCall,
  FunctionCallStatement,
  Script,
  Function,
  MapVar,
  ScriptType,
  /// <summary>Placeholder representing any value, expression, or function call.</summary>
  Value,
  Bo, Bc,
  Po, Pc,
  Expression,
  Sc, Cm,
  Op, Co,
  Ao, Ac,
  Preprocessor,
  Eq,
  If,
  For,
  IncDec,
  LogNot,
  Assign,
  Minus,
  Unary,
  Binary,
  Net,
  Loop,
  Do,
  Switch,
  Case,
  Default,
  Return,
  SimpleJump,
  Void,
  /// <summary>Keyword only. Any type object, such as <c>str</c>, <c>char</c>, <c>bool</c>, or <c>int</c>.</summary>
  Type,
  /// <summary>Keyword Only - Wait Functions</summary>
  Wait,
  PreprocessorFull,
  IfBlock,
  ElseBlock,
  ElseIfBlock,
  VarDecl,
  VarDeclAssn,
  ArrayDecl,
  VarAssn,
  Else,
  Statement,
  BasicCmd,
  VarInc,
  Parameter,
  ArrayValue,
  /// <summary>An expression that can also be used as an independent statement.</summary>
  ExpressionStatement,
  FunctionHeader,
  ScriptHeader,
  PrintParameterValue,
  ParameterValue,
  WaitCall,
  Block,
  ScriptFull,
  FunctionFull,
  PrintFunction
}

public enum ModelDefTokenType
{
  None,

  //Keywords
  Model,
  Skin,
  Path,
  Scale,
  FrameIndex,

  //Flags
  PitchFromMomentum,
  InheritActorPitch,

  //Basic Structures
  Int,
  Decimal,
  String,
  Frame,
  FrameLetter,

  //Objects
  ModelFull,
  PropertyLine,
  FrameLine,
}
