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
    radioButton1 = new RadioButton();
    radioButton2 = new RadioButton();
    radioButton3 = new RadioButton();
    radioButton4 = new RadioButton();
    radioButton5 = new RadioButton();
    groupBox1 = new GroupBox();
    radioButton9 = new RadioButton();
    radioButton8 = new RadioButton();
    radioButton7 = new RadioButton();
    radioButton6 = new RadioButton();
    TypeToAssignListBox = new ListBox();
    label1 = new Label();
    CancelButton = new Button();
    SaveButton = new Button();
    groupBox1.SuspendLayout();
    SuspendLayout();
    // 
    // radioButton1
    // 
    radioButton1.AutoSize = true;
    radioButton1.Location = new Point(16, 32);
    radioButton1.Name = "radioButton1";
    radioButton1.Size = new Size(90, 19);
    radioButton1.TabIndex = 0;
    radioButton1.TabStop = true;
    radioButton1.Text = "Competitive";
    radioButton1.UseVisualStyleBackColor = true;
    // 
    // radioButton2
    // 
    radioButton2.AutoSize = true;
    radioButton2.Location = new Point(16, 56);
    radioButton2.Name = "radioButton2";
    radioButton2.Size = new Size(94, 19);
    radioButton2.TabIndex = 0;
    radioButton2.TabStop = true;
    radioButton2.Text = "Token Match";
    radioButton2.UseVisualStyleBackColor = true;
    // 
    // radioButton3
    // 
    radioButton3.AutoSize = true;
    radioButton3.Location = new Point(16, 80);
    radioButton3.Name = "radioButton3";
    radioButton3.Size = new Size(87, 19);
    radioButton3.TabIndex = 0;
    radioButton3.TabStop = true;
    radioButton3.Text = "Token Exact";
    radioButton3.UseVisualStyleBackColor = true;
    // 
    // radioButton4
    // 
    radioButton4.AutoSize = true;
    radioButton4.Location = new Point(16, 104);
    radioButton4.Name = "radioButton4";
    radioButton4.Size = new Size(85, 19);
    radioButton4.TabIndex = 0;
    radioButton4.TabStop = true;
    radioButton4.Text = "Split Match";
    radioButton4.UseVisualStyleBackColor = true;
    // 
    // radioButton5
    // 
    radioButton5.AutoSize = true;
    radioButton5.Location = new Point(16, 128);
    radioButton5.Name = "radioButton5";
    radioButton5.Size = new Size(78, 19);
    radioButton5.TabIndex = 0;
    radioButton5.TabStop = true;
    radioButton5.Text = "Split Exact";
    radioButton5.UseVisualStyleBackColor = true;
    // 
    // groupBox1
    // 
    groupBox1.Controls.Add(radioButton1);
    groupBox1.Controls.Add(radioButton9);
    groupBox1.Controls.Add(radioButton8);
    groupBox1.Controls.Add(radioButton7);
    groupBox1.Controls.Add(radioButton6);
    groupBox1.Controls.Add(radioButton5);
    groupBox1.Controls.Add(radioButton2);
    groupBox1.Controls.Add(radioButton4);
    groupBox1.Controls.Add(radioButton3);
    groupBox1.Location = new Point(8, 8);
    groupBox1.Name = "groupBox1";
    groupBox1.Size = new Size(128, 248);
    groupBox1.TabIndex = 1;
    groupBox1.TabStop = false;
    groupBox1.Text = "Token Rule Type";
    // 
    // radioButton9
    // 
    radioButton9.AutoSize = true;
    radioButton9.Location = new Point(16, 224);
    radioButton9.Name = "radioButton9";
    radioButton9.Size = new Size(85, 19);
    radioButton9.TabIndex = 0;
    radioButton9.TabStop = true;
    radioButton9.Text = "Store Other";
    radioButton9.UseVisualStyleBackColor = true;
    // 
    // radioButton8
    // 
    radioButton8.AutoSize = true;
    radioButton8.Location = new Point(16, 200);
    radioButton8.Name = "radioButton8";
    radioButton8.Size = new Size(80, 19);
    radioButton8.TabIndex = 0;
    radioButton8.TabStop = true;
    radioButton8.Text = "Store Extra";
    radioButton8.UseVisualStyleBackColor = true;
    // 
    // radioButton7
    // 
    radioButton7.AutoSize = true;
    radioButton7.Location = new Point(16, 176);
    radioButton7.Name = "radioButton7";
    radioButton7.Size = new Size(87, 19);
    radioButton7.TabIndex = 0;
    radioButton7.TabStop = true;
    radioButton7.Text = "Error Match";
    radioButton7.UseVisualStyleBackColor = true;
    // 
    // radioButton6
    // 
    radioButton6.AutoSize = true;
    radioButton6.Location = new Point(16, 152);
    radioButton6.Name = "radioButton6";
    radioButton6.Size = new Size(95, 19);
    radioButton6.TabIndex = 0;
    radioButton6.TabStop = true;
    radioButton6.Text = "Token Extract";
    radioButton6.UseVisualStyleBackColor = true;
    // 
    // TypeToAssignListBox
    // 
    TypeToAssignListBox.FormattingEnabled = true;
    TypeToAssignListBox.Location = new Point(144, 32);
    TypeToAssignListBox.Name = "TypeToAssignListBox";
    TypeToAssignListBox.Size = new Size(120, 214);
    TypeToAssignListBox.TabIndex = 2;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(144, 16);
    label1.Name = "label1";
    label1.Size = new Size(84, 15);
    label1.TabIndex = 3;
    label1.Text = "Type to Assign";
    // 
    // CancelButton
    // 
    CancelButton.Location = new Point(616, 216);
    CancelButton.Name = "CancelButton";
    CancelButton.Size = new Size(64, 32);
    CancelButton.TabIndex = 4;
    CancelButton.Text = "Cancel";
    CancelButton.UseVisualStyleBackColor = true;
    // 
    // SaveButton
    // 
    SaveButton.Location = new Point(272, 216);
    SaveButton.Name = "SaveButton";
    SaveButton.Size = new Size(64, 32);
    SaveButton.TabIndex = 4;
    SaveButton.Text = "Save";
    SaveButton.UseVisualStyleBackColor = true;
    // 
    // RuleEditForm
    // 
    AcceptButton = SaveButton;
    AutoScaleDimensions = new SizeF(7F, 15F);
    AutoScaleMode = AutoScaleMode.Font;
    CancelButton = CancelButton;
    ClientSize = new Size(689, 261);
    Controls.Add(SaveButton);
    Controls.Add(CancelButton);
    Controls.Add(label1);
    Controls.Add(TypeToAssignListBox);
    Controls.Add(groupBox1);
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
    groupBox1.ResumeLayout(false);
    groupBox1.PerformLayout();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private RadioButton radioButton1;
  private RadioButton radioButton2;
  private RadioButton radioButton3;
  private RadioButton radioButton4;
  private RadioButton radioButton5;
  private GroupBox groupBox1;
  private RadioButton radioButton6;
  private RadioButton radioButton9;
  private RadioButton radioButton8;
  private RadioButton radioButton7;
  private ListBox TypeToAssignListBox;
  private Label label1;
  private Button CancelButton;
  private Button SaveButton;
}