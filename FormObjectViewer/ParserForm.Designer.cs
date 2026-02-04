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
    SaveRuleFileDialog = new SaveFileDialog();
    TheMenuStrip = new MenuStrip();
    FileMenu = new ToolStripMenuItem();
    OpenFileMenuItem = new ToolStripMenuItem();
    ToolStripSeparator1 = new ToolStripSeparator();
    ExitMenuItem = new ToolStripMenuItem();
    SpecMenuItem = new ToolStripMenuItem();
    LoadSpecsMenuItem = new ToolStripMenuItem();
    TokensMenu = new ToolStripMenuItem();
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
    ItemTabs = new TabControl();
    TokenRulesPage = new TabPage();
    RuleTableLabel = new Label();
    TokenRuleCountLabel = new Label();
    TokenRuleLabel = new Label();
    TokenRuleDataGrid = new DataGridView();
    EditTokenRuleButtonColumn = new DataGridViewButtonColumn();
    TypeTextColumn = new DataGridViewTextBoxColumn();
    AssignTypeColumnText = new DataGridViewTextBoxColumn();
    RuleDataColumnText = new DataGridViewTextBoxColumn();
    ClearRulesButton = new Button();
    SaveRuleButton = new Button();
    LoadRulesFileButton = new Button();
    LoadRulesButton = new Button();
    TokenListPage = new TabPage();
    ClearTokensButton = new Button();
    TokenTableLabel = new Label();
    TokenLabel = new Label();
    TokenCountLabel = new Label();
    TokenDataGrid = new DataGridView();
    TypeTokenColumn = new DataGridViewTextBoxColumn();
    ExemptTokenColumn = new DataGridViewCheckBoxColumn();
    ContentTokenColumn = new DataGridViewTextBoxColumn();
    ChildrenTokenColumn = new DataGridViewLinkColumn();
    TokenBindingSource = new BindingSource(components);
    ShowUnparsedButton = new Button();
    AssemblerPage = new TabPage();
    LoadSpecButton = new Button();
    LoadParseFileButton = new Button();
    ParsePathLabel = new Label();
    ParsePathTextBox = new TextBox();
    ParseButton = new Button();
    StatusStrip.SuspendLayout();
    TheMenuStrip.SuspendLayout();
    ((ISupportInitialize) SpecBindingSource).BeginInit();
    ((ISupportInitialize) TokenRuleBindingSource).BeginInit();
    ItemTabs.SuspendLayout();
    TokenRulesPage.SuspendLayout();
    ((ISupportInitialize) TokenRuleDataGrid).BeginInit();
    TokenListPage.SuspendLayout();
    ((ISupportInitialize) TokenDataGrid).BeginInit();
    ((ISupportInitialize) TokenBindingSource).BeginInit();
    SuspendLayout();
    // 
    // StatusStrip
    // 
    StatusStrip.ImageScalingSize = new Size(20, 20);
    StatusStrip.Items.AddRange(new ToolStripItem[] { ParseProgressBar });
    StatusStrip.Location = new Point(0, 444);
    StatusStrip.Name = "StatusStrip";
    StatusStrip.Padding = new Padding(1, 0, 12, 0);
    StatusStrip.Size = new Size(1181, 24);
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
    // SaveRuleFileDialog
    // 
    SaveRuleFileDialog.DefaultExt = "xml";
    SaveRuleFileDialog.FileName = "Rule.xml";
    SaveRuleFileDialog.Filter = "XML Files|*.xml";
    SaveRuleFileDialog.InitialDirectory = "%USERPROFILE%";
    SaveRuleFileDialog.Title = "Save Rule XML File";
    // 
    // TheMenuStrip
    // 
    TheMenuStrip.ImageScalingSize = new Size(20, 20);
    TheMenuStrip.Items.AddRange(new ToolStripItem[] { FileMenu, SpecMenuItem, TokensMenu, RuleMenuItem });
    TheMenuStrip.Location = new Point(0, 0);
    TheMenuStrip.Name = "TheMenuStrip";
    TheMenuStrip.Padding = new Padding(5, 2, 0, 2);
    TheMenuStrip.Size = new Size(1181, 25);
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
    SpecMenuItem.DropDownItems.AddRange(new ToolStripItem[] { LoadSpecsMenuItem });
    SpecMenuItem.Name = "SpecMenuItem";
    SpecMenuItem.Size = new Size(48, 21);
    SpecMenuItem.Text = "Spec";
    // 
    // LoadSpecsMenuItem
    // 
    LoadSpecsMenuItem.Name = "LoadSpecsMenuItem";
    LoadSpecsMenuItem.Size = new Size(180, 22);
    LoadSpecsMenuItem.Text = "Load Specs";
    LoadSpecsMenuItem.Click += LoadSpecList;
    // 
    // TokensMenu
    // 
    TokensMenu.DropDownItems.AddRange(new ToolStripItem[] { clearTokenTableToolStripMenuItem, parseTokensToolStripMenuItem });
    TokensMenu.Name = "TokensMenu";
    TokensMenu.Size = new Size(60, 21);
    TokensMenu.Text = "Tokens";
    // 
    // clearTokenTableToolStripMenuItem
    // 
    clearTokenTableToolStripMenuItem.Name = "clearTokenTableToolStripMenuItem";
    clearTokenTableToolStripMenuItem.Size = new Size(180, 22);
    clearTokenTableToolStripMenuItem.Text = "Clear Token Table";
    clearTokenTableToolStripMenuItem.Click += ClearTokens;
    // 
    // parseTokensToolStripMenuItem
    // 
    parseTokensToolStripMenuItem.Name = "parseTokensToolStripMenuItem";
    parseTokensToolStripMenuItem.Size = new Size(180, 22);
    parseTokensToolStripMenuItem.Text = "Parse Tokens";
    parseTokensToolStripMenuItem.Click += ExecuteParse;
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
    loadFromSpecToolStripMenuItem.Click += LoadRules;
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
    clearRuleTableToolStripMenuItem.Click += ClearRules;
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
    // ItemTabs
    // 
    ItemTabs.Controls.Add(TokenRulesPage);
    ItemTabs.Controls.Add(TokenListPage);
    ItemTabs.Controls.Add(AssemblerPage);
    ItemTabs.Location = new Point(196, 27);
    ItemTabs.Margin = new Padding(3, 2, 3, 2);
    ItemTabs.Name = "ItemTabs";
    ItemTabs.SelectedIndex = 0;
    ItemTabs.Size = new Size(964, 325);
    ItemTabs.TabIndex = 7;
    // 
    // TokenRulesPage
    // 
    TokenRulesPage.Controls.Add(RuleTableLabel);
    TokenRulesPage.Controls.Add(TokenRuleCountLabel);
    TokenRulesPage.Controls.Add(TokenRuleLabel);
    TokenRulesPage.Controls.Add(TokenRuleDataGrid);
    TokenRulesPage.Controls.Add(ClearRulesButton);
    TokenRulesPage.Controls.Add(SaveRuleButton);
    TokenRulesPage.Controls.Add(LoadRulesFileButton);
    TokenRulesPage.Controls.Add(LoadRulesButton);
    TokenRulesPage.Location = new Point(4, 26);
    TokenRulesPage.Margin = new Padding(3, 2, 3, 2);
    TokenRulesPage.Name = "TokenRulesPage";
    TokenRulesPage.Padding = new Padding(3, 2, 3, 2);
    TokenRulesPage.Size = new Size(956, 295);
    TokenRulesPage.TabIndex = 0;
    TokenRulesPage.Text = "Token Rules";
    // 
    // RuleTableLabel
    // 
    RuleTableLabel.Font = new Font("Arial Rounded MT Bold", 15.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    RuleTableLabel.Location = new Point(0, 0);
    RuleTableLabel.Margin = new Padding(2, 0, 2, 0);
    RuleTableLabel.Name = "RuleTableLabel";
    RuleTableLabel.Size = new Size(122, 27);
    RuleTableLabel.TabIndex = 12;
    RuleTableLabel.Text = "Rule Table";
    // 
    // TokenRuleCountLabel
    // 
    TokenRuleCountLabel.Location = new Point(784, 8);
    TokenRuleCountLabel.Name = "TokenRuleCountLabel";
    TokenRuleCountLabel.Size = new Size(35, 16);
    TokenRuleCountLabel.TabIndex = 9;
    TokenRuleCountLabel.Text = "0";
    // 
    // TokenRuleLabel
    // 
    TokenRuleLabel.AutoSize = true;
    TokenRuleLabel.Location = new Point(704, 8);
    TokenRuleLabel.Name = "TokenRuleLabel";
    TokenRuleLabel.Size = new Size(77, 17);
    TokenRuleLabel.TabIndex = 9;
    TokenRuleLabel.Text = "Token Rules";
    // 
    // TokenRuleDataGrid
    // 
    TokenRuleDataGrid.AllowUserToOrderColumns = true;
    TokenRuleDataGrid.AutoGenerateColumns = false;
    TokenRuleDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenRuleDataGrid.Columns.AddRange(new DataGridViewColumn[] { EditTokenRuleButtonColumn, TypeTextColumn, AssignTypeColumnText, RuleDataColumnText });
    TokenRuleDataGrid.DataSource = TokenRuleBindingSource;
    TokenRuleDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
    TokenRuleDataGrid.GridColor = SystemColors.MenuText;
    TokenRuleDataGrid.Location = new Point(8, 32);
    TokenRuleDataGrid.Margin = new Padding(3, 2, 3, 2);
    TokenRuleDataGrid.MultiSelect = false;
    TokenRuleDataGrid.Name = "TokenRuleDataGrid";
    TokenRuleDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
    TokenRuleDataGrid.RowHeadersVisible = false;
    TokenRuleDataGrid.RowHeadersWidth = 51;
    dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
    dataGridViewCellStyle2.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
    TokenRuleDataGrid.RowsDefaultCellStyle = dataGridViewCellStyle2;
    TokenRuleDataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
    TokenRuleDataGrid.Size = new Size(816, 248);
    TokenRuleDataGrid.TabIndex = 7;
    TokenRuleDataGrid.CellClick += TokenRuleDataGrid_RowEnter;
    TokenRuleDataGrid.RowValidated += TokenRuleDataGrid_RowValidated;
    // 
    // EditTokenRuleButtonColumn
    // 
    EditTokenRuleButtonColumn.HeaderText = "Edit";
    EditTokenRuleButtonColumn.Name = "EditTokenRuleButtonColumn";
    EditTokenRuleButtonColumn.Text = "Edit";
    EditTokenRuleButtonColumn.Width = 50;
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
    // AssignTypeColumnText
    // 
    AssignTypeColumnText.DataPropertyName = "TypeToAssign";
    AssignTypeColumnText.HeaderText = "TypeToAssign";
    AssignTypeColumnText.MinimumWidth = 6;
    AssignTypeColumnText.Name = "AssignTypeColumnText";
    AssignTypeColumnText.Resizable = DataGridViewTriState.True;
    AssignTypeColumnText.Width = 125;
    // 
    // RuleDataColumnText
    // 
    RuleDataColumnText.DataPropertyName = "RuleStringData";
    RuleDataColumnText.HeaderText = "RuleStringData";
    RuleDataColumnText.MinimumWidth = 6;
    RuleDataColumnText.Name = "RuleDataColumnText";
    RuleDataColumnText.Width = 500;
    // 
    // ClearRulesButton
    // 
    ClearRulesButton.Enabled = false;
    ClearRulesButton.FlatStyle = FlatStyle.System;
    ClearRulesButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    ClearRulesButton.Location = new Point(840, 248);
    ClearRulesButton.Margin = new Padding(3, 2, 3, 2);
    ClearRulesButton.Name = "ClearRulesButton";
    ClearRulesButton.Size = new Size(96, 32);
    ClearRulesButton.TabIndex = 8;
    ClearRulesButton.Text = "Clear Rule Table";
    ClearRulesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ClearRulesButton.UseVisualStyleBackColor = true;
    ClearRulesButton.Click += ClearRules;
    // 
    // SaveRuleButton
    // 
    SaveRuleButton.Enabled = false;
    SaveRuleButton.FlatStyle = FlatStyle.System;
    SaveRuleButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    SaveRuleButton.Location = new Point(840, 168);
    SaveRuleButton.Margin = new Padding(3, 2, 3, 2);
    SaveRuleButton.Name = "SaveRuleButton";
    SaveRuleButton.Size = new Size(96, 45);
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
    LoadRulesFileButton.Location = new Point(840, 32);
    LoadRulesFileButton.Margin = new Padding(3, 2, 3, 2);
    LoadRulesFileButton.Name = "LoadRulesFileButton";
    LoadRulesFileButton.Size = new Size(96, 40);
    LoadRulesFileButton.TabIndex = 8;
    LoadRulesFileButton.Text = "Load Rules From File";
    LoadRulesFileButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadRulesFileButton.UseVisualStyleBackColor = true;
    // 
    // LoadRulesButton
    // 
    LoadRulesButton.Enabled = false;
    LoadRulesButton.FlatStyle = FlatStyle.System;
    LoadRulesButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    LoadRulesButton.Location = new Point(840, 80);
    LoadRulesButton.Margin = new Padding(3, 2, 3, 2);
    LoadRulesButton.Name = "LoadRulesButton";
    LoadRulesButton.Size = new Size(96, 42);
    LoadRulesButton.TabIndex = 8;
    LoadRulesButton.Text = "Load Rules from Specification";
    LoadRulesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadRulesButton.UseVisualStyleBackColor = true;
    LoadRulesButton.Click += LoadRules;
    // 
    // TokenListPage
    // 
    TokenListPage.BackColor = SystemColors.Control;
    TokenListPage.Controls.Add(ClearTokensButton);
    TokenListPage.Controls.Add(TokenTableLabel);
    TokenListPage.Controls.Add(TokenLabel);
    TokenListPage.Controls.Add(TokenCountLabel);
    TokenListPage.Controls.Add(TokenDataGrid);
    TokenListPage.Controls.Add(ShowUnparsedButton);
    TokenListPage.Location = new Point(4, 26);
    TokenListPage.Name = "TokenListPage";
    TokenListPage.Padding = new Padding(3);
    TokenListPage.Size = new Size(956, 295);
    TokenListPage.TabIndex = 2;
    TokenListPage.Text = "Token List";
    // 
    // ClearTokensButton
    // 
    ClearTokensButton.Enabled = false;
    ClearTokensButton.FlatStyle = FlatStyle.System;
    ClearTokensButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    ClearTokensButton.Location = new Point(840, 248);
    ClearTokensButton.Margin = new Padding(3, 2, 3, 2);
    ClearTokensButton.Name = "ClearTokensButton";
    ClearTokensButton.Size = new Size(96, 32);
    ClearTokensButton.TabIndex = 22;
    ClearTokensButton.Text = "Clear Token Table";
    ClearTokensButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ClearTokensButton.UseVisualStyleBackColor = true;
    // 
    // TokenTableLabel
    // 
    TokenTableLabel.Font = new Font("Arial Rounded MT Bold", 15.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    TokenTableLabel.Location = new Point(0, 0);
    TokenTableLabel.Margin = new Padding(2, 0, 2, 0);
    TokenTableLabel.Name = "TokenTableLabel";
    TokenTableLabel.Size = new Size(135, 27);
    TokenTableLabel.TabIndex = 16;
    TokenTableLabel.Text = "Token Table";
    // 
    // TokenLabel
    // 
    TokenLabel.AutoSize = true;
    TokenLabel.Location = new Point(688, 8);
    TokenLabel.Name = "TokenLabel";
    TokenLabel.Size = new Size(92, 17);
    TokenLabel.TabIndex = 14;
    TokenLabel.Text = "Tokens Parsed";
    // 
    // TokenCountLabel
    // 
    TokenCountLabel.Location = new Point(784, 8);
    TokenCountLabel.Name = "TokenCountLabel";
    TokenCountLabel.Size = new Size(38, 20);
    TokenCountLabel.TabIndex = 15;
    TokenCountLabel.Text = "0";
    // 
    // TokenDataGrid
    // 
    TokenDataGrid.AllowUserToAddRows = false;
    TokenDataGrid.AllowUserToDeleteRows = false;
    TokenDataGrid.AllowUserToResizeRows = false;
    TokenDataGrid.AutoGenerateColumns = false;
    TokenDataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
    TokenDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenDataGrid.Columns.AddRange(new DataGridViewColumn[] { TypeTokenColumn, ExemptTokenColumn, ContentTokenColumn, ChildrenTokenColumn });
    TokenDataGrid.DataSource = TokenBindingSource;
    TokenDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
    TokenDataGrid.GridColor = SystemColors.MenuText;
    TokenDataGrid.Location = new Point(8, 32);
    TokenDataGrid.Margin = new Padding(3, 2, 3, 2);
    TokenDataGrid.Name = "TokenDataGrid";
    TokenDataGrid.ReadOnly = true;
    TokenDataGrid.RowHeadersVisible = false;
    TokenDataGrid.RowHeadersWidth = 51;
    TokenDataGrid.ScrollBars = ScrollBars.Vertical;
    TokenDataGrid.Size = new Size(816, 248);
    TokenDataGrid.TabIndex = 13;
    // 
    // TypeTokenColumn
    // 
    TypeTokenColumn.DataPropertyName = "Type";
    TypeTokenColumn.HeaderText = "Type";
    TypeTokenColumn.MinimumWidth = 6;
    TypeTokenColumn.Name = "TypeTokenColumn";
    TypeTokenColumn.ReadOnly = true;
    TypeTokenColumn.Width = 125;
    // 
    // ExemptTokenColumn
    // 
    ExemptTokenColumn.DataPropertyName = "Exempt";
    ExemptTokenColumn.HeaderText = "Exempt";
    ExemptTokenColumn.MinimumWidth = 6;
    ExemptTokenColumn.Name = "ExemptTokenColumn";
    ExemptTokenColumn.ReadOnly = true;
    ExemptTokenColumn.Width = 75;
    // 
    // ContentTokenColumn
    // 
    ContentTokenColumn.DataPropertyName = "Content";
    ContentTokenColumn.HeaderText = "Content";
    ContentTokenColumn.MinimumWidth = 6;
    ContentTokenColumn.Name = "ContentTokenColumn";
    ContentTokenColumn.ReadOnly = true;
    ContentTokenColumn.Width = 400;
    // 
    // ChildrenTokenColumn
    // 
    ChildrenTokenColumn.DataPropertyName = "Children";
    ChildrenTokenColumn.HeaderText = "Children";
    ChildrenTokenColumn.MinimumWidth = 6;
    ChildrenTokenColumn.Name = "ChildrenTokenColumn";
    ChildrenTokenColumn.ReadOnly = true;
    ChildrenTokenColumn.Resizable = DataGridViewTriState.True;
    ChildrenTokenColumn.SortMode = DataGridViewColumnSortMode.Automatic;
    ChildrenTokenColumn.Width = 125;
    // 
    // TokenBindingSource
    // 
    TokenBindingSource.DataSource = typeof(Parser.Tokens.IToken);
    // 
    // ShowUnparsedButton
    // 
    ShowUnparsedButton.Enabled = false;
    ShowUnparsedButton.FlatStyle = FlatStyle.System;
    ShowUnparsedButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    ShowUnparsedButton.Location = new Point(840, 200);
    ShowUnparsedButton.Margin = new Padding(3, 2, 3, 2);
    ShowUnparsedButton.Name = "ShowUnparsedButton";
    ShowUnparsedButton.Size = new Size(96, 37);
    ShowUnparsedButton.TabIndex = 8;
    ShowUnparsedButton.Text = "Show Unparsed Text";
    ShowUnparsedButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ShowUnparsedButton.UseVisualStyleBackColor = true;
    ShowUnparsedButton.Click += ShowUnparsed;
    // 
    // AssemblerPage
    // 
    AssemblerPage.Location = new Point(4, 26);
    AssemblerPage.Name = "AssemblerPage";
    AssemblerPage.Padding = new Padding(3, 2, 3, 2);
    AssemblerPage.Size = new Size(956, 295);
    AssemblerPage.TabIndex = 1;
    AssemblerPage.Text = "Assembler";
    AssemblerPage.UseVisualStyleBackColor = true;
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
    // LoadParseFileButton
    // 
    LoadParseFileButton.Enabled = false;
    LoadParseFileButton.FlatStyle = FlatStyle.System;
    LoadParseFileButton.Font = new Font("Bahnschrift SemiCondensed", 9F);
    LoadParseFileButton.Location = new Point(928, 357);
    LoadParseFileButton.Margin = new Padding(3, 2, 3, 2);
    LoadParseFileButton.Name = "LoadParseFileButton";
    LoadParseFileButton.Size = new Size(88, 29);
    LoadParseFileButton.TabIndex = 13;
    LoadParseFileButton.Text = "Load Parse File";
    LoadParseFileButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadParseFileButton.UseVisualStyleBackColor = true;
    LoadParseFileButton.Click += OpenParseFile;
    // 
    // ParsePathLabel
    // 
    ParsePathLabel.AutoSize = true;
    ParsePathLabel.Location = new Point(208, 373);
    ParsePathLabel.Margin = new Padding(2, 0, 2, 0);
    ParsePathLabel.Name = "ParsePathLabel";
    ParsePathLabel.Size = new Size(123, 17);
    ParsePathLabel.TabIndex = 15;
    ParsePathLabel.Text = "Path to file to parse";
    // 
    // ParsePathTextBox
    // 
    ParsePathTextBox.Location = new Point(208, 397);
    ParsePathTextBox.Margin = new Padding(2);
    ParsePathTextBox.Name = "ParsePathTextBox";
    ParsePathTextBox.Size = new Size(712, 25);
    ParsePathTextBox.TabIndex = 14;
    // 
    // ParseButton
    // 
    ParseButton.Location = new Point(928, 397);
    ParseButton.Margin = new Padding(3, 2, 3, 2);
    ParseButton.Name = "ParseButton";
    ParseButton.Size = new Size(88, 27);
    ParseButton.TabIndex = 12;
    ParseButton.Text = "Parse";
    ParseButton.UseVisualStyleBackColor = true;
    ParseButton.Click += ExecuteParse;
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(7F, 17F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1181, 468);
    Controls.Add(LoadParseFileButton);
    Controls.Add(ParsePathLabel);
    Controls.Add(ParsePathTextBox);
    Controls.Add(ParseButton);
    Controls.Add(LoadSpecButton);
    Controls.Add(ItemTabs);
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
    TokenRulesPage.ResumeLayout(false);
    TokenRulesPage.PerformLayout();
    ((ISupportInitialize) TokenRuleDataGrid).EndInit();
    TokenListPage.ResumeLayout(false);
    TokenListPage.PerformLayout();
    ((ISupportInitialize) TokenDataGrid).EndInit();
    ((ISupportInitialize) TokenBindingSource).EndInit();
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
  private ToolStripMenuItem FileMenu;
  private ToolStripMenuItem OpenFileMenuItem;
  private ToolStripSeparator ToolStripSeparator1;
  private ToolStripMenuItem ExitMenuItem;
  private BindingSource SpecBindingSource;
  private TabControl ItemTabs;
  private TabPage TokenRulesPage;
  private TabPage AssemblerPage;
  private Button LoadRulesButton;
  private DataGridView TokenRuleDataGrid;
  private BindingSource TokenBindingSource;
  private Label TokenRuleCountLabel;
  private Label TokenRuleLabel;
  private Label RuleTableLabel;
  private Button LoadSpecButton;
  private Button LoadRulesFileButton;
  private Button ClearRulesButton;
  private Button SaveRuleButton;
  private Button ShowUnparsedButton;
  private ToolStripMenuItem RuleMenuItem;
  private ToolStripProgressBar ParseProgressBar;
  private ToolStripMenuItem SpecMenuItem;
  private ToolStripMenuItem LoadSpecsMenuItem;
  private ToolStripMenuItem loadFromSpecToolStripMenuItem;
  private ToolStripMenuItem loadFromFilToolStripMenuItem;
  private ToolStripSeparator toolStripSeparator2;
  private ToolStripMenuItem saveToFileToolStripMenuItem;
  private ToolStripMenuItem clearRuleTableToolStripMenuItem;
  private ToolStripMenuItem TokensMenu;
  private ToolStripMenuItem clearTokenTableToolStripMenuItem;
  private ToolStripMenuItem parseTokensToolStripMenuItem;
  private TabPage TokenListPage;
  private Button ClearTokensButton;
  private Label TokenTableLabel;
  private Label TokenLabel;
  private Label TokenCountLabel;
  private DataGridView TokenDataGrid;
  private DataGridViewTextBoxColumn TypeTokenColumn;
  private DataGridViewCheckBoxColumn ExemptTokenColumn;
  private DataGridViewTextBoxColumn ContentTokenColumn;
  private DataGridViewLinkColumn ChildrenTokenColumn;
  private Button LoadParseFileButton;
  private Label ParsePathLabel;
  private TextBox ParsePathTextBox;
  private Button ParseButton;
  private DataGridViewButtonColumn EditTokenRuleButtonColumn;
  private DataGridViewTextBoxColumn TypeTextColumn;
  private DataGridViewTextBoxColumn AssignTypeColumnText;
  private DataGridViewTextBoxColumn RuleDataColumnText;
}
