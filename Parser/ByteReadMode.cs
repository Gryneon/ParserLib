#pragma warning disable IDE0060 // Remove unused parameter

namespace Parser;

public enum ByteReadMode
{
  None = 0,
  Text = 1,
  Value = 1 << 1,
  Binary = Text | Value,

  ToList = 1 << 2,
}
