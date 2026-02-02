#pragma warning disable CA1812 // Don't make unnecessary forms.

using System.Diagnostics.CodeAnalysis;

using Parser.Tokens;

using static Common.Names;

namespace FormObjectViewer;

internal sealed partial class RuleEditForm : Form
{

  [DSVA(DSV.Hidden)]
  [AllowNull]
  public TokenRule Original { get; set; }
  [DSVA(DSV.Hidden)]
  public required Spec Spec { get; init; }
  [AllowNull]
  [DSVA(DSV.Hidden)]
  private TokenRule WorkingCopy { get; set; }

  public RuleEditForm ()
  {
    InitializeComponent();
  }
  [MemberNotNull(nameof(Original))]
  private void CheckSetDefaults () => Original ??= new()
  {
    Type = TokenRuleType.None,
    RuleStringData = SE,
    TypeToAssign = SE
  };
  [MemberNotNull(nameof(WorkingCopy), nameof(Original))]
  private void RuleEditForm_Load (object sender, EventArgs e)
  {
    CheckSetDefaults();

    WorkingCopy = new()
    {
      Type = Original.Type,
      RuleStringData = Original.RuleStringData,
      TypeToAssign = Original.TypeToAssign,
    };

    UpdateForm();

  }
  [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Nobody uses windows < 6.1")]
  private void UpdateForm ()
  {
    StringDataBox.Text = WorkingCopy.RuleStringData;

    if (Spec.TokenType.IsAssignableTo(typeof(Enum)))
    {
      if (TypeToAssignListBox.Items.Count == 0)
        TypeToAssignListBox.Items.AddRange(Enum.GetNames(Spec.TokenType));

      if (WorkingCopy.TypeToAssign != SE)
        TypeToAssignListBox.SelectedItem = WorkingCopy.TypeToAssign;
    }

    FromTokensCheck.Checked = (WorkingCopy.Type | Spec.DefaultRuleSet).HasFlag(TokenRuleType.FromTokens);
    RecursiveCheck.Checked = (WorkingCopy.Type | Spec.DefaultRuleSet).HasFlag(TokenRuleType.Recursive);
    ExemptCheck.Checked = (WorkingCopy.Type | Spec.DefaultRuleSet).HasFlag(TokenRuleType.ExemptAllWithin);
    IgnoreCaseCheck.Checked = (WorkingCopy.Type | Spec.DefaultRuleSet).HasFlag(TokenRuleType.IgnoreCase);
    IgnoreTokenCheck.Checked = (WorkingCopy.Type | Spec.DefaultRuleSet).HasFlag(TokenRuleType.IgnoredToken);
    MultCheck.Checked = (WorkingCopy.Type | Spec.DefaultRuleSet).HasFlag(TokenRuleType.Mult);
    OptCheck.Checked = (WorkingCopy.Type | Spec.DefaultRuleSet).HasFlag(TokenRuleType.Opt);
  }
  private void UpdateCopy ()
  {
    WorkingCopy.RuleStringData = StringDataBox.Text;
    if (TypeToAssignListBox.SelectedIndex != -1 && TypeToAssignListBox.Items.Count > 1)
    {
      WorkingCopy.TypeToAssign = TypeToAssignListBox.SelectedItem?.ToString() ?? SE;
    }
    TokenRuleType assembled = TokenRuleType.None;

    assembled |= FromTokensCheck.Checked ? TokenRuleType.FromTokens : TokenRuleType.None;
    assembled |= RecursiveCheck.Checked ? TokenRuleType.Recursive : TokenRuleType.None;
    assembled |= ExemptCheck.Checked ? TokenRuleType.ExemptAllWithin : TokenRuleType.None;
    assembled |= IgnoreCaseCheck.Checked ? TokenRuleType.IgnoreCase : TokenRuleType.None;
    assembled |= MultCheck.Checked ? TokenRuleType.Mult : TokenRuleType.None;
    assembled |= OptCheck.Checked ? TokenRuleType.Opt : TokenRuleType.None;
    assembled |= IgnoreTokenCheck.Checked ? TokenRuleType.IgnoredToken : TokenRuleType.None;

    assembled |= TokenMatchRadio.Checked ? TokenRuleType.TokenMatch : TokenRuleType.None;
    assembled |= TokenExactRadio.Checked ? TokenRuleType.TokenExact : TokenRuleType.None;
    assembled |= TokenExtractRadio.Checked ? TokenRuleType.TokenExtract : TokenRuleType.None;
    assembled |= SplitMatchRadio.Checked ? TokenRuleType.SplitMatch : TokenRuleType.None;
    assembled |= SplitExactRadio.Checked ? TokenRuleType.SplitExact : TokenRuleType.None;
    assembled |= StoreOtherRadio.Checked ? TokenRuleType.StoreOther : TokenRuleType.None;
    assembled |= StoreExtraRadio.Checked ? TokenRuleType.StoreExtra : TokenRuleType.None;
    assembled |= ErrorMatchRadio.Checked ? TokenRuleType.ErrorMatch : TokenRuleType.None;
    assembled |= CompetitiveRadio.Checked ? TokenRuleType.Competitive : TokenRuleType.None;

    WorkingCopy.Type = assembled;

  }
  [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Nobody uses windows < 6.1")]
  private void SaveButton_Click (object sender, EventArgs e)
  {
    UpdateCopy();
    Original.RuleStringData = WorkingCopy.RuleStringData;
    Original.Type = WorkingCopy.Type;
    Original.TypeToAssign = WorkingCopy.TypeToAssign;
    Close();
  }

  [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Nobody uses windows < 6.1")]
  private void CancelButton_Click (object sender, EventArgs e)
  {
    Close();
  }
}
