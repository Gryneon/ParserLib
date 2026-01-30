#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1812 // Remove unused classes

using System.IO;
using System.Linq;

using Common;
using Common.Extensions;

using Parser.Tokens;

using static Common.Names;

using XMLMaker = Specification.XML.XMLString;

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
  private readonly BindingList<TokenRule> _workingRules = [];
  private readonly BindingList<IToken> _currentTokens = [];
  private string _parseFile = "";
  private TokenFactory? _factory = new();

  public ParserForm () => InitializeComponent();

  private void ParserForm_Load (object sender, EventArgs e)
  {
    TokenDataGrid.DataSource = _currentTokens;
  }

  private void OpenParseFileDialog_FileOk (object sender, CancelEventArgs e)
  {
    ParsePathTextBox.Text = OpenParseFileDialog.FileName;
    ParsePathTextBox.SelectAll();
  }

  private void LoadSpecList (object sender, EventArgs e)
  {
    List<string> specNames = [.. SpecList.Select(i => i.Name)];
    SpecComboBox.DataSource = specNames;
    LoadParseFileButton.Enabled = true;
    LoadSpecButton.Enabled = true;
  }

  private void Exit (object sender, EventArgs e)
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

  private void LoadRules (object sender, EventArgs e)
  {
    TokenRuleDataGrid.DataSource = _workingRules;
    TokenRuleBindingSource.DataSource = _workingRules;

    foreach (TokenRule rule in LoadedSpec.TokenRules)
    {
      _workingRules.Add(rule);
    }
  }
  private void SaveRuleDialog (object sender, EventArgs e)
  {
    //DialogResult dialog = SaveRuleFileDialog.ShowDialog();
    //if (dialog == DialogResult.OK)
    {
      //TODO: Maybe need something here?
    }
  }

  private void OpenParseFile (object sender, EventArgs e)
  {
    DialogResult dialog = OpenParseFileDialog.ShowDialog();
    if (dialog == DialogResult.OK)
    {
      //TODO: Maybe need something here?
    }
  }

  private void ExecuteParse (object sender, EventArgs e)
  {
    _currentTokens.Clear();
    TokenDataGrid.Refresh();
    _parseFile = ParsePathTextBox.Text;

    if (!File.Exists(_parseFile))
    {
      _ = MessageBox.Show(this, "Parse file not found.", "Please correct any mistakes in the path name.");
      return;
    }
    string? contents;
    try
    {
      contents = File.ReadAllText(_parseFile);
      ParseFileContent = contents;
    }
    catch (IOException)
    {
      _ = MessageBox.Show(this, "Parse file not loaded.", "Please investigate the access to this file.");
      return;
    }

    _factory = new(_workingRules, LoadedSpec);

    foreach (IToken token in _factory.Produce(contents))
    {
      _currentTokens.Add(token);
    }
    TokenDataGrid.Refresh();
  }

  private void TokenRuleDataGrid_RowValidated (object sender, DataGridViewCellEventArgs e)
  {
    TokenRuleCountLabel.Text = $"{_workingRules.Count}";
  }

  private void ClearTokens (object sender, EventArgs e)
  {
    _currentTokens.Clear();
    TokenDataGrid.Refresh();
  }

  private void ClearRules (object sender, EventArgs e)
  {
    _workingRules.Clear();
    TokenRuleDataGrid.Refresh();
  }

  private void ShowUnparsed (object sender, EventArgs e)
  {
    UnparsedViewer viewer = new()
    {
      Sections = _factory?.CannotMatch ?? []
    };
    _ = viewer.ShowDialog();
    viewer.Dispose();
  }

  private void SaveRule (object sender, EventArgs e)
  {
    int counter = 0;
    XMLMaker maker = new();
    maker.AddElementOpen("RuleSet");
    maker.AddLineFeed();
    foreach (TokenRule rule in _workingRules)
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

  private void TokenRuleDataGrid_DataError (object sender, DataGridViewDataErrorEventArgs e)
  {
    TokenRuleDataGrid[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Pink;
  }
}
