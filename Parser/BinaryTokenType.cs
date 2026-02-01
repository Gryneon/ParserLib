#pragma warning disable CA1822 // Mark members as static

namespace Parser;

public enum BinaryTokenType
{
  Unknown,

  Cmd,
  CmdType,
  DataKey,
  Co, // StoreIn Operator
  Size,
  AccessKey,
  SaveToDataKey,
  LoadFromDataKey,
}
