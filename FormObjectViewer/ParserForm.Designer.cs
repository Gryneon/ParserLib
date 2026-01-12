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
    FileMenu = new ToolStripMenuItem();
    LoadSpecMenuItem = new ToolStripMenuItem();
    OpenFileMenuItem = new ToolStripMenuItem();
    generateRulesToolStripMenuItem = new ToolStripMenuItem();
    toolStripSeparator1 = new ToolStripSeparator();
    ExitMenuItem = new ToolStripMenuItem();
    SpecComboBox = new ComboBox();
    SpecBindingSource = new BindingSource(components);
    label1 = new Label();
    TypeColumn = new DataGridViewComboBoxColumn();
    typeToAssignDataGridViewTextBoxColumn = new DataGridViewComboBoxColumn();
    RuleStringDataColumn = new DataGridViewTextBoxColumn();
    TokenRuleBindingSource = new BindingSource(components);
    button1 = new Button();
    fileToolStripMenuItem1 = new ToolStripMenuItem();
    menuStrip1.SuspendLayout();
    ((ISupportInitialize) SpecBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleBindingSource).BeginInit();
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
    menuStrip1.Items.AddRange(new ToolStripItem[] { FileMenu });
    menuStrip1.Location = new Point(0, 0);
    menuStrip1.Name = "menuStrip1";
    menuStrip1.Size = new Size(1214, 28);
    menuStrip1.TabIndex = 2;
    menuStrip1.Text = "menuStrip1";
    // 
    // FileMenu
    // 
    FileMenu.DropDownItems.AddRange(new ToolStripItem[] { LoadSpecMenuItem, fileToolStripMenuItem1, OpenFileMenuItem, generateRulesToolStripMenuItem, toolStripSeparator1, ExitMenuItem });
    FileMenu.Name = "FileMenu";
    FileMenu.Size = new Size(46, 24);
    FileMenu.Text = "File";
    // 
    // LoadSpecMenuItem
    // 
    LoadSpecMenuItem.Name = "LoadSpecMenuItem";
    LoadSpecMenuItem.Size = new Size(224, 26);
    LoadSpecMenuItem.Text = "Load Specs";
    LoadSpecMenuItem.Click += LoadSpecMenuItem_Click;
    // 
    // OpenFileMenuItem
    // 
    OpenFileMenuItem.Name = "OpenFileMenuItem";
    OpenFileMenuItem.Size = new Size(224, 26);
    OpenFileMenuItem.Text = "Open File to Parse";
    // 
    // generateRulesToolStripMenuItem
    // 
    generateRulesToolStripMenuItem.Name = "generateRulesToolStripMenuItem";
    generateRulesToolStripMenuItem.Size = new Size(224, 26);
    generateRulesToolStripMenuItem.Text = "Generate Rules";
    // 
    // toolStripSeparator1
    // 
    toolStripSeparator1.Name = "toolStripSeparator1";
    toolStripSeparator1.Size = new Size(221, 6);
    // 
    // ExitMenuItem
    // 
    ExitMenuItem.Name = "ExitMenuItem";
    ExitMenuItem.Size = new Size(224, 26);
    ExitMenuItem.Text = "Exit";
    // 
    // SpecComboBox
    // 
    SpecComboBox.AccessibleRole = AccessibleRole.ComboBox;
    SpecComboBox.DataBindings.Add(new Binding("Text", SpecBindingSource, "Name", true));
    SpecComboBox.DataBindings.Add(new Binding("SelectedItem", SpecBindingSource, "Name", true));
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(8, 48);
    SpecComboBox.Name = "SpecComboBox";
    SpecComboBox.Size = new Size(192, 28);
    SpecComboBox.TabIndex = 3;
    // 
    // SpecBindingSource
    // 
    SpecBindingSource.DataSource = typeof(Spec);
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
    // TypeColumn
    // 
    TypeColumn.DataPropertyName = "Type";
    TypeColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
    TypeColumn.HeaderText = "Type";
    TypeColumn.MinimumWidth = 6;
    TypeColumn.Name = "TypeColumn";
    TypeColumn.Resizable = DataGridViewTriState.True;
    TypeColumn.Width = 125;
    // 
    // typeToAssignDataGridViewTextBoxColumn
    // 
    typeToAssignDataGridViewTextBoxColumn.DataPropertyName = "TypeToAssign";
    typeToAssignDataGridViewTextBoxColumn.HeaderText = "TypeToAssign";
    typeToAssignDataGridViewTextBoxColumn.MinimumWidth = 6;
    typeToAssignDataGridViewTextBoxColumn.Name = "typeToAssignDataGridViewTextBoxColumn";
    typeToAssignDataGridViewTextBoxColumn.Resizable = DataGridViewTriState.True;
    typeToAssignDataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
    typeToAssignDataGridViewTextBoxColumn.Width = 125;
    // 
    // RuleStringDataColumn
    // 
    RuleStringDataColumn.DataPropertyName = "RuleStringData";
    RuleStringDataColumn.HeaderText = "RuleStringData";
    RuleStringDataColumn.MinimumWidth = 6;
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
    // fileToolStripMenuItem1
    // 
    fileToolStripMenuItem1.Enabled = false;
    fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
    fileToolStripMenuItem1.ShowShortcutKeys = false;
    fileToolStripMenuItem1.Size = new Size(224, 26);
    fileToolStripMenuItem1.Text = "File";
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(8F, 20F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1214, 503);
    Controls.Add(button1);
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
    ((ISupportInitialize) SpecBindingSource).EndInit();
    ((ISupportInitialize) TokenRuleBindingSource).EndInit();
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
  private ToolStripMenuItem FileMenu;
  private ToolStripMenuItem LoadSpecMenuItem;
  private ToolStripMenuItem OpenFileMenuItem;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripMenuItem generateRulesToolStripMenuItem;
  private ToolStripMenuItem ExitMenuItem;
  private BindingSource SpecBindingSource;
  private ToolStripMenuItem fileToolStripMenuItem1;
}
