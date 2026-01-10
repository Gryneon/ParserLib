using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormObjectViewer;

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
    components = new Container();
    StatusStrip = new StatusStrip();
    OpenParseFileDialog = new OpenFileDialog();
    menuStrip1 = new MenuStrip();
    fileToolStripMenuItem = new ToolStripMenuItem();
    loadSpecToolStripMenuItem = new ToolStripMenuItem();
    openFileToParseToolStripMenuItem = new ToolStripMenuItem();
    generateRulesToolStripMenuItem = new ToolStripMenuItem();
    toolStripSeparator1 = new ToolStripSeparator();
    exitToolStripMenuItem = new ToolStripMenuItem();
    SpecComboBox = new ComboBox();
    label1 = new Label();
    dataGridView1 = new DataGridView();
    TypeColumn = new DataGridViewComboBoxColumn();
    typeToAssignDataGridViewTextBoxColumn = new DataGridViewComboBoxColumn();
    RuleStringDataColumn = new DataGridViewTextBoxColumn();
    TokenRuleBindingSource = new BindingSource(components);
    button1 = new Button();
    specBindingSource = new BindingSource(components);
    menuStrip1.SuspendLayout();
    ((ISupportInitialize) dataGridView1).BeginInit();
    ((ISupportInitialize) TokenRuleBindingSource).BeginInit();
    ((ISupportInitialize) specBindingSource).BeginInit();
    SuspendLayout();
    // 
    // StatusStrip
    // 
    StatusStrip.ImageScalingSize = new Size(20, 20);
    StatusStrip.Location = new Point(0, 481);
    StatusStrip.Name = "StatusStrip";
    StatusStrip.Size = new Size(1214, 22);
    StatusStrip.TabIndex = 0;
    StatusStrip.Text = "StatusStrip";
    // 
    // OpenParseFileDialog
    // 
    OpenParseFileDialog.InitialDirectory = "C:\\users\\johntay4\\source\\repos\\git";
    OpenParseFileDialog.ShowPreview = true;
    OpenParseFileDialog.ShowReadOnly = true;
    OpenParseFileDialog.SupportMultiDottedExtensions = true;
    OpenParseFileDialog.Title = "Parser File Selection";
    OpenParseFileDialog.FileOk += OpenParseFileDialog_FileOk;
    // 
    // menuStrip1
    // 
    menuStrip1.ImageScalingSize = new Size(20, 20);
    menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
    menuStrip1.Location = new Point(0, 0);
    menuStrip1.Name = "menuStrip1";
    menuStrip1.Size = new Size(1214, 28);
    menuStrip1.TabIndex = 2;
    menuStrip1.Text = "menuStrip1";
    // 
    // fileToolStripMenuItem
    // 
    fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSpecToolStripMenuItem, openFileToParseToolStripMenuItem, generateRulesToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
    fileToolStripMenuItem.Name = "fileToolStripMenuItem";
    fileToolStripMenuItem.Size = new Size(44, 24);
    fileToolStripMenuItem.Text = "File";
    // 
    // loadSpecToolStripMenuItem
    // 
    loadSpecToolStripMenuItem.Name = "loadSpecToolStripMenuItem";
    loadSpecToolStripMenuItem.Size = new Size(197, 24);
    loadSpecToolStripMenuItem.Text = "Load Specs";
    loadSpecToolStripMenuItem.Click += LoadSpecToolStripMenuItem_Click;
    // 
    // openFileToParseToolStripMenuItem
    // 
    openFileToParseToolStripMenuItem.Name = "openFileToParseToolStripMenuItem";
    openFileToParseToolStripMenuItem.Size = new Size(197, 24);
    openFileToParseToolStripMenuItem.Text = "Open File to Parse";
    // 
    // generateRulesToolStripMenuItem
    // 
    generateRulesToolStripMenuItem.Name = "generateRulesToolStripMenuItem";
    generateRulesToolStripMenuItem.Size = new Size(197, 24);
    generateRulesToolStripMenuItem.Text = "Generate Rules";
    // 
    // toolStripSeparator1
    // 
    toolStripSeparator1.Name = "toolStripSeparator1";
    toolStripSeparator1.Size = new Size(194, 6);
    // 
    // exitToolStripMenuItem
    // 
    exitToolStripMenuItem.Name = "exitToolStripMenuItem";
    exitToolStripMenuItem.Size = new Size(197, 24);
    exitToolStripMenuItem.Text = "Exit";
    // 
    // SpecComboBox
    // 
    SpecComboBox.AccessibleRole = AccessibleRole.ComboBox;
    SpecComboBox.DataBindings.Add(new Binding("Text", specBindingSource, "Name", true));
    SpecComboBox.DataBindings.Add(new Binding("SelectedItem", specBindingSource, "Name", true));
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(8, 48);
    SpecComboBox.Name = "SpecComboBox";
    SpecComboBox.Size = new Size(192, 28);
    SpecComboBox.TabIndex = 3;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(8, 24);
    label1.Name = "label1";
    label1.Size = new Size(95, 20);
    label1.TabIndex = 4;
    label1.Text = "Specification";
    // 
    // dataGridView1
    // 
    dataGridView1.AutoGenerateColumns = false;
    dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    dataGridView1.Columns.AddRange(new DataGridViewColumn[] { TypeColumn, typeToAssignDataGridViewTextBoxColumn, RuleStringDataColumn });
    dataGridView1.DataSource = TokenRuleBindingSource;
    dataGridView1.Location = new Point(552, 32);
    dataGridView1.Name = "dataGridView1";
    dataGridView1.Size = new Size(648, 168);
    dataGridView1.TabIndex = 5;
    // 
    // TypeColumn
    // 
    TypeColumn.DataPropertyName = "Type";
    TypeColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
    TypeColumn.HeaderText = "Type";
    TypeColumn.Name = "TypeColumn";
    TypeColumn.Resizable = DataGridViewTriState.True;
    // 
    // typeToAssignDataGridViewTextBoxColumn
    // 
    typeToAssignDataGridViewTextBoxColumn.DataPropertyName = "TypeToAssign";
    typeToAssignDataGridViewTextBoxColumn.HeaderText = "TypeToAssign";
    typeToAssignDataGridViewTextBoxColumn.Name = "typeToAssignDataGridViewTextBoxColumn";
    typeToAssignDataGridViewTextBoxColumn.Resizable = DataGridViewTriState.True;
    typeToAssignDataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
    // 
    // RuleStringDataColumn
    // 
    RuleStringDataColumn.DataPropertyName = "RuleStringData";
    RuleStringDataColumn.HeaderText = "RuleStringData";
    RuleStringDataColumn.Name = "RuleStringDataColumn";
    RuleStringDataColumn.Width = 400;
    // 
    // TokenRuleBindingSource
    // 
    TokenRuleBindingSource.DataSource = typeof(Parser.Tokens.TokenRule);
    // 
    // button1
    // 
    button1.Location = new Point(1096, 440);
    button1.Name = "button1";
    button1.Size = new Size(104, 32);
    button1.TabIndex = 6;
    button1.Text = "Parse";
    button1.UseVisualStyleBackColor = true;
    // 
    // specBindingSource
    // 
    specBindingSource.DataSource = typeof(Spec);
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(8F, 20F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1214, 503);
    Controls.Add(button1);
    Controls.Add(dataGridView1);
    Controls.Add(label1);
    Controls.Add(SpecComboBox);
    Controls.Add(StatusStrip);
    Controls.Add(menuStrip1);
    MainMenuStrip = menuStrip1;
    Name = "ParserForm";
    Text = "Parser Form";
    Load += ParserForm_Load;
    menuStrip1.ResumeLayout(false);
    menuStrip1.PerformLayout();
    ((ISupportInitialize) dataGridView1).EndInit();
    ((ISupportInitialize) TokenRuleBindingSource).EndInit();
    ((ISupportInitialize) specBindingSource).EndInit();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private StatusStrip StatusStrip;
  private OpenFileDialog OpenParseFileDialog;
  private MenuStrip menuStrip1;
  private ComboBox SpecComboBox;
  private Label label1;
  private DataGridView dataGridView1;
  private BindingSource TokenRuleBindingSource;
  private DataGridViewComboBoxColumn TypeColumn;
  private DataGridViewComboBoxColumn typeToAssignDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn RuleStringDataColumn;
  private Button button1;
  private ToolStripMenuItem fileToolStripMenuItem;
  private ToolStripMenuItem loadSpecToolStripMenuItem;
  private ToolStripMenuItem openFileToParseToolStripMenuItem;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripMenuItem generateRulesToolStripMenuItem;
  private ToolStripMenuItem exitToolStripMenuItem;
  private BindingSource specBindingSource;
}
