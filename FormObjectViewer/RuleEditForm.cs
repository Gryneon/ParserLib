using System.Diagnostics.CodeAnalysis;

using Parser.Tokens;

using static Common.Names;

namespace FormObjectViewer;

internal partial class RuleEditForm : Form
{

  [DSVA(DSV.Hidden)]
  public TokenRule? Original { get; set; }
  private TokenRule? WorkingCopy { get; set; }

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
  [MemberNotNull(nameof(WorkingCopy))]
  private void RuleEditForm_Load (object sender, EventArgs e)
  {
    CheckSetDefaults();

    WorkingCopy = new()
    {
      Type = Original.Type,
      RuleStringData = Original.RuleStringData,
      TypeToAssign = Original.TypeToAssign,
    };


  }
}
