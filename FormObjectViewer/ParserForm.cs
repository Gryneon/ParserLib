#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1812 // Remove unused classes

using System.IO;
using System.Linq;

using Common;
using Common.Extensions;

using Parser.Tokens;

using Specification.XML;

using static Common.Names;

namespace FormObjectViewer;

internal sealed partial class ParserForm : Form
{
  private object? ParseFileContent { get; set; } = SE;
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
  public BindingList<string> TokenRules { get; } = [
      "TokenMatch",
      "TokenExact",
      "SplitMatch",
      "SplitExact",
      "ErrorMatch",
      "TokenExtract",
      "StoreExtra",
      "StoreOther",
    ];
  private Spec LoadedSpec => SpecComboBox.SelectedIndex >= 0 ? SpecList[SpecComboBox.SelectedIndex] : DefaultSpec.Unknown;
  private BindingList<TokenRule> WorkingRules { get; set; } = [];
  private BindingList<IToken> CurrentTokens { get; set; } = [];
  private string ParseFile { get; set; } = "";
  private TokenFactory? Factory { get; set; }

  public ParserForm () => InitializeComponent();

  private void ParserForm_Load (object sender, EventArgs e)
  {
    TokenDataGrid.DataSource = CurrentTokens;
  }

  private void OpenParseFileDialog_FileOk (object sender, CancelEventArgs e)
  {
    ParsePathTextBox.Text = OpenParseFileDialog.FileName;
    ParsePathTextBox.SelectAll();
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
      DialogResult ask = MessageBox.Show(this, "Rules have been modified.", "Save changes?", MessageBoxButtons.YesNoCancel);
      if (ask == DialogResult.Yes)
      {
        //TODO: Save File Dialog;
      }
      else if (ask == DialogResult.No)
      {
        Close();
      }
    }
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
      //TODO: Maybe need something here?
    }
  }

  private void ParseButton_Click (object sender, EventArgs e)
  {
    CurrentTokens.Clear();
    TokenDataGrid.Refresh();
    ParseFile = ParsePathTextBox.Text;

    if (!File.Exists(ParseFile))
    {
      _ = MessageBox.Show(this, "Parse file not found.", "Please correct any mistakes in the path name.");
      return;
    }
    string? contents;
    try
    {
      contents = File.ReadAllText(ParseFile);
      ParseFileContent = contents;
    }
    catch (IOException)
    {
      _ = MessageBox.Show(this, "Parse file not loaded.", "Please investigate the access to this file.");
      return;
    }

    Factory = new(WorkingRules, LoadedSpec);

    foreach (IToken token in Factory.Produce(contents))
    {
      CurrentTokens.Add(token);
    }
    TokenDataGrid.Refresh();
  }

  private void TokenRuleDataGrid_RowValidated (object sender, DataGridViewCellEventArgs e)
  {
    TokenRuleCountLabel.Text = $"{WorkingRules.Count}";
  }

  private void ClearTokensButton_Click (object sender, EventArgs e)
  {
    CurrentTokens.Clear();
    TokenDataGrid.Refresh();
  }

  private void LoadParseFileButton_Click (object sender, EventArgs e)
  {
    _ = OpenParseFileDialog.ShowDialog();
  }

  private void ClearRulesButton_Click (object sender, EventArgs e)
  {
    WorkingRules.Clear();
    TokenRuleDataGrid.Refresh();
  }

  private void ShowUnparsedButton_Click (object sender, EventArgs e)
  {
    //TODO: Add Feature (Interactive visual display of everything the tokenizer missed.)
  }

  private void SaveRuleButton_Click (object sender, EventArgs e)
  {
    int counter = 0;
    XMLString maker = new();
    maker.AddElementOpen("RuleSet");
    foreach (TokenRule rule in WorkingRules)
    {

      maker.AddElementOpen("Rule", [new PropertyBase<string>() { Key = "index", Value = $"{counter++}" }]);
      maker.AddLineFeed();
      maker.AddElementOpen("Type");
      maker.AddContent($"{rule.Type}");
      maker.CloseLastElement();
      maker.AddLineFeed();
      maker.AddElementOpen("TypeToAssign");
      maker.AddContent($"{rule.TypeToAssign}");
      maker.CloseLastElement();
      maker.AddLineFeed();
      maker.AddElementOpen("Data");
      maker.AddContent($"{rule.RuleStringData?.XMLEscape()}");
      maker.CloseLastElement();
      maker.AddLineFeed();
      maker.CloseLastElement();
      maker.AddLineFeed();
    }
    maker.CloseLastElement();

    string doc = maker.Serialize();
  }
}
