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

    if (WorkingCopy.Type.HasFlag(TokenRuleType.FromTokens))
      FromTokensCheck.Checked = true;

    if (WorkingCopy.Type.HasFlag(TokenRuleType.Mult))
      MultCheck.Checked = true;

    if (WorkingCopy.Type.HasFlag(TokenRuleType.Opt))
      OptCheck.Checked = true;
  }

  [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Nobody uses windows < 6.1")]
  private void SaveButton_Click (object sender, EventArgs e)
  {
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

  private void IgnoreCaseCheck_CheckedChanged (object sender, EventArgs e)
  {

  }
}
