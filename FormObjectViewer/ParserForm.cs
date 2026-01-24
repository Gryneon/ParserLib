#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1812 // Remove unused classes

using System.Linq;

using Parser.Tokens;

namespace FormObjectViewer;

internal sealed partial class ParserForm : Form
{
  private Spec Spec { get; set; } = DefaultSpec.Unknown;
  private bool ItemsChanged { get; set; }
  private List<Spec> SpecList { get; } = [
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
  private Spec LoadedSpec => SpecList[SpecComboBox.SelectedIndex];
  private BindingList<TokenRule> WorkingRules { get; set; } = [];
  private string ParseFile { get; set; } = "";
  private TokenFactory? Factory { get; set; }

  public ParserForm () => InitializeComponent();

  private void ParserForm_Load (object sender, EventArgs e)
  {

  }

  private void OpenParseFileDialog_FileOk (object sender, CancelEventArgs e)
  {
    string path = OpenParseFileDialog.FileName;
    string? spec_str = Library.CheckFile(path);

    if (spec_str is null) { /*TODO: Open dialog to ask what kind of file. store info in xml.*/ }

    Spec = Library.LookupOrDefault(spec_str);
  }

  private void LoadSpecMenuItem_Click (object sender, EventArgs e)
  {

    List<string> specNames = [.. SpecList.Select(i => i.Name)];

    SpecComboBox.DataSource = specNames;
  }

  private void ExitMenuItem_Click (object sender, EventArgs e)
  {
    if (ItemsChanged)
    {
      //TODO: Prompt to save work.
    }
    Close();
  }

  private void LoadRulesButton_Click (object sender, EventArgs e)
  {
    TokenRuleDataGrid.DataSource = WorkingRules;
    TokenRuleBindingSource.DataSource = WorkingRules;

    foreach (TokenRule rule in LoadedSpec.TokenRules)
    {
      WorkingRules.Add(rule);
    }
  }

  private void OpenFileMenuItem_Click (object sender, EventArgs e)
  {
    DialogResult dialog = OpenParseFileDialog.ShowDialog();
    if (dialog == DialogResult.OK)
    {
      ParseFile = OpenParseFileDialog.FileName;
      Factory = new(WorkingRules, Spec);
    }
  }

  private void Button1_Click (object sender, EventArgs e)
  {

  }
}
