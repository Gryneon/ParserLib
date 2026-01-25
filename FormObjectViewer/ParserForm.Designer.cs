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
    DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
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
    SpecLabel = new Label();
    TypeColumn = new DataGridViewComboBoxColumn();
    TypeToAssignColumn = new DataGridViewComboBoxColumn();
    RuleStringDataColumn = new DataGridViewTextBoxColumn();
    TokenRuleBindingSource = new BindingSource(components);
    ParseButton = new Button();
    ItemTabs = new TabControl();
    tabPage1 = new TabPage();
    label2 = new Label();
    TokenCountLabel = new Label();
    TokenRuleCountLabel = new Label();
    label1 = new Label();
    TokenGridView = new DataGridView();
    contentDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    ignoredDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    exemptDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    typeDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    childrenDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    hasTypeDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
    TokenBindingSource = new BindingSource(components);
    TokenRuleDataGrid = new DataGridView();
    AssemblerPage = new TabPage();
    LoadRulesButton = new Button();
    TypeGridColumnCombo = new DataGridViewComboBoxColumn();
    ruleStringDataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
    typeToAssignDataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
    TheMenuStrip.SuspendLayout();
    ((ISupportInitialize) SpecBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleBindingSource).BeginInit();
    ItemTabs.SuspendLayout();
    tabPage1.SuspendLayout();
    ((ISupportInitialize) TokenGridView).BeginInit();
    ((ISupportInitialize) TokenBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleDataGrid).BeginInit();
    SuspendLayout();
    // 
    // StatusStrip
    // 
    StatusStrip.ImageScalingSize = new Size(20, 20);
    StatusStrip.Location = new Point(0, 606);
    StatusStrip.Name = "StatusStrip";
    StatusStrip.Padding = new Padding(1, 0, 17, 0);
    StatusStrip.Size = new Size(1517, 22);
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
    // TheMenuStrip
    // 
    TheMenuStrip.ImageScalingSize = new Size(20, 20);
    TheMenuStrip.Items.AddRange(new ToolStripItem[] { FileMenu });
    TheMenuStrip.Location = new Point(0, 0);
    TheMenuStrip.Name = "TheMenuStrip";
    TheMenuStrip.Padding = new Padding(7, 3, 0, 3);
    TheMenuStrip.Size = new Size(1517, 35);
    TheMenuStrip.TabIndex = 2;
    TheMenuStrip.Text = "menuStrip1";
    // 
    // FileMenu
    // 
    FileMenu.DropDownItems.AddRange(new ToolStripItem[] { LoadSpecMenuItem, OpenFileMenuItem, generateRulesToolStripMenuItem, toolStripSeparator1, ExitMenuItem });
    FileMenu.Name = "FileMenu";
    FileMenu.Size = new Size(50, 29);
    FileMenu.Text = "File";
    // 
    // LoadSpecMenuItem
    // 
    LoadSpecMenuItem.Name = "LoadSpecMenuItem";
    LoadSpecMenuItem.Size = new Size(227, 30);
    LoadSpecMenuItem.Text = "Load Specs";
    LoadSpecMenuItem.Click += LoadSpecMenuItem_Click;
    // 
    // OpenFileMenuItem
    // 
    OpenFileMenuItem.Name = "OpenFileMenuItem";
    OpenFileMenuItem.Size = new Size(227, 30);
    OpenFileMenuItem.Text = "Open File to Parse";
    OpenFileMenuItem.Click += OpenFileMenuItem_Click;
    // 
    // generateRulesToolStripMenuItem
    // 
    generateRulesToolStripMenuItem.Name = "generateRulesToolStripMenuItem";
    generateRulesToolStripMenuItem.Size = new Size(227, 30);
    generateRulesToolStripMenuItem.Text = "Generate Rules";
    // 
    // toolStripSeparator1
    // 
    toolStripSeparator1.Name = "toolStripSeparator1";
    toolStripSeparator1.Size = new Size(224, 6);
    // 
    // ExitMenuItem
    // 
    ExitMenuItem.Name = "ExitMenuItem";
    ExitMenuItem.Size = new Size(227, 30);
    ExitMenuItem.Text = "Exit";
    ExitMenuItem.Click += ExitMenuItem_Click;
    // 
    // SpecComboBox
    // 
    SpecComboBox.AccessibleRole = AccessibleRole.ComboBox;
    SpecComboBox.DataBindings.Add(new Binding("Text", SpecBindingSource, "Name", true));
    SpecComboBox.DataBindings.Add(new Binding("SelectedItem", SpecBindingSource, "Name", true));
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(10, 70);
    SpecComboBox.Margin = new Padding(4, 3, 4, 3);
    SpecComboBox.Name = "SpecComboBox";
    SpecComboBox.Size = new Size(238, 33);
    SpecComboBox.TabIndex = 3;
    // 
    // SpecBindingSource
    // 
    SpecBindingSource.DataSource = typeof(Spec);
    // 
    // SpecLabel
    // 
    SpecLabel.AutoSize = true;
    SpecLabel.Location = new Point(10, 40);
    SpecLabel.Margin = new Padding(4, 0, 4, 0);
    SpecLabel.Name = "SpecLabel";
    SpecLabel.Size = new Size(112, 25);
    SpecLabel.TabIndex = 4;
    SpecLabel.Text = "Specification";
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
    // TypeToAssignColumn
    // 
    TypeToAssignColumn.DataPropertyName = "TypeToAssign";
    TypeToAssignColumn.HeaderText = "TypeToAssign";
    TypeToAssignColumn.MinimumWidth = 6;
    TypeToAssignColumn.Name = "TypeToAssignColumn";
    TypeToAssignColumn.Resizable = DataGridViewTriState.True;
    TypeToAssignColumn.SortMode = DataGridViewColumnSortMode.Automatic;
    TypeToAssignColumn.Width = 125;
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
    ParseButton.Location = new Point(1048, 424);
    ParseButton.Margin = new Padding(4, 3, 4, 3);
    ParseButton.Name = "ParseButton";
    ParseButton.Size = new Size(130, 40);
    ParseButton.TabIndex = 6;
    ParseButton.Text = "Parse";
    ParseButton.UseVisualStyleBackColor = true;
    ParseButton.Click += Button1_Click;
    // 
    // ItemTabs
    // 
    ItemTabs.Controls.Add(tabPage1);
    ItemTabs.Controls.Add(AssemblerPage);
    ItemTabs.Location = new Point(280, 40);
    ItemTabs.Margin = new Padding(4, 3, 4, 3);
    ItemTabs.Name = "ItemTabs";
    ItemTabs.SelectedIndex = 0;
    ItemTabs.Size = new Size(1210, 528);
    ItemTabs.TabIndex = 7;
    // 
    // tabPage1
    // 
    tabPage1.Controls.Add(label2);
    tabPage1.Controls.Add(TokenCountLabel);
    tabPage1.Controls.Add(TokenRuleCountLabel);
    tabPage1.Controls.Add(label1);
    tabPage1.Controls.Add(TokenGridView);
    tabPage1.Controls.Add(TokenRuleDataGrid);
    tabPage1.Controls.Add(ParseButton);
    tabPage1.Location = new Point(4, 34);
    tabPage1.Margin = new Padding(4, 3, 4, 3);
    tabPage1.Name = "tabPage1";
    tabPage1.Padding = new Padding(4, 3, 4, 3);
    tabPage1.Size = new Size(1202, 490);
    tabPage1.TabIndex = 0;
    tabPage1.Text = "Tokenizer";
    tabPage1.UseVisualStyleBackColor = true;
    // 
    // label2
    // 
    label2.AutoSize = true;
    label2.Location = new Point(584, 8);
    label2.Margin = new Padding(4, 0, 4, 0);
    label2.Name = "label2";
    label2.Size = new Size(123, 25);
    label2.TabIndex = 9;
    label2.Text = "Tokens Parsed";
    // 
    // TokenCountLabel
    // 
    TokenCountLabel.AutoSize = true;
    TokenCountLabel.Location = new Point(720, 8);
    TokenCountLabel.Margin = new Padding(4, 0, 4, 0);
    TokenCountLabel.Name = "TokenCountLabel";
    TokenCountLabel.Size = new Size(105, 25);
    TokenCountLabel.TabIndex = 9;
    TokenCountLabel.Text = "Token Rules";
    // 
    // TokenRuleCountLabel
    // 
    TokenRuleCountLabel.AutoSize = true;
    TokenRuleCountLabel.Location = new Point(120, 8);
    TokenRuleCountLabel.Margin = new Padding(4, 0, 4, 0);
    TokenRuleCountLabel.Name = "TokenRuleCountLabel";
    TokenRuleCountLabel.Size = new Size(0, 25);
    TokenRuleCountLabel.TabIndex = 9;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(8, 8);
    label1.Margin = new Padding(4, 0, 4, 0);
    label1.Name = "label1";
    label1.Size = new Size(105, 25);
    label1.TabIndex = 9;
    label1.Text = "Token Rules";
    // 
    // TokenGridView
    // 
    TokenGridView.AutoGenerateColumns = false;
    TokenGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenGridView.Columns.AddRange(new DataGridViewColumn[] { contentDataGridViewTextBoxColumn, ignoredDataGridViewCheckBoxColumn, exemptDataGridViewCheckBoxColumn, typeDataGridViewTextBoxColumn1, childrenDataGridViewTextBoxColumn, hasTypeDataGridViewCheckBoxColumn });
    TokenGridView.DataSource = TokenBindingSource;
    TokenGridView.Location = new Point(580, 48);
    TokenGridView.Margin = new Padding(4, 3, 4, 3);
    TokenGridView.Name = "TokenGridView";
    TokenGridView.RowHeadersWidth = 51;
    TokenGridView.Size = new Size(610, 360);
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
    // TokenBindingSource
    // 
    TokenBindingSource.DataSource = typeof(Parser.Tokens.IToken);
    // 
    // TokenRuleDataGrid
    // 
    TokenRuleDataGrid.AutoGenerateColumns = false;
    TokenRuleDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenRuleDataGrid.Columns.AddRange(new DataGridViewColumn[] { TypeGridColumnCombo, ruleStringDataDataGridViewTextBoxColumn, typeToAssignDataGridViewTextBoxColumn1 });
    TokenRuleDataGrid.DataSource = TokenRuleBindingSource;
    TokenRuleDataGrid.EditMode = DataGridViewEditMode.EditOnKeystroke;
    TokenRuleDataGrid.GridColor = SystemColors.MenuText;
    TokenRuleDataGrid.Location = new Point(8, 48);
    TokenRuleDataGrid.Margin = new Padding(4, 3, 4, 3);
    TokenRuleDataGrid.MultiSelect = false;
    TokenRuleDataGrid.Name = "TokenRuleDataGrid";
    TokenRuleDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
    TokenRuleDataGrid.RowHeadersVisible = false;
    TokenRuleDataGrid.RowHeadersWidth = 51;
    dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
    dataGridViewCellStyle1.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
    TokenRuleDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
    TokenRuleDataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
    TokenRuleDataGrid.Size = new Size(560, 360);
    TokenRuleDataGrid.TabIndex = 7;
    TokenRuleDataGrid.RowValidated += TokenRuleDataGrid_RowValidated;
    // 
    // AssemblerPage
    // 
    AssemblerPage.Location = new Point(4, 34);
    AssemblerPage.Margin = new Padding(4, 3, 4, 3);
    AssemblerPage.Name = "AssemblerPage";
    AssemblerPage.Padding = new Padding(4, 3, 4, 3);
    AssemblerPage.Size = new Size(1202, 490);
    AssemblerPage.TabIndex = 1;
    AssemblerPage.Text = "Assembler";
    AssemblerPage.UseVisualStyleBackColor = true;
    // 
    // LoadRulesButton
    // 
    LoadRulesButton.Location = new Point(120, 120);
    LoadRulesButton.Margin = new Padding(4, 3, 4, 3);
    LoadRulesButton.Name = "LoadRulesButton";
    LoadRulesButton.Size = new Size(130, 40);
    LoadRulesButton.TabIndex = 8;
    LoadRulesButton.Text = "Load Rules";
    LoadRulesButton.UseVisualStyleBackColor = true;
    LoadRulesButton.Click += LoadRulesButton_Click;
    // 
    // TypeGridColumnCombo
    // 
    TypeGridColumnCombo.DataPropertyName = "Type";
    TypeGridColumnCombo.HeaderText = "Type";
    TypeGridColumnCombo.Items.AddRange(new object[] { "TokenMatch", "TokenExact", "", "SplitMatch", "SplitExact", "ErrorMatch", "TokenExtract", "StoreExtra", "StoreOther" });
    TypeGridColumnCombo.MinimumWidth = 6;
    TypeGridColumnCombo.Name = "TypeGridColumnCombo";
    TypeGridColumnCombo.Resizable = DataGridViewTriState.True;
    TypeGridColumnCombo.SortMode = DataGridViewColumnSortMode.Automatic;
    TypeGridColumnCombo.Width = 125;
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
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(10F, 25F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1517, 628);
    Controls.Add(LoadRulesButton);
    Controls.Add(ItemTabs);
    Controls.Add(SpecLabel);
    Controls.Add(SpecComboBox);
    Controls.Add(StatusStrip);
    Controls.Add(TheMenuStrip);
    MainMenuStrip = TheMenuStrip;
    Margin = new Padding(4, 3, 4, 3);
    Name = "ParserForm";
    Text = "Parser Form";
    Load += ParserForm_Load;
    TheMenuStrip.ResumeLayout(false);
    TheMenuStrip.PerformLayout();
    ((ISupportInitialize) SpecBindingSource).EndInit();
    ((ISupportInitialize) TokenRuleBindingSource).EndInit();
    ItemTabs.ResumeLayout(false);
    tabPage1.ResumeLayout(false);
    tabPage1.PerformLayout();
    ((ISupportInitialize) TokenGridView).EndInit();
    ((ISupportInitialize) TokenBindingSource).EndInit();
    ((ISupportInitialize) TokenRuleDataGrid).EndInit();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private StatusStrip StatusStrip;
  private OpenFileDialog OpenParseFileDialog;
  private MenuStrip TheMenuStrip;
  private ComboBox SpecComboBox;
  private Label SpecLabel;
  private BindingSource TokenRuleBindingSource;
  private DataGridViewComboBoxColumn TypeColumn;
  private DataGridViewComboBoxColumn TypeToAssignColumn;
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
  private BindingSource TokenBindingSource;
  private Label label2;
  private Label TokenCountLabel;
  private Label TokenRuleCountLabel;
  private Label label1;
  private DataGridViewComboBoxColumn TypeGridColumnCombo;
  private DataGridViewTextBoxColumn ruleStringDataDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn typeToAssignDataGridViewTextBoxColumn1;
}
