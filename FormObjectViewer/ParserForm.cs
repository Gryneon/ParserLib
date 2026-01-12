#pragma warning disable CA1416 // Validate platform compatibility

using System.Collections.Generic;
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

  private void LoadSpecMenuItem_Click (object sender, EventArgs e)
  {
    //TODO: pull from Library class
    List<Spec> specs = [
      Specification.ACS.Definition.ACS,
      Specification.ACS.Definition.ModelDef,
      Specification.Decorate.Definition.Spec,
      Specification.INI.Definition.Spec,
      Specification.IPL.Definition.Spec,
      Specification.JSON.Definition.Spec,
      Specification.MapInfo.Definition.Spec,
      Specification.REG.Definition.Spec,
      Specification.SndInfo.Definition.Spec,
      Specification.UDMF.Definition.Spec,
      Specification.XML.Definition.Spec,
      Specification.ZScript.Definition.Spec,
    ];

    SpecComboBox.DataSource = specs;
  }
}
