using Common.Extensions;

namespace Parser.V2.Framework;

internal static class FormatLibrary
{
  private static readonly Collection<IDataFormat> _formats = [];

  public static IDataFormat? GetFormat (string name) =>
    _formats.First(item => item.Name.Like(name));

  public static void StoreFormat (IDataFormat format) =>
    _formats.Add(format);
}
