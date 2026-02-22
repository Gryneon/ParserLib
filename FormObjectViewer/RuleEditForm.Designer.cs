namespace FormObjectViewer;

partial class RuleEditForm
{
  /// <summary>Required designer variable.</summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose (bool disposing)
  {
    if (disposing && (components != null))
    {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent ()
  {
    components = new Container();
    CompetitiveRadio = new RadioButton();
    TokenMatchRadio = new RadioButton();
    TokenExactRadio = new RadioButton();
    SplitMatchRadio = new RadioButton();
    SplitExactRadio = new RadioButton();
    TokenTypeGroup = new GroupBox();
    StoreOtherRadio = new RadioButton();
    StoreExtraRadio = new RadioButton();
    ErrorMatchRadio = new RadioButton();
    TokenExtractRadio = new RadioButton();
    TypeToAssignListBox = new ListBox();
    TypeToAssignLabel = new Label();
    TheCancelButton = new Button();
    SaveButton = new Button();
    IgnoreCaseCheck = new CheckBox();
    IgnoreTokenCheck = new CheckBox();
    FromTokensCheck = new CheckBox();
    RecursiveCheck = new CheckBox();
    OptCheck = new CheckBox();
    MultCheck = new CheckBox();
    AddTypeButton = new Button();
    StringDataBox = new TextBox();
    StringDataContextMenu = new ContextMenuStrip(components);
    SelectAllContextMenuItem = new ToolStripMenuItem();
    StringDataLabel = new Label();
    AddTypeBox = new TextBox();
    RuleFlagsLabel = new Label();
    ExemptCheck = new CheckBox();
    TokenTypeGroup.SuspendLayout();
    StringDataContextMenu.SuspendLayout();
    SuspendLayout();
    // 
    // CompetitiveRadio
    // 
    CompetitiveRadio.AutoSize = true;
    CompetitiveRadio.Location = new Point(8, 32);
    CompetitiveRadio.Name = "CompetitiveRadio";
    CompetitiveRadio.Size = new Size(95, 21);
    CompetitiveRadio.TabIndex = 0;
    CompetitiveRadio.TabStop = true;
    CompetitiveRadio.Text = "Competitive";
    CompetitiveRadio.UseVisualStyleBackColor = true;
    // 
    // TokenMatchRadio
    // 
    TokenMatchRadio.AutoSize = true;
    TokenMatchRadio.Location = new Point(8, 59);
    TokenMatchRadio.Name = "TokenMatchRadio";
    TokenMatchRadio.Size = new Size(100, 21);
    TokenMatchRadio.TabIndex = 0;
    TokenMatchRadio.TabStop = true;
    TokenMatchRadio.Text = "Token Match";
    TokenMatchRadio.UseVisualStyleBackColor = true;
    // 
    // TokenExactRadio
    // 
    TokenExactRadio.AutoSize = true;
    TokenExactRadio.Location = new Point(8, 87);
    TokenExactRadio.Name = "TokenExactRadio";
    TokenExactRadio.Size = new Size(94, 21);
    TokenExactRadio.TabIndex = 0;
    TokenExactRadio.TabStop = true;
    TokenExactRadio.Text = "Token Exact";
    TokenExactRadio.UseVisualStyleBackColor = true;
    // 
    // SplitMatchRadio
    // 
    SplitMatchRadio.AutoSize = true;
    SplitMatchRadio.Location = new Point(8, 114);
    SplitMatchRadio.Name = "SplitMatchRadio";
    SplitMatchRadio.Size = new Size(91, 21);
    SplitMatchRadio.TabIndex = 0;
    SplitMatchRadio.TabStop = true;
    SplitMatchRadio.Text = "Split Match";
    SplitMatchRadio.UseVisualStyleBackColor = true;
    // 
    // SplitExactRadio
    // 
    SplitExactRadio.AutoSize = true;
    SplitExactRadio.Location = new Point(8, 141);
    SplitExactRadio.Name = "SplitExactRadio";
    SplitExactRadio.Size = new Size(85, 21);
    SplitExactRadio.TabIndex = 0;
    SplitExactRadio.TabStop = true;
    SplitExactRadio.Text = "Split Exact";
    SplitExactRadio.UseVisualStyleBackColor = true;
    // 
    // TokenTypeGroup
    // 
    TokenTypeGroup.Controls.Add(CompetitiveRadio);
    TokenTypeGroup.Controls.Add(StoreOtherRadio);
    TokenTypeGroup.Controls.Add(StoreExtraRadio);
    TokenTypeGroup.Controls.Add(ErrorMatchRadio);
    TokenTypeGroup.Controls.Add(TokenExtractRadio);
    TokenTypeGroup.Controls.Add(SplitExactRadio);
    TokenTypeGroup.Controls.Add(TokenMatchRadio);
    TokenTypeGroup.Controls.Add(SplitMatchRadio);
    TokenTypeGroup.Controls.Add(TokenExactRadio);
    TokenTypeGroup.Location = new Point(8, 9);
    TokenTypeGroup.Name = "TokenTypeGroup";
    TokenTypeGroup.Size = new Size(128, 281);
    TokenTypeGroup.TabIndex = 1;
    TokenTypeGroup.TabStop = false;
    TokenTypeGroup.Text = "Token Rule Type";
    // 
    // StoreOtherRadio
    // 
    StoreOtherRadio.AutoSize = true;
    StoreOtherRadio.Location = new Point(8, 250);
    StoreOtherRadio.Name = "StoreOtherRadio";
    StoreOtherRadio.Size = new Size(94, 21);
    StoreOtherRadio.TabIndex = 0;
    StoreOtherRadio.TabStop = true;
    StoreOtherRadio.Text = "Store Other";
    StoreOtherRadio.UseVisualStyleBackColor = true;
    // 
    // StoreExtraRadio
    // 
    StoreExtraRadio.AutoSize = true;
    StoreExtraRadio.Location = new Point(8, 223);
    StoreExtraRadio.Name = "StoreExtraRadio";
    StoreExtraRadio.Size = new Size(90, 21);
    StoreExtraRadio.TabIndex = 0;
    StoreExtraRadio.TabStop = true;
    StoreExtraRadio.Text = "Store Extra";
    StoreExtraRadio.UseVisualStyleBackColor = true;
    // 
    // ErrorMatchRadio
    // 
    ErrorMatchRadio.AutoSize = true;
    ErrorMatchRadio.Location = new Point(8, 195);
    ErrorMatchRadio.Name = "ErrorMatchRadio";
    ErrorMatchRadio.Size = new Size(96, 21);
    ErrorMatchRadio.TabIndex = 0;
    ErrorMatchRadio.TabStop = true;
    ErrorMatchRadio.Text = "Error Match";
    ErrorMatchRadio.UseVisualStyleBackColor = true;
    // 
    // TokenExtractRadio
    // 
    TokenExtractRadio.AutoSize = true;
    TokenExtractRadio.Location = new Point(8, 168);
    TokenExtractRadio.Name = "TokenExtractRadio";
    TokenExtractRadio.Size = new Size(103, 21);
    TokenExtractRadio.TabIndex = 0;
    TokenExtractRadio.TabStop = true;
    TokenExtractRadio.Text = "Token Extract";
    TokenExtractRadio.UseVisualStyleBackColor = true;
    // 
    // TypeToAssignListBox
    // 
    TypeToAssignListBox.Font = new Font("Segoe Fluent Icons", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
    TypeToAssignListBox.FormattingEnabled = true;
    TypeToAssignListBox.IntegralHeight = false;
    TypeToAssignListBox.Location = new Point(144, 40);
    TypeToAssignListBox.Name = "TypeToAssignListBox";
    TypeToAssignListBox.Size = new Size(120, 168);
    TypeToAssignListBox.TabIndex = 2;
    // 
    // TypeToAssignLabel
    // 
    TypeToAssignLabel.AutoSize = true;
    TypeToAssignLabel.Location = new Point(144, 16);
    TypeToAssignLabel.Name = "TypeToAssignLabel";
    TypeToAssignLabel.Size = new Size(93, 17);
    TypeToAssignLabel.TabIndex = 3;
    TypeToAssignLabel.Text = "Type to Assign";
    // 
    // TheCancelButton
    // 
    TheCancelButton.Location = new Point(600, 248);
    TheCancelButton.Name = "TheCancelButton";
    TheCancelButton.Size = new Size(64, 36);
    TheCancelButton.TabIndex = 4;
    TheCancelButton.Text = "Cancel";
    TheCancelButton.UseVisualStyleBackColor = true;
    TheCancelButton.Click += CancelButton_Click;
    // 
    // SaveButton
    // 
    SaveButton.Location = new Point(400, 248);
    SaveButton.Name = "SaveButton";
    SaveButton.Size = new Size(64, 36);
    SaveButton.TabIndex = 4;
    SaveButton.Text = "Save";
    SaveButton.UseVisualStyleBackColor = true;
    SaveButton.Click += SaveButton_Click;
    // 
    // IgnoreCaseCheck
    // 
    IgnoreCaseCheck.AutoSize = true;
    IgnoreCaseCheck.Location = new Point(288, 56);
    IgnoreCaseCheck.Name = "IgnoreCaseCheck";
    IgnoreCaseCheck.Size = new Size(97, 21);
    IgnoreCaseCheck.TabIndex = 5;
    IgnoreCaseCheck.Text = "Ignore Case";
    IgnoreCaseCheck.UseVisualStyleBackColor = true;
    // 
    // IgnoreTokenCheck
    // 
    IgnoreTokenCheck.AutoSize = true;
    IgnoreTokenCheck.Location = new Point(288, 80);
    IgnoreTokenCheck.Name = "IgnoreTokenCheck";
    IgnoreTokenCheck.Size = new Size(111, 21);
    IgnoreTokenCheck.TabIndex = 5;
    IgnoreTokenCheck.Text = "Ignored Token";
    IgnoreTokenCheck.UseVisualStyleBackColor = true;
    // 
    // FromTokensCheck
    // 
    FromTokensCheck.AutoSize = true;
    FromTokensCheck.Location = new Point(288, 104);
    FromTokensCheck.Name = "FromTokensCheck";
    FromTokensCheck.Size = new Size(101, 21);
    FromTokensCheck.TabIndex = 5;
    FromTokensCheck.Text = "From Tokens";
    FromTokensCheck.UseVisualStyleBackColor = true;
    // 
    // RecursiveCheck
    // 
    RecursiveCheck.AutoSize = true;
    RecursiveCheck.Location = new Point(288, 128);
    RecursiveCheck.Name = "RecursiveCheck";
    RecursiveCheck.Size = new Size(82, 21);
    RecursiveCheck.TabIndex = 5;
    RecursiveCheck.Text = "Recursive";
    RecursiveCheck.UseVisualStyleBackColor = true;
    // 
    // OptCheck
    // 
    OptCheck.AutoSize = true;
    OptCheck.Location = new Point(288, 152);
    OptCheck.Name = "OptCheck";
    OptCheck.Size = new Size(77, 21);
    OptCheck.TabIndex = 5;
    OptCheck.Text = "Optional";
    OptCheck.UseVisualStyleBackColor = true;
    // 
    // MultCheck
    // 
    MultCheck.AutoSize = true;
    MultCheck.Location = new Point(288, 176);
    MultCheck.Name = "MultCheck";
    MultCheck.Size = new Size(104, 21);
    MultCheck.TabIndex = 5;
    MultCheck.Text = "One or Many";
    MultCheck.UseVisualStyleBackColor = true;
    // 
    // AddTypeButton
    // 
    AddTypeButton.Location = new Point(144, 248);
    AddTypeButton.Name = "AddTypeButton";
    AddTypeButton.Size = new Size(120, 32);
    AddTypeButton.TabIndex = 6;
    AddTypeButton.Text = "Add Type";
    AddTypeButton.UseVisualStyleBackColor = true;
    AddTypeButton.Click += AddTypeButton_Click;
    // 
    // StringDataBox
    // 
    StringDataBox.AcceptsReturn = true;
    StringDataBox.AcceptsTab = true;
    StringDataBox.AllowDrop = true;
    StringDataBox.BorderStyle = BorderStyle.FixedSingle;
    StringDataBox.ContextMenuStrip = StringDataContextMenu;
    StringDataBox.Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold, GraphicsUnit.Point,  0);
    StringDataBox.Location = new Point(400, 40);
    StringDataBox.Multiline = true;
    StringDataBox.Name = "StringDataBox";
    StringDataBox.ScrollBars = ScrollBars.Both;
    StringDataBox.Size = new Size(264, 200);
    StringDataBox.TabIndex = 7;
    // 
    // StringDataContextMenu
    // 
    StringDataContextMenu.Items.AddRange(new ToolStripItem[] { SelectAllContextMenuItem });
    StringDataContextMenu.Name = "StringDataContextMenu";
    StringDataContextMenu.Size = new Size(174, 26);
    // 
    // SelectAllContextMenuItem
    // 
    SelectAllContextMenuItem.Name = "SelectAllContextMenuItem";
    SelectAllContextMenuItem.ShortcutKeys =  Keys.Control | Keys.A;
    SelectAllContextMenuItem.Size = new Size(173, 22);
    SelectAllContextMenuItem.Text = "Select All";
    // 
    // StringDataLabel
    // 
    StringDataLabel.AutoSize = true;
    StringDataLabel.Location = new Point(400, 16);
    StringDataLabel.Name = "StringDataLabel";
    StringDataLabel.Size = new Size(73, 17);
    StringDataLabel.TabIndex = 3;
    StringDataLabel.Text = "String Data";
    // 
    // AddTypeBox
    // 
    AddTypeBox.Location = new Point(144, 216);
    AddTypeBox.Name = "AddTypeBox";
    AddTypeBox.Size = new Size(120, 25);
    AddTypeBox.TabIndex = 8;
    // 
    // RuleFlagsLabel
    // 
    RuleFlagsLabel.AutoSize = true;
    RuleFlagsLabel.Location = new Point(280, 16);
    RuleFlagsLabel.Name = "RuleFlagsLabel";
    RuleFlagsLabel.Size = new Size(67, 17);
    RuleFlagsLabel.TabIndex = 3;
    RuleFlagsLabel.Text = "Rule Flags";
    // 
    // ExemptCheck
    // 
    ExemptCheck.AutoSize = true;
    ExemptCheck.Location = new Point(288, 200);
    ExemptCheck.Name = "ExemptCheck";
    ExemptCheck.Size = new Size(108, 21);
    ExemptCheck.TabIndex = 5;
    ExemptCheck.Text = "Exempt Token";
    ExemptCheck.UseVisualStyleBackColor = true;
    // 
    // RuleEditForm
    // 
    AcceptButton = SaveButton;
    AutoScaleDimensions = new SizeF(7F, 17F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(689, 296);
    Controls.Add(AddTypeBox);
    Controls.Add(StringDataBox);
    Controls.Add(AddTypeButton);
    Controls.Add(MultCheck);
    Controls.Add(OptCheck);
    Controls.Add(RecursiveCheck);
    Controls.Add(FromTokensCheck);
    Controls.Add(IgnoreTokenCheck);
    Controls.Add(ExemptCheck);
    Controls.Add(IgnoreCaseCheck);
    Controls.Add(SaveButton);
    Controls.Add(TheCancelButton);
    Controls.Add(StringDataLabel);
    Controls.Add(RuleFlagsLabel);
    Controls.Add(TypeToAssignLabel);
    Controls.Add(TypeToAssignListBox);
    Controls.Add(TokenTypeGroup);
    FormBorderStyle = FormBorderStyle.FixedSingle;
    MaximizeBox = false;
    MinimizeBox = false;
    Name = "RuleEditForm";
    ShowIcon = false;
    ShowInTaskbar = false;
    SizeGripStyle = SizeGripStyle.Hide;
    Text = "RuleEditForm";
    TopMost = true;
    Load += RuleEditForm_Load;
    TokenTypeGroup.ResumeLayout(false);
    TokenTypeGroup.PerformLayout();
    StringDataContextMenu.ResumeLayout(false);
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private RadioButton CompetitiveRadio;
  private RadioButton TokenMatchRadio;
  private RadioButton TokenExactRadio;
  private RadioButton SplitMatchRadio;
  private RadioButton SplitExactRadio;
  private GroupBox TokenTypeGroup;
  private RadioButton TokenExtractRadio;
  private RadioButton StoreOtherRadio;
  private RadioButton StoreExtraRadio;
  private RadioButton ErrorMatchRadio;
  private ListBox TypeToAssignListBox;
  private Label TypeToAssignLabel;
  private Button TheCancelButton;
  private Button SaveButton;
  private CheckBox IgnoreCaseCheck;
  private CheckBox IgnoreTokenCheck;
  private CheckBox FromTokensCheck;
  private CheckBox RecursiveCheck;
  private CheckBox OptCheck;
  private CheckBox MultCheck;
  private Button AddTypeButton;
  private TextBox StringDataBox;
  private Label StringDataLabel;
  private TextBox AddTypeBox;
  private Label RuleFlagsLabel;
  private CheckBox ExemptCheck;
  private ContextMenuStrip StringDataContextMenu;
  private ToolStripMenuItem SelectAllContextMenuItem;
}
