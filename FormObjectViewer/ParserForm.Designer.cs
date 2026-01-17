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
    ParseButton = new Button();
    ItemTabs = new TabControl();
    tabPage1 = new TabPage();
    TokenGridView = new DataGridView();
    TokenRuleDataGrid = new DataGridView();
    typeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    ruleStringDataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    typeToAssignDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    AssemblerPage = new TabPage();
    LoadRulesButton = new Button();
    tokenBindingSource = new BindingSource(components);
    contentDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    lastPositionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    lengthDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    ignoredDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    exemptDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    indexDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    typeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    childrenDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    countDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    hasTypeDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    menuStrip1.SuspendLayout();
    ((ISupportInitialize) SpecBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleBindingSource).BeginInit();
    ItemTabs.SuspendLayout();
    tabPage1.SuspendLayout();
    ((ISupportInitialize) TokenGridView).BeginInit();
    ((ISupportInitialize) TokenRuleDataGrid).BeginInit();
    ((ISupportInitialize) tokenBindingSource).BeginInit();
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
    FileMenu.DropDownItems.AddRange(new ToolStripItem[] { LoadSpecMenuItem, OpenFileMenuItem, generateRulesToolStripMenuItem, toolStripSeparator1, ExitMenuItem });
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
    OpenFileMenuItem.Click += OpenFileMenuItem_Click;
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
    ExitMenuItem.Click += ExitMenuItem_Click;
    // 
    // SpecComboBox
    // 
    SpecComboBox.AccessibleRole = AccessibleRole.ComboBox;
    SpecComboBox.DataBindings.Add(new Binding("Text", SpecBindingSource, "Name", true));
    SpecComboBox.DataBindings.Add(new Binding("SelectedItem", SpecBindingSource, "Name", true));
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(8, 56);
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
    label1.Location = new Point(8, 32);
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
    // ParseButton
    // 
    ParseButton.Location = new Point(840, 312);
    ParseButton.Name = "ParseButton";
    ParseButton.Size = new Size(104, 32);
    ParseButton.TabIndex = 6;
    ParseButton.Text = "Parse";
    ParseButton.UseVisualStyleBackColor = true;
    ParseButton.Click += Button1_Click;
    // 
    // ItemTabs
    // 
    ItemTabs.Controls.Add(tabPage1);
    ItemTabs.Controls.Add(AssemblerPage);
    ItemTabs.Location = new Point(224, 32);
    ItemTabs.Name = "ItemTabs";
    ItemTabs.SelectedIndex = 0;
    ItemTabs.Size = new Size(968, 392);
    ItemTabs.TabIndex = 7;
    // 
    // tabPage1
    // 
    tabPage1.Controls.Add(TokenGridView);
    tabPage1.Controls.Add(TokenRuleDataGrid);
    tabPage1.Controls.Add(ParseButton);
    tabPage1.Location = new Point(4, 29);
    tabPage1.Name = "tabPage1";
    tabPage1.Padding = new Padding(3);
    tabPage1.Size = new Size(960, 359);
    tabPage1.TabIndex = 0;
    tabPage1.Text = "Tokenizer";
    tabPage1.UseVisualStyleBackColor = true;
    // 
    // TokenGridView
    // 
    TokenGridView.AutoGenerateColumns = false;
    TokenGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenGridView.Columns.AddRange(new DataGridViewColumn[] { contentDataGridViewTextBoxColumn, lastPositionDataGridViewTextBoxColumn, lengthDataGridViewTextBoxColumn, ignoredDataGridViewCheckBoxColumn, exemptDataGridViewCheckBoxColumn, indexDataGridViewTextBoxColumn, typeDataGridViewTextBoxColumn1, childrenDataGridViewTextBoxColumn, countDataGridViewTextBoxColumn, hasTypeDataGridViewCheckBoxColumn });
    TokenGridView.DataSource = tokenBindingSource;
    TokenGridView.Location = new Point(464, 16);
    TokenGridView.Name = "TokenGridView";
    TokenGridView.RowHeadersWidth = 51;
    TokenGridView.Size = new Size(488, 288);
    TokenGridView.TabIndex = 8;
    // 
    // TokenRuleDataGrid
    // 
    TokenRuleDataGrid.AutoGenerateColumns = false;
    TokenRuleDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenRuleDataGrid.Columns.AddRange(new DataGridViewColumn[] { typeDataGridViewTextBoxColumn, ruleStringDataDataGridViewTextBoxColumn, typeToAssignDataGridViewTextBoxColumn1 });
    TokenRuleDataGrid.DataSource = TokenRuleBindingSource;
    TokenRuleDataGrid.GridColor = SystemColors.MenuText;
    TokenRuleDataGrid.Location = new Point(8, 16);
    TokenRuleDataGrid.MultiSelect = false;
    TokenRuleDataGrid.Name = "TokenRuleDataGrid";
    TokenRuleDataGrid.RowHeadersWidth = 51;
    TokenRuleDataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
    TokenRuleDataGrid.Size = new Size(448, 288);
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
    AssemblerPage.Location = new Point(4, 29);
    AssemblerPage.Name = "AssemblerPage";
    AssemblerPage.Padding = new Padding(3);
    AssemblerPage.Size = new Size(960, 359);
    AssemblerPage.TabIndex = 1;
    AssemblerPage.Text = "Assembler";
    AssemblerPage.UseVisualStyleBackColor = true;
    // 
    // LoadRulesButton
    // 
    LoadRulesButton.Location = new Point(96, 96);
    LoadRulesButton.Name = "LoadRulesButton";
    LoadRulesButton.Size = new Size(104, 32);
    LoadRulesButton.TabIndex = 8;
    LoadRulesButton.Text = "Load Rules";
    LoadRulesButton.UseVisualStyleBackColor = true;
    LoadRulesButton.Click += LoadRulesButton_Click;
    // 
    // tokenBindingSource
    // 
    tokenBindingSource.DataSource = typeof(Parser.Tokens.IToken);
    // 
    // contentDataGridViewTextBoxColumn
    // 
    contentDataGridViewTextBoxColumn.DataPropertyName = "Content";
    contentDataGridViewTextBoxColumn.HeaderText = "Content";
    contentDataGridViewTextBoxColumn.MinimumWidth = 6;
    contentDataGridViewTextBoxColumn.Name = "contentDataGridViewTextBoxColumn";
    contentDataGridViewTextBoxColumn.Width = 125;
    // 
    // lastPositionDataGridViewTextBoxColumn
    // 
    lastPositionDataGridViewTextBoxColumn.DataPropertyName = "LastPosition";
    lastPositionDataGridViewTextBoxColumn.HeaderText = "LastPosition";
    lastPositionDataGridViewTextBoxColumn.MinimumWidth = 6;
    lastPositionDataGridViewTextBoxColumn.Name = "lastPositionDataGridViewTextBoxColumn";
    lastPositionDataGridViewTextBoxColumn.ReadOnly = true;
    lastPositionDataGridViewTextBoxColumn.Width = 125;
    // 
    // lengthDataGridViewTextBoxColumn
    // 
    lengthDataGridViewTextBoxColumn.DataPropertyName = "Length";
    lengthDataGridViewTextBoxColumn.HeaderText = "Length";
    lengthDataGridViewTextBoxColumn.MinimumWidth = 6;
    lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
    lengthDataGridViewTextBoxColumn.ReadOnly = true;
    lengthDataGridViewTextBoxColumn.Width = 125;
    // 
    // ignoredDataGridViewCheckBoxColumn
    // 
    ignoredDataGridViewCheckBoxColumn.DataPropertyName = "Ignored";
    ignoredDataGridViewCheckBoxColumn.HeaderText = "Ignored";
    ignoredDataGridViewCheckBoxColumn.MinimumWidth = 6;
    ignoredDataGridViewCheckBoxColumn.Name = "ignoredDataGridViewCheckBoxColumn";
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
    // indexDataGridViewTextBoxColumn
    // 
    indexDataGridViewTextBoxColumn.DataPropertyName = "Index";
    indexDataGridViewTextBoxColumn.HeaderText = "Index";
    indexDataGridViewTextBoxColumn.MinimumWidth = 6;
    indexDataGridViewTextBoxColumn.Name = "indexDataGridViewTextBoxColumn";
    indexDataGridViewTextBoxColumn.Width = 125;
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
    // countDataGridViewTextBoxColumn
    // 
    countDataGridViewTextBoxColumn.DataPropertyName = "Count";
    countDataGridViewTextBoxColumn.HeaderText = "Count";
    countDataGridViewTextBoxColumn.MinimumWidth = 6;
    countDataGridViewTextBoxColumn.Name = "countDataGridViewTextBoxColumn";
    countDataGridViewTextBoxColumn.ReadOnly = true;
    countDataGridViewTextBoxColumn.Width = 125;
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
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(8F, 20F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1214, 503);
    Controls.Add(LoadRulesButton);
    Controls.Add(ItemTabs);
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
    ItemTabs.ResumeLayout(false);
    tabPage1.ResumeLayout(false);
    ((ISupportInitialize) TokenGridView).EndInit();
    ((ISupportInitialize) TokenRuleDataGrid).EndInit();
    ((ISupportInitialize) tokenBindingSource).EndInit();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private StatusStrip StatusStrip;
  private OpenFileDialog OpenParseFileDialog;
  private MenuStrip menuStrip1;
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
}
