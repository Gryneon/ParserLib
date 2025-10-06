#pragma warning disable IDE0072 // Add missing cases

using OP = Parser.OpStatus;

namespace Parser.V2;

public interface ISpecHandler<out TBaseOperation, in TParseItem, TSpec, TWorkStorage, TFileStorage>
  where TBaseOperation : IOperation
  where TSpec : Spec
  where TParseItem : class
  where TWorkStorage : IWorkStorage<TParseItem>
  where TFileStorage : IFileStorage
{
  TWorkStorage Work { get; }
  TFileStorage Files { get; }
  TSpec? Spec { get; }

  void AddFile (string file_name);
  void AddData (TParseItem data_content);
  void AddData (string data_content);
  void DoWork_Files ();
  void DoWork_Initial ();
  void DoWork_Section (int section_num);
  void DoWork_Final ();
  void Complete ();
#if ALLOW_WORKITEM
  WorkItem GetResults (int doc_index);
  WorkList GetResults ();
#endif
}

public class ParseShell : ISpecHandler<IOperation, string, Spec, TextWorkStorage, FileStorage>
{
  public FileStorage Files { get; }
  public TextWorkStorage Work { get; }
  protected ParserStatus CalculateStatus ()
  {
    ParserStatus status = ParserStatus.Unknown;

    status |= Files.Count > 0 ? ParserStatus.Files_Present : 0;
    status |= Work.Count > 0 ? ParserStatus.Data_Present : 0;
    status |= Operations.Count > 0 ? ParserStatus.Ops_Loaded : 0;
    status |= Spec is not null ? ParserStatus.Spec_Loaded : 0;
    status |= LastStatus < OP.Fail ? ParserStatus.Spec_Loaded : 0;

    return status;
  }
  public ParserStatus Status => CalculateStatus();
  public Spec? Spec { get; }
  //public required Collection<OpSection<object, object>> Sections { get; init; }
  public Collection<IOperation> Operations => Spec?.Operations ?? [];
  public IOperation CurrentOp { get; protected set; }
  public string ParseItem { set => throw new NotImplementedException(); }
  public OP LastStatus { get; }

  public ParseShell (string? initial_input = null, IEnumerable<string>? file_names = null, Spec? given_spec = null)
  {
    Files = new(file_names ?? []);
    Work = initial_input is null ? new() : new(initial_input);
    Spec = given_spec;
    CurrentOp = Operations.First();
  }

  public void AddFile (string file_name) => Files.Add(file_name);
  public void AddWork (string content) => Work.Add(content);
  public void Complete () => throw new NotImplementedException();
  public void DoWork_Files ()
  {
    if (Status.HasFlag(ParserStatus.Files_Present))
    {
      do
      {
        string? temp = Files.PopNext();

        if (temp != null)
          _ = Work.Add(temp);
      } while (Files.Count > 0);
    }
  }
  public void DoWork_Final () => throw new NotImplementedException();
  public void DoWork_Initial ()
  {
    if (Status.HasFlag(ParserStatus.Files_Present))
    {
      foreach (string file in Files.Files)
      {
        AddWork(File.ReadAllText(file));
      }
    }
  }
  public void DoWork_Section (int section_num) => throw new NotImplementedException();
  public void AddData (string data_content) => throw new NotImplementedException();
  //public WorkItem GetResults (int doc_index) => throw new NotImplementedException();
  //public WorkList GetResults () => throw new NotImplementedException();
}
