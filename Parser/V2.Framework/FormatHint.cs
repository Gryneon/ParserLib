using Common.Extensions;

namespace Parser.V2.Framework;

public interface IFormatHint
{
  dynamic Value { get; }
  FormatHintType Type { get; }
}

public struct FormatHintInt : IFormatHint
{
  /// <summary>
  /// The value of the hint struct.
  /// </summary>
  public readonly dynamic Value => ValueInt;

  /// <summary>
  /// The integer value of this hint struct.
  /// </summary>
  public int ValueInt { get; set; }

  /// <summary>
  /// The type of the hint struct.
  /// </summary>
  public FormatHintType Type
  {
    readonly get => field;
    set => field = value.HasAnyFlag(FormatHintType.FileSize | FormatHintType.LargerThan | FormatHintType.SmallerThan) ?
      value :
      throw new Exception();
  }
}
public struct FormatHintStr : IFormatHint
{
  /// <summary>
  /// The value of the hint struct.
  /// </summary>
  public readonly dynamic Value => ValueStr;

  /// <summary>
  /// The integer value of this hint struct.
  /// </summary>
  public string ValueStr { get; set; }

  /// <summary>
  /// The type of the hint struct.
  /// </summary>
  public FormatHintType Type
  {
    readonly get => field;
    set => field = !value.HasAnyFlag(FormatHintType.FileSize | FormatHintType.LargerThan | FormatHintType.SmallerThan) ?
      value :
      throw new ArgumentException("A string attemped the store in the wrong type of hint container. This one requires an int.");
  }
}