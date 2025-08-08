#pragma warning disable IDE0306 // Simplify collection initialization

namespace Parser.V2.Framework;

public static class TokenFlags
{
  public const int TF_Final = 0x40000;
  public const int TF_OneOrMany = 0x20000;
  public const int TF_Required = 0x10000;
  public const int TF_Optional = 0x8000;
  public const int TF_Unparsed = 0x4000;
  public const int TF_Ignore = 0x2000;
  public const int TF_All = 0x1000;

  public const int TM_RemoveFlags = 0x0FFF;
}
