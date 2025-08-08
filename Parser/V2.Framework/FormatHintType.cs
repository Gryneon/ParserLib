namespace Parser.V2.Framework;

public enum FormatHintType
{
  Empty = 0,
  Header = 1,
  FileName = 2,
  FileExt = 3,
  FileSize = 4,

  StartsWith = 256,
  EndsWith = 512,
  Equals = 1024,
  LargerThan = 2048,
  SmallerThan = 4096,

  Not = 65536
}