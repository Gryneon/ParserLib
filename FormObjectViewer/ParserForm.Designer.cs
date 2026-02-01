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
    DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
    DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
    StatusStrip = new StatusStrip();
    ParseProgressBar = new ToolStripProgressBar();
    OpenParseFileDialog = new OpenFileDialog();
    TheMenuStrip = new MenuStrip();
    FileMenu = new ToolStripMenuItem();
    OpenFileMenuItem = new ToolStripMenuItem();
    ToolStripSeparator1 = new ToolStripSeparator();
    ExitMenuItem = new ToolStripMenuItem();
    SpecMenuItem = new ToolStripMenuItem();
    loadSpecsToolStripMenuItem = new ToolStripMenuItem();
    tokensToolStripMenuItem = new ToolStripMenuItem();
    clearTokenTableToolStripMenuItem = new ToolStripMenuItem();
    parseTokensToolStripMenuItem = new ToolStripMenuItem();
    RuleMenuItem = new ToolStripMenuItem();
    loadFromSpecToolStripMenuItem = new ToolStripMenuItem();
    loadFromFilToolStripMenuItem = new ToolStripMenuItem();
    toolStripSeparator2 = new ToolStripSeparator();
    saveToFileToolStripMenuItem = new ToolStripMenuItem();
    clearRuleTableToolStripMenuItem = new ToolStripMenuItem();
    SpecComboBox = new ComboBox();
    SpecBindingSource = new BindingSource(components);
    SpecLabel = new Label();
    TypeColumn = new DataGridViewComboBoxColumn();
    TypeToAssignColumn = new DataGridViewComboBoxColumn();
    RuleStringDataColumn = new DataGridViewTextBoxColumn();
    TokenRuleBindingSource = new BindingSource(components);
    ParseButton = new Button();
    ItemTabs = new TabControl();
    TokenizerPage = new TabPage();
    TokenTableLabel = new Label();
    LoadParseFileButton = new Button();
    RuleTableLabel = new Label();
    ParsePathLabel = new Label();
    ClearTokensButton = new Button();
    ParsePathTextBox = new TextBox();
    TokenLabel = new Label();
    TokenCountLabel = new Label();
    TokenRuleCountLabel = new Label();
    TokenRuleLabel = new Label();
    TokenDataGrid = new DataGridView();
    ContentTokenColumn = new DataGridViewTextBoxColumn();
    ExemptTokenColumn = new DataGridViewCheckBoxColumn();
    TypeTokenColumn = new DataGridViewTextBoxColumn();
    ChildrenTokenColumn = new DataGridViewLinkColumn();
    TokenBindingSource = new BindingSource(components);
    TokenRuleDataGrid = new DataGridView();
    StatusImageColumn = new DataGridViewImageColumn();
    TypeTextColumn = new DataGridViewTextBoxColumn();
    RuleDataColumnText = new DataGridViewTextBoxColumn();
    AssignTypeColumnText = new DataGridViewTextBoxColumn();
    AssemblerPage = new TabPage();
    SaveRuleButton = new Button();
    LoadRulesFileButton = new Button();
    ClearRulesButton = new Button();
    LoadRulesButton = new Button();
    ((ISupportInitialize) SpecBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleBindingSource).BeginInit();
    ItemTabs.SuspendLayout();
    TokenizerPage.SuspendLayout();
    ((ISupportInitialize) TokenDataGrid).BeginInit();
    ((ISupportInitialize) TokenBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleDataGrid).BeginInit();
    SuspendLayout();
    // 
    // StatusStrip
    // 
    StatusStrip.ImageScalingSize = new Size(20, 20);
    StatusStrip.Items.AddRange(new ToolStripItem[] { ParseProgressBar });
    StatusStrip.Location = new Point(0, 444);
    StatusStrip.Name = "StatusStrip";
    StatusStrip.Padding = new Padding(1, 0, 12, 0);
    StatusStrip.Size = new Size(1062, 24);
    StatusStrip.TabIndex = 0;
    StatusStrip.Text = "StatusStrip";
    // 
    // ParseProgressBar
    // 
    ParseProgressBar.Name = "ParseProgressBar";
    ParseProgressBar.Size = new Size(100, 18);
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
    TheMenuStrip.Items.AddRange(new ToolStripItem[] { FileMenu, SpecMenuItem, tokensToolStripMenuItem, RuleMenuItem });
    TheMenuStrip.Location = new Point(0, 0);
    TheMenuStrip.Name = "TheMenuStrip";
    TheMenuStrip.Padding = new Padding(5, 2, 0, 2);
    TheMenuStrip.Size = new Size(1062, 25);
    TheMenuStrip.TabIndex = 2;
    TheMenuStrip.Text = "TheMenuStrip";
    // 
    // FileMenu
    // 
    FileMenu.DropDownItems.AddRange(new ToolStripItem[] { OpenFileMenuItem, ToolStripSeparator1, ExitMenuItem });
    FileMenu.Name = "FileMenu";
    FileMenu.Size = new Size(39, 21);
    FileMenu.Text = "File";
    // 
    // OpenFileMenuItem
    // 
    OpenFileMenuItem.Name = "OpenFileMenuItem";
    OpenFileMenuItem.Size = new Size(183, 22);
    OpenFileMenuItem.Text = "Open File to Parse";
    OpenFileMenuItem.Click += OpenParseFile;
    // 
    // ToolStripSeparator1
    // 
    ToolStripSeparator1.Name = "ToolStripSeparator1";
    ToolStripSeparator1.Size = new Size(180, 6);
    // 
    // ExitMenuItem
    // 
    ExitMenuItem.Name = "ExitMenuItem";
    ExitMenuItem.Size = new Size(183, 22);
    ExitMenuItem.Text = "Exit";
    ExitMenuItem.Click += Exit;
    // 
    // SpecMenuItem
    // 
    SpecMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSpecsToolStripMenuItem });
    SpecMenuItem.Name = "SpecMenuItem";
    SpecMenuItem.Size = new Size(48, 21);
    SpecMenuItem.Text = "Spec";
    // 
    // loadSpecsToolStripMenuItem
    // 
    loadSpecsToolStripMenuItem.Name = "loadSpecsToolStripMenuItem";
    loadSpecsToolStripMenuItem.Size = new Size(143, 22);
    loadSpecsToolStripMenuItem.Text = "Load Specs";
    loadSpecsToolStripMenuItem.Click += LoadSpecList;
    // 
    // tokensToolStripMenuItem
    // 
    tokensToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clearTokenTableToolStripMenuItem, parseTokensToolStripMenuItem });
    tokensToolStripMenuItem.Name = "tokensToolStripMenuItem";
    tokensToolStripMenuItem.Size = new Size(60, 21);
    tokensToolStripMenuItem.Text = "Tokens";
    // 
    // clearTokenTableToolStripMenuItem
    // 
    clearTokenTableToolStripMenuItem.Name = "clearTokenTableToolStripMenuItem";
    clearTokenTableToolStripMenuItem.Size = new Size(180, 22);
    clearTokenTableToolStripMenuItem.Text = "Clear Token Table";
    // 
    // parseTokensToolStripMenuItem
    // 
    parseTokensToolStripMenuItem.Name = "parseTokensToolStripMenuItem";
    parseTokensToolStripMenuItem.Size = new Size(180, 22);
    parseTokensToolStripMenuItem.Text = "Parse Tokens";
    // 
    // RuleMenuItem
    // 
    RuleMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadFromSpecToolStripMenuItem, loadFromFilToolStripMenuItem, toolStripSeparator2, saveToFileToolStripMenuItem, clearRuleTableToolStripMenuItem });
    RuleMenuItem.Name = "RuleMenuItem";
    RuleMenuItem.Size = new Size(45, 21);
    RuleMenuItem.Text = "Rule";
    // 
    // loadFromSpecToolStripMenuItem
    // 
    loadFromSpecToolStripMenuItem.Name = "loadFromSpecToolStripMenuItem";
    loadFromSpecToolStripMenuItem.Size = new Size(180, 22);
    loadFromSpecToolStripMenuItem.Text = "Load From Spec";
    // 
    // loadFromFilToolStripMenuItem
    // 
    loadFromFilToolStripMenuItem.Name = "loadFromFilToolStripMenuItem";
    loadFromFilToolStripMenuItem.Size = new Size(180, 22);
    loadFromFilToolStripMenuItem.Text = "Load From File";
    // 
    // toolStripSeparator2
    // 
    toolStripSeparator2.Name = "toolStripSeparator2";
    toolStripSeparator2.Size = new Size(177, 6);
    // 
    // saveToFileToolStripMenuItem
    // 
    saveToFileToolStripMenuItem.Name = "saveToFileToolStripMenuItem";
    saveToFileToolStripMenuItem.Size = new Size(180, 22);
    saveToFileToolStripMenuItem.Text = "Save to File";
    saveToFileToolStripMenuItem.Click += SaveRuleDialog;
    // 
    // clearRuleTableToolStripMenuItem
    // 
    clearRuleTableToolStripMenuItem.Name = "clearRuleTableToolStripMenuItem";
    clearRuleTableToolStripMenuItem.Size = new Size(180, 22);
    clearRuleTableToolStripMenuItem.Text = "Clear Rule Table";
    // 
    // SpecComboBox
    // 
    SpecComboBox.AccessibleRole = AccessibleRole.ComboBox;
    SpecComboBox.DataBindings.Add(new Binding("Text", SpecBindingSource, "Name", true));
    SpecComboBox.DataBindings.Add(new Binding("SelectedItem", SpecBindingSource, "Name", true));
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(7, 48);
    SpecComboBox.Margin = new Padding(3, 2, 3, 2);
    SpecComboBox.Name = "SpecComboBox";
    SpecComboBox.Size = new Size(179, 25);
    SpecComboBox.TabIndex = 3;
    // 
    // SpecBindingSource
    // 
    SpecBindingSource.DataSource = typeof(Spec);
    // 
    // SpecLabel
    // 
    SpecLabel.AutoSize = true;
    SpecLabel.Location = new Point(7, 27);
    SpecLabel.Name = "SpecLabel";
    SpecLabel.Size = new Size(81, 17);
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
    ParseButton.Location = new Point(736, 328);
    ParseButton.Margin = new Padding(3, 2, 3, 2);
    ParseButton.Name = "ParseButton";
    ParseButton.Size = new Size(91, 27);
    ParseButton.TabIndex = 6;
    ParseButton.Text = "Parse";
    ParseButton.UseVisualStyleBackColor = true;
    ParseButton.Click += ExecuteParse;
    // 
    // ItemTabs
    // 
    ItemTabs.Controls.Add(TokenizerPage);
    ItemTabs.Controls.Add(AssemblerPage);
    ItemTabs.Location = new Point(196, 27);
    ItemTabs.Margin = new Padding(3, 2, 3, 2);
    ItemTabs.Name = "ItemTabs";
    ItemTabs.SelectedIndex = 0;
    ItemTabs.Size = new Size(847, 397);
    ItemTabs.TabIndex = 7;
    // 
    // TokenizerPage
    // 
    TokenizerPage.Controls.Add(TokenTableLabel);
    TokenizerPage.Controls.Add(LoadParseFileButton);
    TokenizerPage.Controls.Add(RuleTableLabel);
    TokenizerPage.Controls.Add(ParsePathLabel);
    TokenizerPage.Controls.Add(ClearTokensButton);
    TokenizerPage.Controls.Add(ParsePathTextBox);
    TokenizerPage.Controls.Add(TokenLabel);
    TokenizerPage.Controls.Add(TokenCountLabel);
    TokenizerPage.Controls.Add(TokenRuleCountLabel);
    TokenizerPage.Controls.Add(TokenRuleLabel);
    TokenizerPage.Controls.Add(TokenDataGrid);
    TokenizerPage.Controls.Add(TokenRuleDataGrid);
    TokenizerPage.Controls.Add(ParseButton);
    TokenizerPage.Location = new Point(4, 26);
    TokenizerPage.Margin = new Padding(3, 2, 3, 2);
    TokenizerPage.Name = "TokenizerPage";
    TokenizerPage.Padding = new Padding(3, 2, 3, 2);
    TokenizerPage.Size = new Size(839, 367);
    TokenizerPage.TabIndex = 0;
    TokenizerPage.Text = "Tokenizer";
    // 
    // TokenTableLabel
    // 
    TokenTableLabel.Font = new Font("Arial Rounded MT Bold", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
    TokenTableLabel.Location = new Point(409, 5);
    TokenTableLabel.Margin = new Padding(2, 0, 2, 0);
    TokenTableLabel.Name = "TokenTableLabel";
    TokenTableLabel.Size = new Size(135, 27);
    TokenTableLabel.TabIndex = 12;
    TokenTableLabel.Text = "Token Table";
    // 
    // LoadParseFileButton
    // 
    LoadParseFileButton.Enabled = false;
    LoadParseFileButton.FlatStyle = FlatStyle.System;
    LoadParseFileButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    LoadParseFileButton.Location = new Point(640, 288);
    LoadParseFileButton.Margin = new Padding(3, 2, 3, 2);
    LoadParseFileButton.Name = "LoadParseFileButton";
    LoadParseFileButton.Size = new Size(85, 29);
    LoadParseFileButton.TabIndex = 8;
    LoadParseFileButton.Text = "Load Parse File";
    LoadParseFileButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadParseFileButton.UseVisualStyleBackColor = true;
    LoadParseFileButton.Click += OpenParseFile;
    // 
    // RuleTableLabel
    // 
    RuleTableLabel.Font = new Font("Arial Rounded MT Bold", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
    RuleTableLabel.Location = new Point(6, 5);
    RuleTableLabel.Margin = new Padding(2, 0, 2, 0);
    RuleTableLabel.Name = "RuleTableLabel";
    RuleTableLabel.Size = new Size(122, 27);
    RuleTableLabel.TabIndex = 12;
    RuleTableLabel.Text = "Rule Table";
    // 
    // ParsePathLabel
    // 
    ParsePathLabel.AutoSize = true;
    ParsePathLabel.Location = new Point(16, 304);
    ParsePathLabel.Margin = new Padding(2, 0, 2, 0);
    ParsePathLabel.Name = "ParsePathLabel";
    ParsePathLabel.Size = new Size(123, 17);
    ParsePathLabel.TabIndex = 11;
    ParsePathLabel.Text = "Path to file to parse";
    // 
    // ClearTokensButton
    // 
    ClearTokensButton.Enabled = false;
    ClearTokensButton.FlatStyle = FlatStyle.System;
    ClearTokensButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    ClearTokensButton.Location = new Point(736, 288);
    ClearTokensButton.Margin = new Padding(3, 2, 3, 2);
    ClearTokensButton.Name = "ClearTokensButton";
    ClearTokensButton.Size = new Size(93, 29);
    ClearTokensButton.TabIndex = 8;
    ClearTokensButton.Text = "Clear Token Table";
    ClearTokensButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ClearTokensButton.UseVisualStyleBackColor = true;
    ClearTokensButton.Click += ClearTokens;
    // 
    // ParsePathTextBox
    // 
    ParsePathTextBox.Location = new Point(16, 328);
    ParsePathTextBox.Margin = new Padding(2);
    ParsePathTextBox.Name = "ParsePathTextBox";
    ParsePathTextBox.Size = new Size(712, 25);
    ParsePathTextBox.TabIndex = 10;
    // 
    // TokenLabel
    // 
    TokenLabel.AutoSize = true;
    TokenLabel.Location = new Point(656, 8);
    TokenLabel.Name = "TokenLabel";
    TokenLabel.Size = new Size(92, 17);
    TokenLabel.TabIndex = 9;
    TokenLabel.Text = "Tokens Parsed";
    // 
    // TokenCountLabel
    // 
    TokenCountLabel.Location = new Point(752, 8);
    TokenCountLabel.Name = "TokenCountLabel";
    TokenCountLabel.Size = new Size(54, 20);
    TokenCountLabel.TabIndex = 9;
    TokenCountLabel.Text = "0";
    // 
    // TokenRuleCountLabel
    // 
    TokenRuleCountLabel.Location = new Point(344, 8);
    TokenRuleCountLabel.Name = "TokenRuleCountLabel";
    TokenRuleCountLabel.Size = new Size(35, 16);
    TokenRuleCountLabel.TabIndex = 9;
    TokenRuleCountLabel.Text = "0";
    // 
    // TokenRuleLabel
    // 
    TokenRuleLabel.AutoSize = true;
    TokenRuleLabel.Location = new Point(264, 8);
    TokenRuleLabel.Name = "TokenRuleLabel";
    TokenRuleLabel.Size = new Size(77, 17);
    TokenRuleLabel.TabIndex = 9;
    TokenRuleLabel.Text = "Token Rules";
    // 
    // TokenDataGrid
    // 
    TokenDataGrid.AutoGenerateColumns = false;
    TokenDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenDataGrid.Columns.AddRange(new DataGridViewColumn[] { ContentTokenColumn, ExemptTokenColumn, TypeTokenColumn, ChildrenTokenColumn });
    TokenDataGrid.DataSource = TokenBindingSource;
    TokenDataGrid.Location = new Point(406, 33);
    TokenDataGrid.Margin = new Padding(3, 2, 3, 2);
    TokenDataGrid.Name = "TokenDataGrid";
    TokenDataGrid.RowHeadersWidth = 51;
    TokenDataGrid.Size = new Size(427, 245);
    TokenDataGrid.TabIndex = 8;
    // 
    // ContentTokenColumn
    // 
    ContentTokenColumn.DataPropertyName = "Content";
    ContentTokenColumn.Frozen = true;
    ContentTokenColumn.HeaderText = "Content";
    ContentTokenColumn.MinimumWidth = 6;
    ContentTokenColumn.Name = "ContentTokenColumn";
    ContentTokenColumn.ReadOnly = true;
    ContentTokenColumn.Width = 125;
    // 
    // ExemptTokenColumn
    // 
    ExemptTokenColumn.DataPropertyName = "Exempt";
    ExemptTokenColumn.HeaderText = "Exempt";
    ExemptTokenColumn.MinimumWidth = 6;
    ExemptTokenColumn.Name = "ExemptTokenColumn";
    ExemptTokenColumn.Width = 75;
    // 
    // TypeTokenColumn
    // 
    TypeTokenColumn.DataPropertyName = "Type";
    TypeTokenColumn.HeaderText = "Type";
    TypeTokenColumn.MinimumWidth = 6;
    TypeTokenColumn.Name = "TypeTokenColumn";
    TypeTokenColumn.Width = 125;
    // 
    // ChildrenTokenColumn
    // 
    ChildrenTokenColumn.DataPropertyName = "Children";
    ChildrenTokenColumn.HeaderText = "Children";
    ChildrenTokenColumn.MinimumWidth = 6;
    ChildrenTokenColumn.Name = "ChildrenTokenColumn";
    ChildrenTokenColumn.Resizable = DataGridViewTriState.True;
    ChildrenTokenColumn.SortMode = DataGridViewColumnSortMode.Automatic;
    ChildrenTokenColumn.Width = 125;
    // 
    // TokenBindingSource
    // 
    TokenBindingSource.DataSource = typeof(Parser.Tokens.IToken);
    // 
    // TokenRuleDataGrid
    // 
    TokenRuleDataGrid.AllowUserToOrderColumns = true;
    TokenRuleDataGrid.AutoGenerateColumns = false;
    TokenRuleDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenRuleDataGrid.Columns.AddRange(new DataGridViewColumn[] { StatusImageColumn, TypeTextColumn, RuleDataColumnText, AssignTypeColumnText });
    TokenRuleDataGrid.DataSource = TokenRuleBindingSource;
    TokenRuleDataGrid.EditMode = DataGridViewEditMode.EditOnKeystroke;
    TokenRuleDataGrid.GridColor = SystemColors.MenuText;
    TokenRuleDataGrid.Location = new Point(6, 33);
    TokenRuleDataGrid.Margin = new Padding(3, 2, 3, 2);
    TokenRuleDataGrid.MultiSelect = false;
    TokenRuleDataGrid.Name = "TokenRuleDataGrid";
    TokenRuleDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
    TokenRuleDataGrid.RowHeadersVisible = false;
    TokenRuleDataGrid.RowHeadersWidth = 51;
    dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
    dataGridViewCellStyle2.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
    dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
    TokenRuleDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle2;
    TokenRuleDataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
    TokenRuleDataGrid.Size = new Size(392, 245);
    TokenRuleDataGrid.TabIndex = 7;
    TokenRuleDataGrid.DataError += TokenRuleDataGrid_DataError;
    TokenRuleDataGrid.RowEnter += TokenRuleDataGrid_RowEnter;
    TokenRuleDataGrid.RowValidated += TokenRuleDataGrid_RowValidated;
    // 
    // StatusImageColumn
    // 
    StatusImageColumn.HeaderText = "";
    StatusImageColumn.Image = Properties.Resources.ToolStripIcon_Image;
    StatusImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
    StatusImageColumn.Name = "StatusImageColumn";
    StatusImageColumn.ReadOnly = true;
    StatusImageColumn.Resizable = DataGridViewTriState.False;
    StatusImageColumn.Width = 25;
    // 
    // TypeTextColumn
    // 
    TypeTextColumn.DataPropertyName = "Type";
    dataGridViewCellStyle1.NullValue = "None";
    TypeTextColumn.DefaultCellStyle = dataGridViewCellStyle1;
    TypeTextColumn.HeaderText = "Type";
    TypeTextColumn.MinimumWidth = 6;
    TypeTextColumn.Name = "TypeTextColumn";
    TypeTextColumn.Resizable = DataGridViewTriState.True;
    TypeTextColumn.Width = 125;
    // 
    // RuleDataColumnText
    // 
    RuleDataColumnText.DataPropertyName = "RuleStringData";
    RuleDataColumnText.HeaderText = "RuleStringData";
    RuleDataColumnText.MinimumWidth = 6;
    RuleDataColumnText.Name = "RuleDataColumnText";
    RuleDataColumnText.Width = 125;
    // 
    // AssignTypeColumnText
    // 
    AssignTypeColumnText.DataPropertyName = "TypeToAssign";
    AssignTypeColumnText.HeaderText = "TypeToAssign";
    AssignTypeColumnText.MinimumWidth = 6;
    AssignTypeColumnText.Name = "AssignTypeColumnText";
    AssignTypeColumnText.Resizable = DataGridViewTriState.True;
    AssignTypeColumnText.Width = 125;
    // 
    // AssemblerPage
    // 
    AssemblerPage.Location = new Point(4, 26);
    AssemblerPage.Name = "AssemblerPage";
    AssemblerPage.Padding = new Padding(3, 2, 3, 2);
    AssemblerPage.Size = new Size(839, 367);
    AssemblerPage.TabIndex = 1;
    AssemblerPage.Text = "Assembler";
    AssemblerPage.UseVisualStyleBackColor = true;
    // 
    // SaveRuleButton
    // 
    SaveRuleButton.Enabled = false;
    SaveRuleButton.FlatStyle = FlatStyle.System;
    SaveRuleButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    SaveRuleButton.Location = new Point(104, 336);
    SaveRuleButton.Margin = new Padding(3, 2, 3, 2);
    SaveRuleButton.Name = "SaveRuleButton";
    SaveRuleButton.Size = new Size(85, 45);
    SaveRuleButton.TabIndex = 8;
    SaveRuleButton.Text = "Save Ruleset to File";
    SaveRuleButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    SaveRuleButton.UseVisualStyleBackColor = true;
    SaveRuleButton.Click += SaveRuleDialog;
    // 
    // LoadRulesFileButton
    // 
    LoadRulesFileButton.Enabled = false;
    LoadRulesFileButton.FlatStyle = FlatStyle.System;
    LoadRulesFileButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    LoadRulesFileButton.Location = new Point(104, 224);
    LoadRulesFileButton.Margin = new Padding(3, 2, 3, 2);
    LoadRulesFileButton.Name = "LoadRulesFileButton";
    LoadRulesFileButton.Size = new Size(85, 45);
    LoadRulesFileButton.TabIndex = 8;
    LoadRulesFileButton.Text = "Load Rules From File";
    LoadRulesFileButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadRulesFileButton.UseVisualStyleBackColor = true;
    // 
    // ClearRulesButton
    // 
    ClearRulesButton.Enabled = false;
    ClearRulesButton.FlatStyle = FlatStyle.System;
    ClearRulesButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    ClearRulesButton.Location = new Point(104, 181);
    ClearRulesButton.Margin = new Padding(3, 2, 3, 2);
    ClearRulesButton.Name = "ClearRulesButton";
    ClearRulesButton.Size = new Size(85, 42);
    ClearRulesButton.TabIndex = 8;
    ClearRulesButton.Text = "Clear Rule Table";
    ClearRulesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ClearRulesButton.UseVisualStyleBackColor = true;
    ClearRulesButton.Click += ClearRules;
    // 
    // LoadRulesButton
    // 
    LoadRulesButton.Enabled = false;
    LoadRulesButton.FlatStyle = FlatStyle.System;
    LoadRulesButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    LoadRulesButton.Location = new Point(101, 76);
    LoadRulesButton.Margin = new Padding(3, 2, 3, 2);
    LoadRulesButton.Name = "LoadRulesButton";
    LoadRulesButton.Size = new Size(85, 42);
    LoadRulesButton.TabIndex = 8;
    LoadRulesButton.Text = "Load Rules from Specification";
    LoadRulesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadRulesButton.UseVisualStyleBackColor = true;
    LoadRulesButton.Click += LoadRules;
    // 
    // LoadSpecButton
    // 
    LoadSpecButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    LoadSpecButton.Location = new Point(6, 76);
    LoadSpecButton.Margin = new Padding(3, 2, 3, 2);
    LoadSpecButton.Name = "LoadSpecButton";
    LoadSpecButton.Size = new Size(85, 42);
    LoadSpecButton.TabIndex = 8;
    LoadSpecButton.Text = "Load Specifications";
    LoadSpecButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadSpecButton.UseVisualStyleBackColor = true;
    LoadSpecButton.Click += LoadSpecList;
    // 
    // ShowUnparsedButton
    // 
    ShowUnparsedButton.Enabled = false;
    ShowUnparsedButton.FlatStyle = FlatStyle.System;
    ShowUnparsedButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    ShowUnparsedButton.Location = new Point(8, 335);
    ShowUnparsedButton.Margin = new Padding(3, 2, 3, 2);
    ShowUnparsedButton.Name = "ShowUnparsedButton";
    ShowUnparsedButton.Size = new Size(85, 45);
    ShowUnparsedButton.TabIndex = 8;
    ShowUnparsedButton.Text = "Show Unparsed Text";
    ShowUnparsedButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ShowUnparsedButton.UseVisualStyleBackColor = true;
    ShowUnparsedButton.Click += ShowUnparsed;
    // 
    // SaveRuleFileDialog
    // 
    SaveRuleFileDialog.DefaultExt = "xml";
    SaveRuleFileDialog.FileName = "Rule.xml";
    SaveRuleFileDialog.Filter = "XML Files|*.xml";
    SaveRuleFileDialog.InitialDirectory = "%USERPROFILE%";
    SaveRuleFileDialog.Title = "Save Rule XML File";
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(7F, 17F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1062, 468);
    Controls.Add(LoadSpecButton);
    Controls.Add(ShowUnparsedButton);
    Controls.Add(LoadRulesButton);
    Controls.Add(ClearRulesButton);
    Controls.Add(SaveRuleButton);
    Controls.Add(ItemTabs);
    Controls.Add(LoadRulesFileButton);
    Controls.Add(SpecLabel);
    Controls.Add(SpecComboBox);
    Controls.Add(StatusStrip);
    Controls.Add(TheMenuStrip);
    MainMenuStrip = TheMenuStrip;
    Margin = new Padding(3, 2, 3, 2);
    Name = "ParserForm";
    Text = "Parser Form";
    Load += ParserForm_Load;
    StatusStrip.ResumeLayout(false);
    StatusStrip.PerformLayout();
    TheMenuStrip.ResumeLayout(false);
    TheMenuStrip.PerformLayout();
    ((ISupportInitialize) SpecBindingSource).EndInit();
    ((ISupportInitialize) TokenRuleBindingSource).EndInit();
    ItemTabs.ResumeLayout(false);
    TokenizerPage.ResumeLayout(false);
    TokenizerPage.PerformLayout();
    ((ISupportInitialize) TokenDataGrid).EndInit();
    ((ISupportInitialize) TokenBindingSource).EndInit();
    ((ISupportInitialize) TokenRuleDataGrid).EndInit();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private StatusStrip StatusStrip;
  private SaveFileDialog SaveRuleFileDialog;
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
  private ToolStripMenuItem OpenFileMenuItem;
  private ToolStripSeparator ToolStripSeparator1;
  private ToolStripMenuItem ExitMenuItem;
  private BindingSource SpecBindingSource;
  private TabControl ItemTabs;
  private TabPage TokenizerPage;
  private TabPage AssemblerPage;
  private Button LoadRulesButton;
  private DataGridView TokenDataGrid;
  private DataGridView TokenRuleDataGrid;
  private DataGridViewTextBoxColumn LengthTokenColumn;
  private DataGridViewTextBoxColumn IndexTokenColumn;
  private BindingSource TokenBindingSource;
  private Label TokenLabel;
  private Label TokenCountLabel;
  private Label TokenRuleCountLabel;
  private Label TokenRuleLabel;
  private DataGridViewTextBoxColumn ContentTokenColumn;
  private DataGridViewCheckBoxColumn ExemptTokenColumn;
  private DataGridViewTextBoxColumn TypeTokenColumn;
  private DataGridViewLinkColumn ChildrenTokenColumn;
  private Label ParsePathLabel;
  private TextBox ParsePathTextBox;
  private Label RuleTableLabel;
  private Label TokenTableLabel;
  private Button LoadSpecButton;
  private Button LoadRulesFileButton;
  private Button ClearRulesButton;
  private Button ClearTokensButton;
  private Button SaveRuleButton;
  private Button ShowUnparsedButton;
  private Button LoadParseFileButton;
  private ToolStripMenuItem RuleMenuItem;
  private ToolStripProgressBar ParseProgressBar;
  private ToolStripMenuItem SpecMenuItem;
  private ToolStripMenuItem loadSpecsToolStripMenuItem;
  private ToolStripMenuItem loadFromSpecToolStripMenuItem;
  private ToolStripMenuItem loadFromFilToolStripMenuItem;
  private ToolStripSeparator toolStripSeparator2;
  private ToolStripMenuItem saveToFileToolStripMenuItem;
  private ToolStripMenuItem clearRuleTableToolStripMenuItem;
  private ToolStripMenuItem tokensToolStripMenuItem;
  private ToolStripMenuItem clearTokenTableToolStripMenuItem;
  private ToolStripMenuItem parseTokensToolStripMenuItem;
  private DataGridViewImageColumn StatusImageColumn;
  private DataGridViewTextBoxColumn TypeTextColumn;
  private DataGridViewTextBoxColumn RuleDataColumnText;
  private DataGridViewTextBoxColumn AssignTypeColumnText;
}
