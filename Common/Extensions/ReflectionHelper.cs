//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Linq;

namespace Common.Extensions;

public static class ReflectionHelper
{
  public static Collection<AttributeLinkedType<TAttribute>> GetAttributedClasses<TAttribute> (BindingFlags flags = BindingFlags.Default)
  where TAttribute : Attribute =>
  [..
    AppDomain.
    CurrentDomain.
    GetAssemblies().
    SelectMany(assy => assy.GetTypes().
    Where(type => type.GetCustomAttribute<TAttribute>() is not null).
    Select(t => new AttributeLinkedType<TAttribute>() { Type = t, Attribute = t.GetCustomAttribute<TAttribute>()! }))
  ];

  public static (TType? Data, TAttribute? Attribute) GetStaticPropertyByAttribute<TType, TAttribute> (Type? type, BindingFlags flags = BFS)
  where TAttribute : Attribute
  where TType : class
  {
    if (type is null) return (null, null);
    PropertyInfo prop = type.GetProperties(flags).First(static prop => prop.GetCustomAttribute<TAttribute>() is not null);
    return (prop.GetValue(null) as TType, prop.GetCustomAttribute<TAttribute>());
  }

  public static Collection<(TType? Data, TAttribute? Attribute)> GetStaticPropertiesByAttribute<TType, TAttribute> (Type? type, BindingFlags flags = BFS)
  where TAttribute : Attribute
  where TType : class
  {
    if (type is null) return [];
    IEnumerable<PropertyInfo> props = type.GetProperties(flags).Where(static prop => prop.GetCustomAttribute<TAttribute>() is not null);
    return [.. props.Select(p => (p.GetValue(null) as TType, p.GetCustomAttribute<TAttribute>()))];
  }
}
