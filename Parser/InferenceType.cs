namespace Parser;

public enum InferenceType
{
  Unknown = 0,

  Ext,
  FName,
  FileHeader,
  FileContent,
  FileSize,

  And,
  Or,

  Is = 0x1000, //Case sensitive
  Like = 0x80000, //Case insensitive
  End = 0x2000, //endswith
  Start = 0x4000, //startswith
  Contains = 0x8000, //contatins
  Larger = 0x10000, // greater than
  Smaller = 0x20000, // less than
  Not = 0x40000 // inverse
}
