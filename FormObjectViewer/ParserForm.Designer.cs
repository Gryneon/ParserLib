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
    TheMenuStrip = new MenuStrip();
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
    ParseButton = new Button();
    ItemTabs = new TabControl();
    tabPage1 = new TabPage();
    TokenGridView = new DataGridView();
    contentDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    ignoredDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    exemptDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    typeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    childrenDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    hasTypeDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    tokenBindingSource = new BindingSource(components);
    TokenRuleDataGrid = new DataGridView();
    typeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    ruleStringDataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    typeToAssignDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    AssemblerPage = new TabPage();
    LoadRulesButton = new Button();
    readOnlyDictionaryGrid1 = new Parser.Forms.ReadOnlyDictionaryGrid();
    DataDictionaryGrid = new Parser.Forms.ReadOnlyDictionaryGrid();
    checkBox1 = new CheckBox();
    checkBox2 = new CheckBox();
    checkBox3 = new CheckBox();
    SpecLoadedCheck = new CheckBox();
    button1 = new Button();
    textBox1 = new TextBox();
    menuStrip1.SuspendLayout();
    ((ISupportInitialize) SpecBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleBindingSource).BeginInit();
    ItemTabs.SuspendLayout();
    tabPage1.SuspendLayout();
    ((ISupportInitialize) TokenGridView).BeginInit();
    ((ISupportInitialize) tokenBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleDataGrid).BeginInit();
    AssemblerPage.SuspendLayout();
    SuspendLayout();
    // 
    // StatusStrip
    // 
    StatusStrip.ImageScalingSize = new Size(20, 20);
    StatusStrip.Location = new Point(0, 355);
    StatusStrip.Name = "StatusStrip";
    StatusStrip.Padding = new Padding(1, 0, 12, 0);
    StatusStrip.Size = new Size(1062, 22);
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
    // MainMenuStrip
    // 
    TheMenuStrip.ImageScalingSize = new Size(20, 20);
    TheMenuStrip.Items.AddRange(new ToolStripItem[] { FileMenu });
    TheMenuStrip.Location = new Point(0, 0);
    TheMenuStrip.Name = "MainMenuStrip";
    TheMenuStrip.Padding = new Padding(5, 2, 0, 2);
    TheMenuStrip.Size = new Size(1062, 24);
    TheMenuStrip.TabIndex = 2;
    TheMenuStrip.Text = "menuStrip1";
    // 
    // FileMenu
    // 
    FileMenu.DropDownItems.AddRange(new ToolStripItem[] { LoadSpecMenuItem, OpenFileMenuItem, generateRulesToolStripMenuItem, toolStripSeparator1, ExitMenuItem });
    FileMenu.Name = "FileMenu";
    FileMenu.Size = new Size(44, 24);
    FileMenu.Text = "File";
    // 
    // LoadSpecMenuItem
    // 
    LoadSpecMenuItem.Name = "LoadSpecMenuItem";
    LoadSpecMenuItem.Size = new Size(197, 24);
    LoadSpecMenuItem.Text = "Load Specs";
    LoadSpecMenuItem.Click += LoadSpecMenuItem_Click;
    // 
    // OpenFileMenuItem
    // 
    OpenFileMenuItem.Name = "OpenFileMenuItem";
    OpenFileMenuItem.Size = new Size(197, 24);
    OpenFileMenuItem.Text = "Open File to Parse";
    OpenFileMenuItem.Click += OpenFileMenuItem_Click;
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
    // ExitMenuItem
    // 
    ExitMenuItem.Name = "ExitMenuItem";
    ExitMenuItem.Size = new Size(197, 24);
    ExitMenuItem.Text = "Exit";
    ExitMenuItem.Click += ExitMenuItem_Click;
    // 
    // SpecComboBox
    // 
    SpecComboBox.AccessibleRole = AccessibleRole.ComboBox;
    SpecComboBox.DataBindings.Add(new Binding("Text", SpecBindingSource, "Name", true));
    SpecComboBox.DataBindings.Add(new Binding("SelectedItem", SpecBindingSource, "Name", true));
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(7, 42);
    SpecComboBox.Margin = new Padding(3, 2, 3, 2);
    SpecComboBox.Name = "SpecComboBox";
    SpecComboBox.Size = new Size(168, 23);
    SpecComboBox.TabIndex = 3;
    // 
    // SpecBindingSource
    // 
    SpecBindingSource.DataSource = typeof(Spec);
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(7, 24);
    label1.Name = "label1";
    label1.Size = new Size(75, 15);
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
    // ParseButton
    // 
    ParseButton.Location = new Point(735, 234);
    ParseButton.Margin = new Padding(3, 2, 3, 2);
    ParseButton.Name = "ParseButton";
    ParseButton.Size = new Size(91, 24);
    ParseButton.TabIndex = 6;
    ParseButton.Text = "Parse";
    ParseButton.UseVisualStyleBackColor = true;
    ParseButton.Click += Button1_Click;
    // 
    // ItemTabs
    // 
    ItemTabs.Controls.Add(tabPage1);
    ItemTabs.Controls.Add(AssemblerPage);
    ItemTabs.Location = new Point(196, 24);
    ItemTabs.Margin = new Padding(3, 2, 3, 2);
    ItemTabs.Name = "ItemTabs";
    ItemTabs.SelectedIndex = 0;
    ItemTabs.Size = new Size(847, 294);
    ItemTabs.TabIndex = 7;
    // 
    // tabPage1
    // 
    tabPage1.Controls.Add(TokenGridView);
    tabPage1.Controls.Add(TokenRuleDataGrid);
    tabPage1.Controls.Add(ParseButton);
    tabPage1.Location = new Point(4, 24);
    tabPage1.Margin = new Padding(3, 2, 3, 2);
    tabPage1.Name = "tabPage1";
    tabPage1.Padding = new Padding(3, 2, 3, 2);
    tabPage1.Size = new Size(839, 266);
    tabPage1.TabIndex = 0;
    tabPage1.Text = "Tokenizer";
    tabPage1.UseVisualStyleBackColor = true;
    // 
    // TokenGridView
    // 
    TokenGridView.AutoGenerateColumns = false;
    TokenGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenGridView.Columns.AddRange(new DataGridViewColumn[] { contentDataGridViewTextBoxColumn, ignoredDataGridViewCheckBoxColumn, exemptDataGridViewCheckBoxColumn, typeDataGridViewTextBoxColumn1, childrenDataGridViewTextBoxColumn, hasTypeDataGridViewCheckBoxColumn });
    TokenGridView.DataSource = tokenBindingSource;
    TokenGridView.Location = new Point(464, 16);
    TokenGridView.Name = "TokenGridView";
    TokenGridView.RowHeadersWidth = 51;
    TokenGridView.Size = new Size(427, 216);
    TokenGridView.TabIndex = 8;
    // 
    // contentDataGridViewTextBoxColumn
    // 
    contentDataGridViewTextBoxColumn.DataPropertyName = "Content";
    contentDataGridViewTextBoxColumn.HeaderText = "Content";
    contentDataGridViewTextBoxColumn.MinimumWidth = 6;
    contentDataGridViewTextBoxColumn.Name = "contentDataGridViewTextBoxColumn";
    contentDataGridViewTextBoxColumn.ReadOnly = true;
    contentDataGridViewTextBoxColumn.Width = 125;
    // 
    // ignoredDataGridViewCheckBoxColumn
    // 
    ignoredDataGridViewCheckBoxColumn.DataPropertyName = "Ignored";
    ignoredDataGridViewCheckBoxColumn.HeaderText = "Ignored";
    ignoredDataGridViewCheckBoxColumn.MinimumWidth = 6;
    ignoredDataGridViewCheckBoxColumn.Name = "ignoredDataGridViewCheckBoxColumn";
    ignoredDataGridViewCheckBoxColumn.ReadOnly = true;
    ignoredDataGridViewCheckBoxColumn.Width = 125;
    // 
    // exemptDataGridViewCheckBoxColumn
    // 
    exemptDataGridViewCheckBoxColumn.DataPropertyName = "Exempt";
    exemptDataGridViewCheckBoxColumn.HeaderText = "Exempt";
    exemptDataGridViewCheckBoxColumn.MinimumWidth = 6;
    exemptDataGridViewCheckBoxColumn.Name = "exemptDataGridViewCheckBoxColumn";
    exemptDataGridViewCheckBoxColumn.Width = 125;
    // 
    // typeDataGridViewTextBoxColumn1
    // 
    typeDataGridViewTextBoxColumn1.DataPropertyName = "Type";
    typeDataGridViewTextBoxColumn1.HeaderText = "Type";
    typeDataGridViewTextBoxColumn1.MinimumWidth = 6;
    typeDataGridViewTextBoxColumn1.Name = "typeDataGridViewTextBoxColumn1";
    typeDataGridViewTextBoxColumn1.Width = 125;
    // 
    // childrenDataGridViewTextBoxColumn
    // 
    childrenDataGridViewTextBoxColumn.DataPropertyName = "Children";
    childrenDataGridViewTextBoxColumn.HeaderText = "Children";
    childrenDataGridViewTextBoxColumn.MinimumWidth = 6;
    childrenDataGridViewTextBoxColumn.Name = "childrenDataGridViewTextBoxColumn";
    childrenDataGridViewTextBoxColumn.Width = 125;
    // 
    // hasTypeDataGridViewCheckBoxColumn
    // 
    hasTypeDataGridViewCheckBoxColumn.DataPropertyName = "HasType";
    hasTypeDataGridViewCheckBoxColumn.HeaderText = "HasType";
    hasTypeDataGridViewCheckBoxColumn.MinimumWidth = 6;
    hasTypeDataGridViewCheckBoxColumn.Name = "hasTypeDataGridViewCheckBoxColumn";
    hasTypeDataGridViewCheckBoxColumn.ReadOnly = true;
    hasTypeDataGridViewCheckBoxColumn.Width = 125;
    // 
    // tokenBindingSource
    // 
    tokenBindingSource.DataSource = typeof(Parser.Tokens.IToken);
    // 
    // TokenRuleDataGrid
    // 
    TokenRuleDataGrid.AutoGenerateColumns = false;
    TokenRuleDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenRuleDataGrid.Columns.AddRange(new DataGridViewColumn[] { typeDataGridViewTextBoxColumn, ruleStringDataDataGridViewTextBoxColumn, typeToAssignDataGridViewTextBoxColumn1 });
    TokenRuleDataGrid.DataSource = TokenRuleBindingSource;
    TokenRuleDataGrid.GridColor = SystemColors.MenuText;
    TokenRuleDataGrid.Location = new Point(7, 12);
    TokenRuleDataGrid.Margin = new Padding(3, 2, 3, 2);
    TokenRuleDataGrid.MultiSelect = false;
    TokenRuleDataGrid.Name = "TokenRuleDataGrid";
    TokenRuleDataGrid.RowHeadersWidth = 51;
    TokenRuleDataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
    TokenRuleDataGrid.Size = new Size(392, 216);
    TokenRuleDataGrid.TabIndex = 7;
    // 
    // typeDataGridViewTextBoxColumn
    // 
    typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
    typeDataGridViewTextBoxColumn.HeaderText = "Type";
    typeDataGridViewTextBoxColumn.MinimumWidth = 6;
    typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
    typeDataGridViewTextBoxColumn.Width = 125;
    // 
    // ruleStringDataDataGridViewTextBoxColumn
    // 
    ruleStringDataDataGridViewTextBoxColumn.DataPropertyName = "RuleStringData";
    ruleStringDataDataGridViewTextBoxColumn.HeaderText = "RuleStringData";
    ruleStringDataDataGridViewTextBoxColumn.MinimumWidth = 6;
    ruleStringDataDataGridViewTextBoxColumn.Name = "ruleStringDataDataGridViewTextBoxColumn";
    ruleStringDataDataGridViewTextBoxColumn.Width = 125;
    // 
    // typeToAssignDataGridViewTextBoxColumn1
    // 
    typeToAssignDataGridViewTextBoxColumn1.DataPropertyName = "TypeToAssign";
    typeToAssignDataGridViewTextBoxColumn1.HeaderText = "TypeToAssign";
    typeToAssignDataGridViewTextBoxColumn1.MinimumWidth = 6;
    typeToAssignDataGridViewTextBoxColumn1.Name = "typeToAssignDataGridViewTextBoxColumn1";
    typeToAssignDataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.True;
    typeToAssignDataGridViewTextBoxColumn1.Width = 125;
    // 
    // AssemblerPage
    // 
    AssemblerPage.Controls.Add(DataDictionaryGrid);
    AssemblerPage.Location = new Point(4, 29);
    AssemblerPage.Name = "AssemblerPage";
    AssemblerPage.Padding = new Padding(3, 2, 3, 2);
    AssemblerPage.Size = new Size(839, 266);
    AssemblerPage.TabIndex = 1;
    AssemblerPage.Text = "Assembler";
    AssemblerPage.UseVisualStyleBackColor = true;
    // 
    // LoadRulesButton
    // 
    LoadRulesButton.Location = new Point(96, 88);
    LoadRulesButton.Name = "LoadRulesButton";
    LoadRulesButton.Size = new Size(91, 24);
    LoadRulesButton.TabIndex = 8;
    LoadRulesButton.Text = "Load Rules";
    LoadRulesButton.UseVisualStyleBackColor = true;
    LoadRulesButton.Click += LoadRulesButton_Click;
    // 
    // readOnlyDictionaryGrid1
    // 
    readOnlyDictionaryGrid1.Location = new Point(60, 245);
    readOnlyDictionaryGrid1.Name = "readOnlyDictionaryGrid1";
    readOnlyDictionaryGrid1.Size = new Size(416, 216);
    readOnlyDictionaryGrid1.TabIndex = 9;
    // 
    // DataDictionaryGrid
    // 
    DataDictionaryGrid.Dock = DockStyle.Fill;
    DataDictionaryGrid.Location = new Point(3, 3);
    DataDictionaryGrid.Name = "DataDictionaryGrid";
    DataDictionaryGrid.Size = new Size(954, 353);
    DataDictionaryGrid.TabIndex = 0;
    // 
    // checkBox1
    // 
    checkBox1.AutoSize = true;
    checkBox1.Location = new Point(16, 392);
    checkBox1.Name = "checkBox1";
    checkBox1.Size = new Size(120, 24);
    checkBox1.TabIndex = 10;
    checkBox1.Text = "checkBox1";
    checkBox1.UseVisualStyleBackColor = true;
    // 
    // checkBox2
    // 
    checkBox2.AutoSize = true;
    checkBox2.Location = new Point(16, 368);
    checkBox2.Name = "checkBox2";
    checkBox2.Size = new Size(120, 24);
    checkBox2.TabIndex = 10;
    checkBox2.Text = "checkBox1";
    checkBox2.UseVisualStyleBackColor = true;
    // 
    // checkBox3
    // 
    checkBox3.AutoSize = true;
    checkBox3.Location = new Point(16, 344);
    checkBox3.Name = "checkBox3";
    checkBox3.Size = new Size(130, 24);
    checkBox3.TabIndex = 10;
    checkBox3.Text = "Parser Created";
    checkBox3.UseVisualStyleBackColor = true;
    // 
    // SpecLoadedCheck
    // 
    SpecLoadedCheck.AutoSize = true;
    SpecLoadedCheck.Location = new Point(16, 320);
    SpecLoadedCheck.Name = "SpecLoadedCheck";
    SpecLoadedCheck.Size = new Size(120, 24);
    SpecLoadedCheck.TabIndex = 10;
    SpecLoadedCheck.Text = "Spec Loaded";
    SpecLoadedCheck.UseVisualStyleBackColor = true;
    // 
    // button1
    // 
    button1.Location = new Point(96, 184);
    button1.Name = "button1";
    button1.Size = new Size(104, 32);
    button1.TabIndex = 8;
    button1.Text = "Load Input";
    button1.UseVisualStyleBackColor = true;
    button1.Click += LoadRulesButton_Click;
    // 
    // textBox1
    // 
    textBox1.Location = new Point(8, 152);
    textBox1.Name = "textBox1";
    textBox1.Size = new Size(192, 27);
    textBox1.TabIndex = 11;
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(7F, 15F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1214, 503);
    Controls.Add(textBox1);
    Controls.Add(SpecLoadedCheck);
    Controls.Add(checkBox3);
    Controls.Add(checkBox2);
    Controls.Add(checkBox1);
    Controls.Add(readOnlyDictionaryGrid1);
    Controls.Add(button1);
    Controls.Add(LoadRulesButton);
    Controls.Add(ItemTabs);
    Controls.Add(label1);
    Controls.Add(SpecComboBox);
    Controls.Add(StatusStrip);
    Controls.Add(TheMenuStrip);
    MainMenuStrip = TheMenuStrip;
    Margin = new Padding(3, 2, 3, 2);
    Name = "ParserForm";
    Text = "Parser Form";
    Load += ParserForm_Load;
    TheMenuStrip.ResumeLayout(false);
    TheMenuStrip.PerformLayout();
    ((ISupportInitialize) SpecBindingSource).EndInit();
    ((ISupportInitialize) TokenRuleBindingSource).EndInit();
    ItemTabs.ResumeLayout(false);
    tabPage1.ResumeLayout(false);
    ((ISupportInitialize) TokenGridView).EndInit();
    ((ISupportInitialize) tokenBindingSource).EndInit();
    ((ISupportInitialize) TokenRuleDataGrid).EndInit();
    AssemblerPage.ResumeLayout(false);
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private StatusStrip StatusStrip;
  private OpenFileDialog OpenParseFileDialog;
  private MenuStrip TheMenuStrip;
  private ComboBox SpecComboBox;
  private Label label1;
  private BindingSource TokenRuleBindingSource;
  private DataGridViewComboBoxColumn TypeColumn;
  private DataGridViewComboBoxColumn typeToAssignDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn RuleStringDataColumn;
  private Button ParseButton;
  private ToolStripMenuItem FileMenu;
  private ToolStripMenuItem LoadSpecMenuItem;
  private ToolStripMenuItem OpenFileMenuItem;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripMenuItem generateRulesToolStripMenuItem;
  private ToolStripMenuItem ExitMenuItem;
  private BindingSource SpecBindingSource;
  private TabControl ItemTabs;
  private TabPage tabPage1;
  private TabPage AssemblerPage;
  private Button LoadRulesButton;
  private DataGridView TokenGridView;
  private DataGridView TokenRuleDataGrid;
  private DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn ruleStringDataDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn typeToAssignDataGridViewTextBoxColumn1;
  private DataGridViewTextBoxColumn contentDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn lastPositionDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
  private DataGridViewCheckBoxColumn ignoredDataGridViewCheckBoxColumn;
  private DataGridViewCheckBoxColumn exemptDataGridViewCheckBoxColumn;
  private DataGridViewTextBoxColumn indexDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn1;
  private DataGridViewTextBoxColumn childrenDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
  private DataGridViewCheckBoxColumn hasTypeDataGridViewCheckBoxColumn;
  private BindingSource tokenBindingSource;
  private Parser.Forms.ReadOnlyDictionaryGrid readOnlyDictionaryGrid1;
  private Parser.Forms.ReadOnlyDictionaryGrid DataDictionaryGrid;
  private CheckBox checkBox1;
  private CheckBox checkBox2;
  private CheckBox checkBox3;
  private CheckBox SpecLoadedCheck;
  private Button button1;
  private TextBox textBox1;
}
