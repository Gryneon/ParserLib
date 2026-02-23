using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Specification.ZDoom.UDMFTokenType;

namespace Specification.UDMF;

[DefinitionExport]

}

public abstract class ZMapObj
{
  protected virtual string GroupName => EmptyString;

  public Collection<IProperty<string>> Properties { get; } = [];
  public bool TryGetProperty<T> (string key, [NotNullWhen(true)][MaybeNullWhen(false)] out T value) where T : IParsable<T>
  {
    value = default;
    return T.TryParse(Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE, null, out value);
  }
  public bool TryGetProperty (string key, out string value)
  {
    value = Properties.First(p => p.Key.Equals(key, SCOIC)).Value ?? SE;
    return true;
  }
}

public class ZVertex : ZMapObj, IGeneratable
{
  public string? X => Properties.Single(item => item.Key.Like("x")).Value;
  public string? Y => Properties.Single(item => item.Key.Like("y")).Value;

  public static ZVertex Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input) => CanGenerate(input);
}

public class ZThing : ZMapObj, IGeneratable
{
  public string? X => Properties.Single(item => item.Key.Like("x")).Value;
  public string? Y => Properties.Single(item => item.Key.Like("y")).Value;
  public static ZThing Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input) => CanGenerate(input);
}

public class ZLineDef : ZMapObj, IGeneratable
{
  public static ZLineDef Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input)
  {
    input.ThrowIfNull();
    return input.Name.Like("linedef");
  }
}

public class ZSideDef : ZMapObj, IGeneratable
{
  public static ZSideDef Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input)
  {
    input.ThrowIfNull();
    return input.Name.Like("sidedef");
  }
}

public class ZSector : ZMapObj, IGeneratable
{
  public static ZSector Generate (TokenObject input)
  {
    input.ThrowIfNull();
    return new();
  }
  public static bool CanGenerate (TokenObject input) => CanGenerate(input);
}

