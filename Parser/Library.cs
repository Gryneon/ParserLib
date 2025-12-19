using System.Reflection;

using Parser.Inference;

namespace Parser;

public class Library : KeyedCollection<string, Spec>
{
  /// <summary>
  /// The singleton instance of this object.
  /// </summary>
  protected static Library Instance { get; } = [Spec.Unknown];
  protected static Dictionary<string, ReadOnlyCollection<IInferenceNode>> SpecInferences =>
    [.. Instance.Select(
      item => new KeyValuePair<string, ReadOnlyCollection<IInferenceNode>>(item.Name, item.FileInferences))];

  protected Library ()
  {
    IReadOnlyList<(Type Type, PropertyInfo Property)> matches = ReflectionHelper.GetTypesWithAttributeAndProperties<DefinitionExportAttribute>(typeof(Spec));

    foreach ((Type? type, PropertyInfo? prop) in matches)
    {
      Console.WriteLine($"Type: {type.FullName}, Property: {prop.Name}");
    }
  }
  protected override string GetKeyForItem (Spec item) => item?.Name ?? SE;

  public static void AddToLibrary (Spec spec) => Instance.Add(spec);
  public static void AddToLibrary (IEnumerable<Spec> specs) => Instance.AddRange(specs);
  public static void AddToLibrary (params Collection<Spec> speclist) => Instance.AddRange(speclist);
  public static Spec? Lookup (string? name) => name is not null && Instance.Contains(name) ? Instance[name] : null;
  public static bool TryLookup (string name, [NotNullWhen(true)][MaybeNullWhen(false)] out Spec spec)
  {
    if (Instance.Contains(name) && Instance[name] is Spec s)
    {
      spec = s;
      return true;
    }
    else
    {
      spec = null;
      return false;
    }
  }
  public static new int Count => Instance.Count();
  /// <summary>
  /// Provides the <see cref="Spec"/> for the provided file path.
  /// </summary>
  public static string? CheckFile (string path)
  {
    foreach (KeyValuePair<string, ReadOnlyCollection<IInferenceNode>> fi in SpecInferences)
    {
      foreach (IInferenceNode node in fi.Value)
      {
        if (node.CheckFile(path))
        {
          Log($"CheckFile({path})", "File match found. S");
          return fi.Key;
        }
      }
    }
    return null;
  }
  public static IEnumerable<Spec> SpecList => Instance.ToCollection();
}
