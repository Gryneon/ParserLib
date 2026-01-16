#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1720 // Identifier contains type name

namespace Specification.SndInfo;

public enum SndInfoTokenType
{
  Ignore,
  Command,
  Bo, Bc,
  Eq,
  SoundFile,
  SoundLump,
  Integer,
  Decimal,
  String,
  PlayerSound,

}
