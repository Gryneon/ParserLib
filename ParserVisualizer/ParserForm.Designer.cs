using System;
using System.Drawing;
using System.Windows.Forms;

namespace ParserVisualizer;

partial class ParserForm : Form
{
  /// <summary>
  ///  Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  ///  Clean up any resources being used.
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
  ///  Required method for Designer support - do not modify
  ///  the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent ()
  {
    OperationListBox = new ListBox();
    DataListBox = new ListBox();
    AdvanceOperationButton = new Button();
    SpecBox = new ComboBox();
    label1 = new Label();
    label2 = new Label();
    label3 = new Label();
    LoadSpecButton = new Button();
    FileNameLabel = new Label();
    button1 = new Button();
    label4 = new Label();
    button2 = new Button();
    SuspendLayout();
    // 
    // OperationListBox
    // 
    OperationListBox.FormattingEnabled = true;
    OperationListBox.Location = new Point(12, 149);
    OperationListBox.Name = "OperationListBox";
    OperationListBox.Size = new Size(185, 289);
    OperationListBox.TabIndex = 0;
    // 
    // DataListBox
    // 
    DataListBox.FormattingEnabled = true;
    DataListBox.Location = new Point(313, 149);
    DataListBox.Name = "DataListBox";
    DataListBox.Size = new Size(185, 289);
    DataListBox.TabIndex = 0;
    // 
    // AdvanceOperationButton
    // 
    AdvanceOperationButton.Location = new Point(504, 235);
    AdvanceOperationButton.Name = "AdvanceOperationButton";
    AdvanceOperationButton.Size = new Size(75, 23);
    AdvanceOperationButton.TabIndex = 1;
    AdvanceOperationButton.Text = "Advance";
    AdvanceOperationButton.UseVisualStyleBackColor = true;
    // 
    // SpecBox
    // 
    SpecBox.FormattingEnabled = true;
    SpecBox.Location = new Point(12, 27);
    SpecBox.Name = "SpecBox";
    SpecBox.Size = new Size(121, 23);
    SpecBox.TabIndex = 2;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(12, 9);
    label1.Name = "label1";
    label1.Size = new Size(32, 15);
    label1.TabIndex = 3;
    label1.Text = "Spec";
    // 
    // label2
    // 
    label2.AutoSize = true;
    label2.Location = new Point(313, 131);
    label2.Name = "label2";
    label2.Size = new Size(58, 15);
    label2.TabIndex = 3;
    label2.Text = "Data Keys";
    // 
    // label3
    // 
    label3.AutoSize = true;
    label3.Location = new Point(12, 131);
    label3.Name = "label3";
    label3.Size = new Size(65, 15);
    label3.TabIndex = 3;
    label3.Text = "Operations";
    // 
    // LoadSpecButton
    // 
    LoadSpecButton.Location = new Point(139, 27);
    LoadSpecButton.Name = "LoadSpecButton";
    LoadSpecButton.Size = new Size(75, 23);
    LoadSpecButton.TabIndex = 1;
    LoadSpecButton.Text = "Load Spec";
    LoadSpecButton.UseVisualStyleBackColor = true;
    // 
    // FileNameLabel
    // 
    FileNameLabel.AutoSize = true;
    FileNameLabel.Location = new Point(220, 60);
    FileNameLabel.Name = "FileNameLabel";
    FileNameLabel.Size = new Size(60, 15);
    FileNameLabel.TabIndex = 4;
    FileNameLabel.Text = "<No File>";
    // 
    // button1
    // 
    button1.Location = new Point(139, 56);
    button1.Name = "button1";
    button1.Size = new Size(75, 23);
    button1.TabIndex = 1;
    button1.Text = "Load File";
    button1.UseVisualStyleBackColor = true;
    // 
    // label4
    // 
    label4.AutoSize = true;
    label4.Location = new Point(220, 31);
    label4.Name = "label4";
    label4.Size = new Size(67, 15);
    label4.TabIndex = 4;
    label4.Text = "<No Spec>";
    // 
    // button2
    // 
    button2.Location = new Point(504, 152);
    button2.Name = "button2";
    button2.Size = new Size(136, 23);
    button2.TabIndex = 1;
    button2.Text = "Configure Operations";
    button2.UseVisualStyleBackColor = true;
    button2.Click += Button2_Click;
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(7F, 15F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(800, 450);
    Controls.Add(label4);
    Controls.Add(FileNameLabel);
    Controls.Add(label3);
    Controls.Add(label2);
    Controls.Add(label1);
    Controls.Add(SpecBox);
    Controls.Add(button1);
    Controls.Add(LoadSpecButton);
    Controls.Add(button2);
    Controls.Add(AdvanceOperationButton);
    Controls.Add(DataListBox);
    Controls.Add(OperationListBox);
    Name = "ParserForm";
    Text = "Parser Visualizer";
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private ListBox OperationListBox;
  private ListBox DataListBox;
  private Button AdvanceOperationButton;
  private ComboBox SpecBox;
  private Label label1;
  private Label label2;
  private Label label3;
  private Button LoadSpecButton;
  private Label FileNameLabel;
  private Button button1;
  private Label label4;
  private Button button2;
}
