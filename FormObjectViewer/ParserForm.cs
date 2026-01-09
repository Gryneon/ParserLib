#pragma warning disable CA1416 // Validate platform compatibility

using System.Windows.Forms;

namespace FormObjectViewer;

internal sealed partial class ParserForm : Form
{
  private XParser Parser { get; set; } = new();
  private Spec Spec { get; set; } = DefaultSpec.Unknown;

  public ParserForm ()
  {
    InitializeComponent();
  }

  private void ParserForm_Load (object sender, EventArgs e)
  {

  }

  private void OpenParseFileDialog_FileOk (object sender, System.ComponentModel.CancelEventArgs e)
  {
    string path = OpenParseFileDialog.FileName;
    string? spec_str = Library.CheckFile(path);

    if (spec_str is null) { /*TODO: Open dialog to ask what kind of file. store info in xml.*/ }

    Spec = Library.LookupOrDefault(spec_str);
    Parser = new(Spec);

  }
}
