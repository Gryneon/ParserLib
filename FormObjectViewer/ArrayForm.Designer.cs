namespace FormObjectViewer;

partial class ArrayForm
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
    ArrayListBox = new ListBox();
    CountLabel = new Label();
    CountBox = new TextBox();
    UpdateButton = new Button();
    SuspendLayout();
    // 
    // ArrayListBox
    // 
    ArrayListBox.FormattingEnabled = true;
    ArrayListBox.Location = new Point(8, 40);
    ArrayListBox.Name = "ArrayListBox";
    ArrayListBox.Size = new Size(224, 349);
    ArrayListBox.TabIndex = 0;
    ArrayListBox.MouseDoubleClick += ArrayListBox_MouseDoubleClick;
    // 
    // CountLabel
    // 
    CountLabel.AutoSize = true;
    CountLabel.Location = new Point(120, 8);
    CountLabel.Name = "CountLabel";
    CountLabel.Size = new Size(43, 15);
    CountLabel.TabIndex = 1;
    CountLabel.Text = "Count:";
    // 
    // CountBox
    // 
    CountBox.Enabled = false;
    CountBox.Location = new Point(168, 8);
    CountBox.Name = "CountBox";
    CountBox.Size = new Size(64, 23);
    CountBox.TabIndex = 2;
    // 
    // UpdateButton
    // 
    UpdateButton.Location = new Point(8, 400);
    UpdateButton.Name = "UpdateButton";
    UpdateButton.Size = new Size(216, 24);
    UpdateButton.TabIndex = 3;
    UpdateButton.Text = "Update";
    UpdateButton.UseVisualStyleBackColor = true;
    UpdateButton.Click += UpdateButton_Click;
    // 
    // ArrayForm
    // 
    AutoScaleDimensions = new SizeF(7F, 15F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(241, 431);
    Controls.Add(UpdateButton);
    Controls.Add(CountBox);
    Controls.Add(CountLabel);
    Controls.Add(ArrayListBox);
    Name = "ArrayForm";
    Text = "ArrayForm";
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private System.Windows.Forms.ListBox ArrayListBox;
  private System.Windows.Forms.Label CountLabel;
  private System.Windows.Forms.TextBox CountBox;
  private Button UpdateButton;
}
