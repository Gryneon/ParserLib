using System.Drawing;
using System.Windows.Forms;

namespace ParserVisualizer;

partial class OperationBuilder
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
    OperationListBox = new ListBox();
    label1 = new Label();
    tabControl1 = new TabControl();
    tabPage1 = new TabPage();
    label2 = new Label();
    textBox3 = new TextBox();
    label3 = new Label();
    textBox1 = new TextBox();
    tabPage2 = new TabPage();
    dataGridView1 = new DataGridView();
    ColName = new DataGridViewTextBoxColumn();
    Regex = new DataGridViewTextBoxColumn();
    button1 = new Button();
    tabControl1.SuspendLayout();
    tabPage1.SuspendLayout();
    tabPage2.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize) dataGridView1).BeginInit();
    SuspendLayout();
    // 
    // OperationListBox
    // 
    OperationListBox.FormattingEnabled = true;
    OperationListBox.Location = new Point(8, 64);
    OperationListBox.Name = "OperationListBox";
    OperationListBox.Size = new Size(160, 379);
    OperationListBox.TabIndex = 0;
    OperationListBox.SelectedIndexChanged += OperationListBox_SelectedIndexChanged;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(4, 48);
    label1.Name = "label1";
    label1.Size = new Size(63, 15);
    label1.TabIndex = 1;
    label1.Text = "Operation:";
    // 
    // tabControl1
    // 
    tabControl1.Controls.Add(tabPage1);
    tabControl1.Controls.Add(tabPage2);
    tabControl1.Location = new Point(176, 40);
    tabControl1.Name = "tabControl1";
    tabControl1.SelectedIndex = 0;
    tabControl1.Size = new Size(616, 384);
    tabControl1.TabIndex = 2;
    // 
    // tabPage1
    // 
    tabPage1.Controls.Add(label2);
    tabPage1.Controls.Add(textBox3);
    tabPage1.Controls.Add(label3);
    tabPage1.Controls.Add(textBox1);
    tabPage1.Location = new Point(4, 24);
    tabPage1.Name = "tabPage1";
    tabPage1.Padding = new Padding(3);
    tabPage1.Size = new Size(608, 356);
    tabPage1.TabIndex = 0;
    tabPage1.Text = "Basic";
    tabPage1.UseVisualStyleBackColor = true;
    // 
    // label2
    // 
    label2.AutoSize = true;
    label2.Location = new Point(8, 8);
    label2.Name = "label2";
    label2.Size = new Size(60, 15);
    label2.TabIndex = 5;
    label2.Text = "Input Key:";
    // 
    // textBox3
    // 
    textBox3.Location = new Point(8, 64);
    textBox3.Name = "textBox3";
    textBox3.Size = new Size(168, 23);
    textBox3.TabIndex = 2;
    // 
    // label3
    // 
    label3.AutoSize = true;
    label3.Location = new Point(8, 48);
    label3.Name = "label3";
    label3.Size = new Size(70, 15);
    label3.TabIndex = 1;
    label3.Text = "Output Key:";
    // 
    // textBox1
    // 
    textBox1.Location = new Point(8, 24);
    textBox1.Name = "textBox1";
    textBox1.Size = new Size(168, 23);
    textBox1.TabIndex = 2;
    // 
    // tabPage2
    // 
    tabPage2.Controls.Add(dataGridView1);
    tabPage2.Location = new Point(4, 24);
    tabPage2.Name = "tabPage2";
    tabPage2.Padding = new Padding(3);
    tabPage2.Size = new Size(608, 356);
    tabPage2.TabIndex = 1;
    tabPage2.Text = "Regex List";
    tabPage2.UseVisualStyleBackColor = true;
    // 
    // dataGridView1
    // 
    dataGridView1.AllowUserToDeleteRows = false;
    dataGridView1.AllowUserToOrderColumns = true;
    dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColName, Regex });
    dataGridView1.Location = new Point(8, 8);
    dataGridView1.Name = "dataGridView1";
    dataGridView1.Size = new Size(592, 344);
    dataGridView1.TabIndex = 0;
    // 
    // ColName
    // 
    ColName.HeaderText = "Name";
    ColName.Name = "ColName";
    ColName.Width = 80;
    // 
    // Regex
    // 
    Regex.HeaderText = "Regex";
    Regex.Name = "Regex";
    // 
    // button1
    // 
    button1.Location = new Point(712, 424);
    button1.Name = "button1";
    button1.Size = new Size(75, 23);
    button1.TabIndex = 3;
    button1.Text = "Add";
    button1.UseVisualStyleBackColor = true;
    // 
    // OperationBuilder
    // 
    AutoScaleDimensions = new SizeF(7F, 15F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(800, 450);
    Controls.Add(tabControl1);
    Controls.Add(button1);
    Controls.Add(OperationListBox);
    Controls.Add(label1);
    Name = "OperationBuilder";
    Text = "OperationBuilder";
    Load += OperationBuilder_Load;
    tabControl1.ResumeLayout(false);
    tabPage1.ResumeLayout(false);
    tabPage1.PerformLayout();
    tabPage2.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize) dataGridView1).EndInit();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private ListBox OperationListBox;
  private Label label1;
  private TabControl tabControl1;
  private TabPage tabPage1;
  private TextBox textBox3;
  private Label label3;
  private TextBox textBox1;
  private TabPage tabPage2;
  private Label label2;
  private Button button1;
  private DataGridView dataGridView1;
  private DataGridViewTextBoxColumn ColName;
  private DataGridViewTextBoxColumn Regex;
}
