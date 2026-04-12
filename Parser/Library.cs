#pragma warning disable CA1710 // Rename Parser.Library to end in either 'Dictionary' or 'Collection'

using Parser.Inference;

namespace Parser;

public sealed class Library : IReadOnlyDictionary<string, Spec>
{
  private readonly Dictionary<string, Spec> _specs = [];
  /// <summary>The singleton instance of this object.</summary>
  private static Library? Instance { get; set; }
  private static Dictionary<string, ReadOnlyCollection<IInferenceNode>> SpecInferences =>
    [.. Instance?.Select(
      item => new KeyValuePair<string, ReadOnlyCollection<IInferenceNode>>(item.Key, item.Value.FileInferences)) ?? throw new InvalidOperationException("Library must be initialized.")];

  private Library () { }
  public Spec this[string key] { get => _specs[key]; set => _specs[key] = value; }
  public static ReadOnlyCollection<Spec> SpecList => Instance is null ? throw new InvalidOperationException("Library must be initialized.") : [.. Instance._specs.Values];
  public static Spec? Lookup (string? name) => name is not null && Instance is not null && Instance.ContainsKey(name) ? Instance[name] : null;
  public static Spec LookupOrDefault (string? name)
  {
    DebugIn("LookupOrDefault");
    if (Instance is null)
    {
      Log(MsgClass.Error, "Must initialize library before using.");
      DebugOut();
      return DefaultSpec.Unknown;
    }
    DebugOut();
    return (name is null || !TryLookup(name, out Spec? spec)) ? DefaultSpec.Unknown : spec;
  }
  public static bool TryLookup (string? name, [NotNullWhen(true)][MaybeNullWhen(false)] out Spec spec)
  {
    if (name is not null && Instance is not null && Instance.ContainsKey(name))
    {
      spec = Instance[name];
      return true;
    }
    else
    {
      spec = null;
      return false;
    }
  }
  /// <summary>Initializes the library.</summary>
  /// <remarks>This must be called before the library is used.</remarks>
  /// <param name="domain">The domain that we are loading the <see cref="Spec"/> objects from.</param>
  public static void InitializeLibrary (AppDomain domain)
  {
    DebugIn("InitializeLibrary");
    domain.ThrowIfNull();
    Instance = new();
    List<Assembly> assemblies = [.. domain.GetAssemblies()];

    IOrderedEnumerable<Assembly> sorted = assemblies.OrderBy(static i => i.GetName().Name);

    foreach (Assembly assembly in sorted)
    {
      if (assembly.GetName().Name?.StartsWithAny(SCO, "System", "Microsoft", "Common") ?? false)
        continue;

      Type[] types = assembly.GetTypes();

      foreach (Type type in types)
      {
        if (type.GetCustomAttribute<DefinitionExportAttribute>() is null)
          continue;

        foreach (PropertyInfo? prop in type.GetProperties())
        {
          bool isSpec = prop.PropertyType.Name.Is(nameof(Spec));
          bool isMarked = prop.GetCustomAttribute<DefinitionExportAttribute>() is not null;
          if (isSpec && isMarked)
          {
            Spec? check = prop.GetValue(null) as Spec;
            check.ThrowIfNull();
            Instance._specs.Add(check.Name, check);
          }
        }
      }
    }
    Log(MsgClass.Informational, $"{Instance._specs.Count} Specs Loaded.");
    DebugOut();
  }
  /// <summary>Provides the <see cref="Spec"/> for the provided file path.</summary>
  public static string? CheckFile (string path)
  {
    DebugIn("CheckFile");
    if (Instance is null)
    {
      throw new InvalidOperationException("Library must be initialized.");
    }
    foreach (KeyValuePair<string, ReadOnlyCollection<IInferenceNode>> fi in SpecInferences)
    {
      foreach (IInferenceNode node in fi.Value)
      {
        if (node.CheckFile(path))
        {
          Log(MsgClass.Informational, $"File match found. Using {fi.Key} as Spec.");
          DebugOut();
          return fi.Key;
        }
      }
    }
    DebugOut();
    return null;
  }

  public void Add (string key, Spec value) => _specs.Add(key, value);
  public void AddRange (IEnumerable<KeyValuePair<string, Spec>> list) => _specs.AddRange(list);
  public bool ContainsKey (string key) => _specs.ContainsKey(key);
  public bool TryGetValue (string key, [MaybeNullWhen(false)] out Spec value) => _specs.TryGetValue(key, out value);
  public void Add (KeyValuePair<string, Spec> item) => _specs.Add(item);
  public IEnumerator<KeyValuePair<string, Spec>> GetEnumerator () => _specs.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => _specs.GetEnumerator();
  IEnumerable<string> IReadOnlyDictionary<string, Spec>.Keys => _specs.Keys;
  IEnumerable<Spec> IReadOnlyDictionary<string, Spec>.Values => _specs.Values;
  int IReadOnlyCollection<KeyValuePair<string, Spec>>.Count => _specs.Count;
}
