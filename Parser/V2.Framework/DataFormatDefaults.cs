using Common.Extensions;

using InitialObj = Parser.V2.Framework.DataItem<string, System.Collections.ObjectModel.Collection<byte>>;
using KeyItem = System.Collections.Generic.KeyValuePair<string, Parser.V2.Framework.IDataItem>;

namespace Parser.V2.Framework;

public static class DataFormatDefaults
{
  public static Dictionary<string, string> Descriptions { get; } =
    [
    ("initial", "The original file content").ToKVP()
    ];
  public static Dictionary<string, IDataItem> Defaults { get; } =
    [
    new KeyItem("initial", new InitialObj(SE))
    ];
}
