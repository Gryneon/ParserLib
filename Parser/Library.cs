#pragma warning disable CA1710 // Rename Parser.Library to end in either 'Dictionary' or 'Collection'

using System.Xml.Linq;

using Parser.Inference;

namespace Parser;

public static class SpecInstructionParser
{
  private static readonly XNamespace NS = "Parser/Spec";

  public static Spec LoadSpec (string path)
  {
    XDocument doc = XDocument.Load(path);
    XElement? root = doc.Root ?? throw Err.ThrowNoSpec("Spec XML is not good.");
    string name = (root.Element(NS + "Name") ?? throw Err.ThrowNoSpec("Invalid XML - No Name in Spec.")).Value;
    bool? textfile = bool.TryParse(root.Element(NS + "TextFile")?.Value, out bool result) ? result : null;
    OperationFactory factory = new(NS);
    // Parse instructions
    IEnumerable<XElement>? instructionElements = root.Element(NS + "Instructions")?.Elements();
    List<IOperation> ops = [.. instructionElements?.Select(factory.Produce) ?? []];

    // Parse file inferences
    XElement? fileInf = root.Element(NS + "FileInferences");
    List<InferenceNode> inferenceNodes = [];// = ParseFileInferences(fileInf);

    return new Spec
    {
      Name = name,
      Operations = [.. ops],
      FileInferences = [.. inferenceNodes],
      IsTextFile = textfile ?? true,
    };
  }
}

public sealed class Library : IReadOnlyDictionary<string, Spec>, IPrintable
{
  private readonly Dictionary<string, Spec> _specs = [];
  /// <summary>The singleton instance of this object.</summary>
  private static Library? Instance { get; set; }
  private static Dictionary<string, ReadOnlyCollection<InferenceNode>> SpecInferences =>
    [.. Instance?.Select(
      item => new KeyValuePair<string, ReadOnlyCollection<InferenceNode>>(item.Key, item.Value.FileInferences)) ?? throw new InvalidOperationException("Library must be initialized.")];

  private Library () { }
  public Spec this[string key] { get => _specs[key]; set => _specs[key] = value; }
  public static ReadOnlyCollection<Spec> SpecList => Instance is null ? throw new InvalidOperationException("Library must be initialized.") : [.. Instance._specs.Values];
  public static Spec? Lookup (string? name) => name is not null && Instance?.ContainsKey(name) == true ? Instance[name] : null;
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
    if (name is not null && Instance?.ContainsKey(name) == true)
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
    DebugIn("Library", "InitializeLibrary");

    Instance = new();

    foreach (string path in Directory.EnumerateFiles("Specs"))
    {
      Spec loaded = SpecInstructionParser.LoadSpec(path);

      Instance._specs.Add(loaded.Name, loaded);
    }

    domain.ThrowIfNull();

    List<Assembly> assemblies = [.. domain.GetAssemblies()];

    foreach (Assembly assembly in assemblies.OrderBy(static i => i.GetName().Name))
    {
      if (assembly.GetName().Name?.StartsWithAny(SCO, "System", "Microsoft", "Common") ?? false)
        continue;

      foreach (Type type in assembly.GetTypes())
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
    Log(MsgClass.BlueInfo, $"{Instance._specs.Count} Specs Loaded.");
    DebugOut();
  }
  /// <summary>Provides the <see cref="Spec"/> for the provided file path.</summary>
  public static string? CheckFile (string path)
  {
    DebugIn("Library", "CheckFile");
    if (Instance is null)
    {
      throw new InvalidOperationException("Library must be initialized.");
    }
    foreach (KeyValuePair<string, ReadOnlyCollection<InferenceNode>> fi in SpecInferences)
    {
      foreach (InferenceNode node in fi.Value)
      {
        if (node.CheckFile(path))
        {
          Log(MsgClass.BlueInfo, $"File match found. Using {fi.Key} as Spec.");
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
  public void Print (int indent)
  {

  }

  IEnumerable<string> IReadOnlyDictionary<string, Spec>.Keys => _specs.Keys;
  IEnumerable<Spec> IReadOnlyDictionary<string, Spec>.Values => _specs.Values;
  int IReadOnlyCollection<KeyValuePair<string, Spec>>.Count => _specs.Count;
}
