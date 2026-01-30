namespace FormObjectViewer;

partial class UnparsedViewer
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
    SectionCountTextBox = new TextBox();
    InverseCountTextBox = new TextBox();
    InverseCharsTextBox = new TextBox();
    SectionsTotalLabel = new Label();
    label1 = new Label();
    label2 = new Label();
    VisualRichBox = new RichTextBox();
    CloseButton = new Button();
    SuspendLayout();
    // 
    // SectionCountTextBox
    // 
    SectionCountTextBox.Location = new Point(8, 8);
    SectionCountTextBox.Name = "SectionCountTextBox";
    SectionCountTextBox.ReadOnly = true;
    SectionCountTextBox.Size = new Size(100, 25);
    SectionCountTextBox.TabIndex = 0;
    // 
    // InverseCountTextBox
    // 
    InverseCountTextBox.Location = new Point(8, 40);
    InverseCountTextBox.Name = "InverseCountTextBox";
    InverseCountTextBox.ReadOnly = true;
    InverseCountTextBox.Size = new Size(100, 25);
    InverseCountTextBox.TabIndex = 0;
    // 
    // InverseCharsTextBox
    // 
    InverseCharsTextBox.Location = new Point(8, 72);
    InverseCharsTextBox.Name = "InverseCharsTextBox";
    InverseCharsTextBox.ReadOnly = true;
    InverseCharsTextBox.Size = new Size(100, 25);
    InverseCharsTextBox.TabIndex = 0;
    // 
    // SectionsTotalLabel
    // 
    SectionsTotalLabel.AutoSize = true;
    SectionsTotalLabel.Location = new Point(112, 8);
    SectionsTotalLabel.Name = "SectionsTotalLabel";
    SectionsTotalLabel.Size = new Size(88, 17);
    SectionsTotalLabel.TabIndex = 1;
    SectionsTotalLabel.Text = "Sections Total";
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(112, 40);
    label1.Name = "label1";
    label1.Size = new Size(107, 17);
    label1.TabIndex = 1;
    label1.Text = "Inverted Sections";
    // 
    // label2
    // 
    label2.AutoSize = true;
    label2.Location = new Point(112, 72);
    label2.Name = "label2";
    label2.Size = new Size(182, 17);
    label2.TabIndex = 1;
    label2.Text = "Inverted Unparsed Characters";
    // 
    // VisualRichBox
    // 
    VisualRichBox.Location = new Point(8, 104);
    VisualRichBox.Name = "VisualRichBox";
    VisualRichBox.ReadOnly = true;
    VisualRichBox.Size = new Size(440, 248);
    VisualRichBox.TabIndex = 2;
    VisualRichBox.Text = "";
    // 
    // CloseButton
    // 
    CloseButton.Location = new Point(368, 64);
    CloseButton.Name = "CloseButton";
    CloseButton.Size = new Size(75, 31);
    CloseButton.TabIndex = 3;
    CloseButton.Text = "Close";
    CloseButton.UseVisualStyleBackColor = true;
    // 
    // UnparsedViewer
    // 
    AutoScaleDimensions = new SizeF(7F, 17F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(454, 359);
    ControlBox = false;
    Controls.Add(CloseButton);
    Controls.Add(VisualRichBox);
    Controls.Add(label2);
    Controls.Add(label1);
    Controls.Add(SectionsTotalLabel);
    Controls.Add(InverseCharsTextBox);
    Controls.Add(InverseCountTextBox);
    Controls.Add(SectionCountTextBox);
    FormBorderStyle = FormBorderStyle.FixedToolWindow;
    MinimizeBox = false;
    Name = "UnparsedViewer";
    ShowInTaskbar = false;
    SizeGripStyle = SizeGripStyle.Hide;
    StartPosition = FormStartPosition.CenterParent;
    Text = "UnparsedViewer";
    TopMost = true;
    Load += UnparsedViewer_Load;
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private TextBox SectionCountTextBox;
  private TextBox InverseCountTextBox;
  private TextBox InverseCharsTextBox;
  private Label SectionsTotalLabel;
  private Label label1;
  private Label label2;
  private RichTextBox VisualRichBox;
  private Button CloseButton;
}
