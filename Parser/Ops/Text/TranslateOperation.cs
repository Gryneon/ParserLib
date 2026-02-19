#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using IndexedConstruct = (int Index, Parser.Ops.Text.TranslationConstruct Construct);
using TT = Parser.Ops.Text.TranslatorType;

namespace Parser.Ops.Text;

public enum TranslatorType
{
  None,
  Name,
  Body,

  Include,
  Exclude,
  Split,
  Replace,
}

public class TranslationRule
{
  public TT Type { get; set; }
  public object? Data { get; init; }

  private Func<XParser, string, object> Task => Type switch
  {
    TT.None => (p, s) => s,
    TT.Include when Data is string restrict => (parser, s) => new string([.. s.Where(c => restrict.Contains(c, SCO))]),
    TT.Exclude when Data is string restrict => (parser, s) => new string([.. s.Where(c => !restrict.Contains(c, SCO))]),
    TT.Split when Data is string regex => (parser, s) => Regex.Split(s, regex, parser.Spec.RxOpt),
    TT.Replace when Data is ReplaceNode rn => (parser, s) => rn.ReplaceRegex(s, Spec.Active.RxOpt),
    _ => throw new NotImplementedException(),
  };

  public object Execute (XParser parser, string s) => Task(parser, s);
}

public class TranslationConstruct
{
  [MaybeNull] public required string Name { get; init; }
  [MaybeNull] public required string Body { get; init; }
  public TT RegexName { get; init; }
  public TT RegexBody { get; init; }

  [AllowNull] public required string Translation { get; init; }

  public static TranslationConstruct Null { get; } = new();

  [SetsRequiredMembers]
  private TranslationConstruct () { }
}

public class TranslatedNodeCollection<T> : ICollection<TranslatedNode>
{
  private readonly Collection<TranslatedNode> _nodes = [];

  public int Count => _nodes.Count;

  public bool IsReadOnly => false;

  public void Add (TranslatedNode item) => _nodes.Add(item);
  public void Clear () => _nodes.Clear();
  public bool Contains (TranslatedNode item) => _nodes.Contains(item);
  public void CopyTo (TranslatedNode[] array, int arrayIndex) => _nodes.CopyTo(array, arrayIndex);
  public IEnumerator<TranslatedNode> GetEnumerator () => _nodes.GetEnumerator();
  public bool Remove (TranslatedNode item) => _nodes.Remove(item);
  IEnumerator IEnumerable.GetEnumerator () => ((IEnumerable) _nodes).GetEnumerator();
}

public class TranslatedNode
{
  public required string Construct { get; init; }
  public required string Value { get; init; }
  public required string Regex { get; init; }
  public int Position { get; init; }
}

public class TranslateOperation (
  string input_key,
  string output_key,
  ICollection<TranslationConstruct> constructs,
  ICollection<TranslationRule> rules
  ) : Operation(input_key, output_key)
{
  private readonly ICollection<TranslationConstruct> _constructs = constructs;
  private readonly ICollection<TranslationRule> _rules = rules;

  public string TranslateReplace (string s)
  {
    s ??= SE;
    IEnumerable<TranslationRule>? replaces = _rules.Where(r => r.Type is TT.Replace);
    foreach (TranslationRule rule in replaces)
    {
      if (rule.Data is ReplaceNode rn)
        s = rn.ReplaceRegex(s, Spec.RxOpt);
    }
    return s;
  }

  public string TranslateExclude (string s)
  {
    s ??= SE;
    IEnumerable<TranslationRule>? exclude = _rules.Where(r => r.Type is TT.Exclude);
    foreach (TranslationRule rule in exclude)
    {
      if (rule.Data is string remove)
        s = s.Replace(remove, SE, SCO);
      else if (rule.Data is ReplaceNode rn)
        s = s.Replace(rn);
      else if (rule.Data is ReplaceNodes rns)
        foreach (ReplaceNode node in rns)
          s = s.Replace(node);
    }
    return s;
  }

  public RxS TranslateSplit (string s)
  {
    s ??= SE;
    RxS assembly = SE;
    IList<IndexedConstruct> pos;
    TranslationRule? splitter;
    Collection<string>? split_string = null;

    try { splitter = _rules.Single(r => r.Type is TT.Split); }
    catch (InvalidOperationException) { splitter = null; }

    if (splitter is not null)
    {
      split_string = splitter.Execute(Parser, s).AsCollection<string>();
      split_string = [.. split_string.Select(TranslateReplace)];
      split_string = [.. split_string.Select(TranslateExclude)];
    }
    do
    {
      pos = [.. _constructs.Select(x =>
      {
        Match match = Regex.Match(s, x.Translation);
        return match.Success ? (match.Index, x) : (-1, TranslationConstruct.Null);
      })];
      pos = [.. pos.Where(x => x.Index != -1).OrderBy(x => x.Index)];
      assembly += pos[0].Construct.Translation ?? SE;
    } while (pos.Count > 0);
    return assembly;
  }

  protected override void Execute ()
  {
    if (CheckInput(out string? casted))
    {
      try
      {
        WorkToReturn = TranslateSplit(casted);
        Status = OpStatus.Pass;
      }
      catch (InvalidOperationException)
      {
        Status = OpStatus.FailBadOpDefinition;
        return;
      }
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }
}
