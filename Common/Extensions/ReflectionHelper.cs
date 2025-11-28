//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class ReflectionHelper
{
  public static IReadOnlyList<(Type Type, PropertyInfo Property)> GetTypesWithAttributeAndProperties<TAttribute> (
    Type? propertyTypeFilter = null,
    BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static)
    where TAttribute : Attribute
  {
    List<(Type, PropertyInfo)> result = [];

    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      Type[] types;
      try
      {
        types = assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        types = ex.Types.Where(t => t != null).ToArray()!;
      }

      foreach (Type type in types)
      {
        if (type == null || !type.IsClass) continue;

        if (!Attribute.IsDefined(type, typeof(TAttribute), inherit: true))
          continue;

        // Get all public static properties
        PropertyInfo[] properties = type.GetProperties(bindingFlags);

        foreach (PropertyInfo prop in properties)
        {
          if (propertyTypeFilter == null || propertyTypeFilter.IsAssignableFrom(prop.PropertyType))
          {
            result.Add((type, prop));
          }
        }
      }
    }

    return result;
  }
}
