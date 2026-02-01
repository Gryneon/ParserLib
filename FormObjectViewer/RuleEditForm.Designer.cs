namespace FormObjectViewer;

partial class RuleEditForm
{
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
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
    CompetitiveRadio = new RadioButton();
    TokenMatchRadio = new RadioButton();
    radioButton3 = new RadioButton();
    radioButton4 = new RadioButton();
    radioButton5 = new RadioButton();
    TokenTypeGroup = new GroupBox();
    radioButton9 = new RadioButton();
    radioButton8 = new RadioButton();
    radioButton7 = new RadioButton();
    radioButton6 = new RadioButton();
    TypeToAssignListBox = new ListBox();
    label1 = new Label();
    TheCancelButton = new Button();
    SaveButton = new Button();
    IgnoreCaseCheck = new CheckBox();
    IgnoreTokenCheck = new CheckBox();
    FromTokensCheck = new CheckBox();
    RecursiveCheck = new CheckBox();
    OptCheck = new CheckBox();
    MultCheck = new CheckBox();
    button1 = new Button();
    StringDataBox = new TextBox();
    StringDataLabel = new Label();
    textBox1 = new TextBox();
    label2 = new Label();
    TokenTypeGroup.SuspendLayout();
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
    // radioButton3
    // 
    radioButton3.AutoSize = true;
    radioButton3.Location = new Point(8, 87);
    radioButton3.Name = "radioButton3";
    radioButton3.Size = new Size(94, 21);
    radioButton3.TabIndex = 0;
    radioButton3.TabStop = true;
    radioButton3.Text = "Token Exact";
    radioButton3.UseVisualStyleBackColor = true;
    // 
    // radioButton4
    // 
    radioButton4.AutoSize = true;
    radioButton4.Location = new Point(8, 114);
    radioButton4.Name = "radioButton4";
    radioButton4.Size = new Size(91, 21);
    radioButton4.TabIndex = 0;
    radioButton4.TabStop = true;
    radioButton4.Text = "Split Match";
    radioButton4.UseVisualStyleBackColor = true;
    // 
    // radioButton5
    // 
    radioButton5.AutoSize = true;
    radioButton5.Location = new Point(8, 141);
    radioButton5.Name = "radioButton5";
    radioButton5.Size = new Size(85, 21);
    radioButton5.TabIndex = 0;
    radioButton5.TabStop = true;
    radioButton5.Text = "Split Exact";
    radioButton5.UseVisualStyleBackColor = true;
    // 
    // TokenTypeGroup
    // 
    TokenTypeGroup.Controls.Add(CompetitiveRadio);
    TokenTypeGroup.Controls.Add(radioButton9);
    TokenTypeGroup.Controls.Add(radioButton8);
    TokenTypeGroup.Controls.Add(radioButton7);
    TokenTypeGroup.Controls.Add(radioButton6);
    TokenTypeGroup.Controls.Add(radioButton5);
    TokenTypeGroup.Controls.Add(TokenMatchRadio);
    TokenTypeGroup.Controls.Add(radioButton4);
    TokenTypeGroup.Controls.Add(radioButton3);
    TokenTypeGroup.Location = new Point(8, 9);
    TokenTypeGroup.Name = "TokenTypeGroup";
    TokenTypeGroup.Size = new Size(128, 281);
    TokenTypeGroup.TabIndex = 1;
    TokenTypeGroup.TabStop = false;
    TokenTypeGroup.Text = "Token Rule Type";
    // 
    // radioButton9
    // 
    radioButton9.AutoSize = true;
    radioButton9.Location = new Point(8, 250);
    radioButton9.Name = "radioButton9";
    radioButton9.Size = new Size(94, 21);
    radioButton9.TabIndex = 0;
    radioButton9.TabStop = true;
    radioButton9.Text = "Store Other";
    radioButton9.UseVisualStyleBackColor = true;
    // 
    // radioButton8
    // 
    radioButton8.AutoSize = true;
    radioButton8.Location = new Point(8, 223);
    radioButton8.Name = "radioButton8";
    radioButton8.Size = new Size(90, 21);
    radioButton8.TabIndex = 0;
    radioButton8.TabStop = true;
    radioButton8.Text = "Store Extra";
    radioButton8.UseVisualStyleBackColor = true;
    // 
    // radioButton7
    // 
    radioButton7.AutoSize = true;
    radioButton7.Location = new Point(8, 195);
    radioButton7.Name = "radioButton7";
    radioButton7.Size = new Size(96, 21);
    radioButton7.TabIndex = 0;
    radioButton7.TabStop = true;
    radioButton7.Text = "Error Match";
    radioButton7.UseVisualStyleBackColor = true;
    // 
    // radioButton6
    // 
    radioButton6.AutoSize = true;
    radioButton6.Location = new Point(8, 168);
    radioButton6.Name = "radioButton6";
    radioButton6.Size = new Size(103, 21);
    radioButton6.TabIndex = 0;
    radioButton6.TabStop = true;
    radioButton6.Text = "Token Extract";
    radioButton6.UseVisualStyleBackColor = true;
    // 
    // TypeToAssignListBox
    // 
    TypeToAssignListBox.Font = new Font("Segoe Fluent Icons", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
    TypeToAssignListBox.FormattingEnabled = true;
    TypeToAssignListBox.IntegralHeight = false;
    TypeToAssignListBox.Location = new Point(144, 40);
    TypeToAssignListBox.Name = "TypeToAssignListBox";
    TypeToAssignListBox.Size = new Size(120, 168);
    TypeToAssignListBox.TabIndex = 2;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(144, 16);
    label1.Name = "label1";
    label1.Size = new Size(93, 17);
    label1.TabIndex = 3;
    label1.Text = "Type to Assign";
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
    IgnoreCaseCheck.CheckedChanged += IgnoreCaseCheck_CheckedChanged;
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
    // button1
    // 
    button1.Location = new Point(144, 248);
    button1.Name = "button1";
    button1.Size = new Size(120, 32);
    button1.TabIndex = 6;
    button1.Text = "Add Type";
    button1.UseVisualStyleBackColor = true;
    // 
    // StringDataBox
    // 
    StringDataBox.BorderStyle = BorderStyle.FixedSingle;
    StringDataBox.Location = new Point(400, 40);
    StringDataBox.Multiline = true;
    StringDataBox.Name = "StringDataBox";
    StringDataBox.Size = new Size(264, 200);
    StringDataBox.TabIndex = 7;
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
    // textBox1
    // 
    textBox1.Location = new Point(144, 216);
    textBox1.Name = "textBox1";
    textBox1.Size = new Size(120, 25);
    textBox1.TabIndex = 8;
    // 
    // label2
    // 
    label2.AutoSize = true;
    label2.Location = new Point(280, 16);
    label2.Name = "label2";
    label2.Size = new Size(67, 17);
    label2.TabIndex = 3;
    label2.Text = "Rule Flags";
    // 
    // RuleEditForm
    // 
    AcceptButton = SaveButton;
    AutoScaleDimensions = new SizeF(7F, 17F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(689, 296);
    Controls.Add(textBox1);
    Controls.Add(StringDataBox);
    Controls.Add(button1);
    Controls.Add(MultCheck);
    Controls.Add(OptCheck);
    Controls.Add(RecursiveCheck);
    Controls.Add(FromTokensCheck);
    Controls.Add(IgnoreTokenCheck);
    Controls.Add(IgnoreCaseCheck);
    Controls.Add(SaveButton);
    Controls.Add(TheCancelButton);
    Controls.Add(StringDataLabel);
    Controls.Add(label2);
    Controls.Add(label1);
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
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private RadioButton CompetitiveRadio;
  private RadioButton TokenMatchRadio;
  private RadioButton radioButton3;
  private RadioButton radioButton4;
  private RadioButton radioButton5;
  private GroupBox TokenTypeGroup;
  private RadioButton radioButton6;
  private RadioButton radioButton9;
  private RadioButton radioButton8;
  private RadioButton radioButton7;
  private ListBox TypeToAssignListBox;
  private Label label1;
  private Button TheCancelButton;
  private Button SaveButton;
  private CheckBox IgnoreCaseCheck;
  private CheckBox IgnoreTokenCheck;
  private CheckBox FromTokensCheck;
  private CheckBox RecursiveCheck;
  private CheckBox OptCheck;
  private CheckBox MultCheck;
  private Button button1;
  private TextBox StringDataBox;
  private Label StringDataLabel;
  private TextBox textBox1;
  private Label label2;
}
