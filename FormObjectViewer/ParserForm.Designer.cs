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
    GenerateRulesMenuItem = new ToolStripMenuItem();
    ToolStripSeparator1 = new ToolStripSeparator();
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
    TokenizerPage = new TabPage();
    TokenTableLabel = new Label();
    RuleTableLabel = new Label();
    ParsePathLabel = new Label();
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
    TypeGridColumnCombo = new DataGridViewComboBoxColumn();
    RuleDataColumnText = new DataGridViewTextBoxColumn();
    AssignTypeColumnText = new DataGridViewTextBoxColumn();
    AssemblerPage = new TabPage();
    LoadRulesButton = new Button();
    LoadSpecButton = new Button();
    LoadRulesFileButton = new Button();
    ClearRulesButton = new Button();
    ClearTokensButton = new Button();
    SaveRuleButton = new Button();
    ShowUnparsedButton = new Button();
    LoadParseFileButton = new Button();
    TheMenuStrip.SuspendLayout();
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
    TheMenuStrip.Size = new Size(1517, 27);
    TheMenuStrip.TabIndex = 2;
    TheMenuStrip.Text = "TheMenuStrip";
    // 
    // FileMenu
    // 
    FileMenu.DropDownItems.AddRange(new ToolStripItem[] { LoadSpecMenuItem, OpenFileMenuItem, GenerateRulesMenuItem, ToolStripSeparator1, ExitMenuItem });
    FileMenu.Name = "FileMenu";
    FileMenu.Size = new Size(39, 21);
    FileMenu.Text = "File";
    // 
    // LoadSpecMenuItem
    // 
    LoadSpecMenuItem.Name = "LoadSpecMenuItem";
    LoadSpecMenuItem.Size = new Size(191, 22);
    LoadSpecMenuItem.Text = "Load Specs";
    LoadSpecMenuItem.Click += LoadSpecMenuItem_Click;
    // 
    // OpenFileMenuItem
    // 
    OpenFileMenuItem.Name = "OpenFileMenuItem";
    OpenFileMenuItem.Size = new Size(191, 22);
    OpenFileMenuItem.Text = "Open File to Parse";
    OpenFileMenuItem.Click += OpenFileMenuItem_Click;
    // 
    // GenerateRulesMenuItem
    // 
    GenerateRulesMenuItem.Enabled = false;
    GenerateRulesMenuItem.Name = "GenerateRulesMenuItem";
    GenerateRulesMenuItem.Size = new Size(191, 22);
    GenerateRulesMenuItem.Text = "Load Rules to Table";
    // 
    // ToolStripSeparator1
    // 
    ToolStripSeparator1.Name = "ToolStripSeparator1";
    ToolStripSeparator1.Size = new Size(188, 6);
    // 
    // ExitMenuItem
    // 
    ExitMenuItem.Name = "ExitMenuItem";
    ExitMenuItem.Size = new Size(191, 22);
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
    SpecComboBox.Size = new Size(254, 33);
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
    ParseButton.Location = new Point(1048, 432);
    ParseButton.Margin = new Padding(4, 3, 4, 3);
    ParseButton.Name = "ParseButton";
    ParseButton.Size = new Size(130, 40);
    ParseButton.TabIndex = 6;
    ParseButton.Text = "Parse";
    ParseButton.UseVisualStyleBackColor = true;
    ParseButton.Click += ParseButton_Click;
    // 
    // ItemTabs
    // 
    ItemTabs.Controls.Add(TokenizerPage);
    ItemTabs.Controls.Add(AssemblerPage);
    ItemTabs.Location = new Point(280, 40);
    ItemTabs.Margin = new Padding(4, 3, 4, 3);
    ItemTabs.Name = "ItemTabs";
    ItemTabs.SelectedIndex = 0;
    ItemTabs.Size = new Size(1210, 528);
    ItemTabs.TabIndex = 7;
    // 
    // TokenizerPage
    // 
    TokenizerPage.Controls.Add(TokenTableLabel);
    TokenizerPage.Controls.Add(RuleTableLabel);
    TokenizerPage.Controls.Add(ParsePathLabel);
    TokenizerPage.Controls.Add(ParsePathTextBox);
    TokenizerPage.Controls.Add(TokenLabel);
    TokenizerPage.Controls.Add(TokenCountLabel);
    TokenizerPage.Controls.Add(TokenRuleCountLabel);
    TokenizerPage.Controls.Add(TokenRuleLabel);
    TokenizerPage.Controls.Add(TokenDataGrid);
    TokenizerPage.Controls.Add(TokenRuleDataGrid);
    TokenizerPage.Controls.Add(ParseButton);
    TokenizerPage.Location = new Point(4, 34);
    TokenizerPage.Margin = new Padding(4, 3, 4, 3);
    TokenizerPage.Name = "TokenizerPage";
    TokenizerPage.Padding = new Padding(4, 3, 4, 3);
    TokenizerPage.Size = new Size(1202, 490);
    TokenizerPage.TabIndex = 0;
    TokenizerPage.Text = "Tokenizer";
    TokenizerPage.UseVisualStyleBackColor = true;
    // 
    // TokenTableLabel
    // 
    TokenTableLabel.Font = new Font("Arial Rounded MT Bold", 15.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    TokenTableLabel.Location = new Point(584, 8);
    TokenTableLabel.Name = "TokenTableLabel";
    TokenTableLabel.Size = new Size(144, 30);
    TokenTableLabel.TabIndex = 12;
    TokenTableLabel.Text = "Token Table";
    // 
    // RuleTableLabel
    // 
    RuleTableLabel.Font = new Font("Arial Rounded MT Bold", 15.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    RuleTableLabel.Location = new Point(8, 8);
    RuleTableLabel.Name = "RuleTableLabel";
    RuleTableLabel.Size = new Size(128, 30);
    RuleTableLabel.TabIndex = 12;
    RuleTableLabel.Text = "Rule Table";
    // 
    // ParsePathLabel
    // 
    ParsePathLabel.AutoSize = true;
    ParsePathLabel.Location = new Point(16, 416);
    ParsePathLabel.Name = "ParsePathLabel";
    ParsePathLabel.Size = new Size(123, 17);
    ParsePathLabel.TabIndex = 11;
    ParsePathLabel.Text = "Path to file to parse";
    // 
    // ParsePathTextBox
    // 
    ParsePathTextBox.Location = new Point(16, 440);
    ParsePathTextBox.Name = "ParsePathTextBox";
    ParsePathTextBox.Size = new Size(1016, 25);
    ParsePathTextBox.TabIndex = 10;
    // 
    // TokenLabel
    // 
    TokenLabel.AutoSize = true;
    TokenLabel.Location = new Point(928, 16);
    TokenLabel.Margin = new Padding(4, 0, 4, 0);
    TokenLabel.Name = "TokenLabel";
    TokenLabel.Size = new Size(92, 17);
    TokenLabel.TabIndex = 9;
    TokenLabel.Text = "Tokens Parsed";
    // 
    // TokenCountLabel
    // 
    TokenCountLabel.Location = new Point(1024, 16);
    TokenCountLabel.Margin = new Padding(4, 0, 4, 0);
    TokenCountLabel.Name = "TokenCountLabel";
    TokenCountLabel.Size = new Size(77, 17);
    TokenCountLabel.TabIndex = 9;
    TokenCountLabel.Text = "0";
    // 
    // TokenRuleCountLabel
    // 
    TokenRuleCountLabel.Location = new Point(504, 16);
    TokenRuleCountLabel.Margin = new Padding(4, 0, 4, 0);
    TokenRuleCountLabel.Name = "TokenRuleCountLabel";
    TokenRuleCountLabel.Size = new Size(50, 17);
    TokenRuleCountLabel.TabIndex = 9;
    TokenRuleCountLabel.Text = "0";
    // 
    // TokenRuleLabel
    // 
    TokenRuleLabel.AutoSize = true;
    TokenRuleLabel.Location = new Point(424, 16);
    TokenRuleLabel.Margin = new Padding(4, 0, 4, 0);
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
    TokenDataGrid.Location = new Point(580, 48);
    TokenDataGrid.Margin = new Padding(4, 3, 4, 3);
    TokenDataGrid.Name = "TokenDataGrid";
    TokenDataGrid.RowHeadersWidth = 51;
    TokenDataGrid.Size = new Size(610, 360);
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
    TokenRuleDataGrid.AutoGenerateColumns = false;
    TokenRuleDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    TokenRuleDataGrid.Columns.AddRange(new DataGridViewColumn[] { TypeGridColumnCombo, RuleDataColumnText, AssignTypeColumnText });
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
    LoadRulesButton.Enabled = false;
    LoadRulesButton.FlatStyle = FlatStyle.System;
    LoadRulesButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    LoadRulesButton.Location = new Point(144, 112);
    LoadRulesButton.Margin = new Padding(4, 3, 4, 3);
    LoadRulesButton.Name = "LoadRulesButton";
    LoadRulesButton.Size = new Size(122, 48);
    LoadRulesButton.TabIndex = 8;
    LoadRulesButton.Text = "Load Rules From Specification";
    LoadRulesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadRulesButton.UseVisualStyleBackColor = true;
    LoadRulesButton.Click += LoadRulesButton_Click;
    // 
    // LoadSpecButton
    // 
    LoadSpecButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    LoadSpecButton.Location = new Point(8, 112);
    LoadSpecButton.Margin = new Padding(4, 3, 4, 3);
    LoadSpecButton.Name = "LoadSpecButton";
    LoadSpecButton.Size = new Size(122, 48);
    LoadSpecButton.TabIndex = 8;
    LoadSpecButton.Text = "Load Specifications";
    LoadSpecButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadSpecButton.UseVisualStyleBackColor = true;
    // 
    // LoadRulesFileButton
    // 
    LoadRulesFileButton.Enabled = false;
    LoadRulesFileButton.FlatStyle = FlatStyle.System;
    LoadRulesFileButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    LoadRulesFileButton.Location = new Point(144, 168);
    LoadRulesFileButton.Margin = new Padding(4, 3, 4, 3);
    LoadRulesFileButton.Name = "LoadRulesFileButton";
    LoadRulesFileButton.Size = new Size(122, 48);
    LoadRulesFileButton.TabIndex = 8;
    LoadRulesFileButton.Text = "Load Rules From File";
    LoadRulesFileButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadRulesFileButton.UseVisualStyleBackColor = true;
    // 
    // ClearRulesButton
    // 
    ClearRulesButton.Enabled = false;
    ClearRulesButton.FlatStyle = FlatStyle.System;
    ClearRulesButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    ClearRulesButton.Location = new Point(144, 224);
    ClearRulesButton.Margin = new Padding(4, 3, 4, 3);
    ClearRulesButton.Name = "ClearRulesButton";
    ClearRulesButton.Size = new Size(122, 48);
    ClearRulesButton.TabIndex = 8;
    ClearRulesButton.Text = "Clear Rule Table";
    ClearRulesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ClearRulesButton.UseVisualStyleBackColor = true;
    ClearRulesButton.Click += ClearRulesButton_Click;
    // 
    // ClearTokensButton
    // 
    ClearTokensButton.Enabled = false;
    ClearTokensButton.FlatStyle = FlatStyle.System;
    ClearTokensButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    ClearTokensButton.Location = new Point(144, 280);
    ClearTokensButton.Margin = new Padding(4, 3, 4, 3);
    ClearTokensButton.Name = "ClearTokensButton";
    ClearTokensButton.Size = new Size(122, 48);
    ClearTokensButton.TabIndex = 8;
    ClearTokensButton.Text = "Clear Token Table";
    ClearTokensButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ClearTokensButton.UseVisualStyleBackColor = true;
    ClearTokensButton.Click += ClearTokensButton_Click;
    // 
    // SaveRuleButton
    // 
    SaveRuleButton.Enabled = false;
    SaveRuleButton.FlatStyle = FlatStyle.System;
    SaveRuleButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    SaveRuleButton.Location = new Point(8, 168);
    SaveRuleButton.Margin = new Padding(4, 3, 4, 3);
    SaveRuleButton.Name = "SaveRuleButton";
    SaveRuleButton.Size = new Size(122, 48);
    SaveRuleButton.TabIndex = 8;
    SaveRuleButton.Text = "Save Ruleset to File";
    SaveRuleButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    SaveRuleButton.UseVisualStyleBackColor = true;
    SaveRuleButton.Click += SaveRuleButton_Click;
    // 
    // ShowUnparsedButton
    // 
    ShowUnparsedButton.Enabled = false;
    ShowUnparsedButton.FlatStyle = FlatStyle.System;
    ShowUnparsedButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    ShowUnparsedButton.Location = new Point(8, 224);
    ShowUnparsedButton.Margin = new Padding(4, 3, 4, 3);
    ShowUnparsedButton.Name = "ShowUnparsedButton";
    ShowUnparsedButton.Size = new Size(122, 48);
    ShowUnparsedButton.TabIndex = 8;
    ShowUnparsedButton.Text = "Show Unparsed Text";
    ShowUnparsedButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    ShowUnparsedButton.UseVisualStyleBackColor = true;
    ShowUnparsedButton.Click += ShowUnparsedButton_Click;
    // 
    // LoadParseFileButton
    // 
    LoadParseFileButton.Enabled = false;
    LoadParseFileButton.FlatStyle = FlatStyle.System;
    LoadParseFileButton.Font = new Font("MS Reference Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    LoadParseFileButton.Location = new Point(8, 280);
    LoadParseFileButton.Margin = new Padding(4, 3, 4, 3);
    LoadParseFileButton.Name = "LoadParseFileButton";
    LoadParseFileButton.Size = new Size(122, 48);
    LoadParseFileButton.TabIndex = 8;
    LoadParseFileButton.Text = "Load Parse File";
    LoadParseFileButton.TextImageRelation = TextImageRelation.ImageBeforeText;
    LoadParseFileButton.UseVisualStyleBackColor = true;
    LoadParseFileButton.Click += LoadParseFileButton_Click;
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(10F, 25F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1517, 628);
    Controls.Add(LoadSpecButton);
    Controls.Add(LoadParseFileButton);
    Controls.Add(ShowUnparsedButton);
    Controls.Add(SaveRuleButton);
    Controls.Add(ClearTokensButton);
    Controls.Add(ClearRulesButton);
    Controls.Add(LoadRulesFileButton);
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
  private ToolStripSeparator ToolStripSeparator1;
  private ToolStripMenuItem GenerateRulesMenuItem;
  private ToolStripMenuItem ExitMenuItem;
  private BindingSource SpecBindingSource;
  private TabControl ItemTabs;
  private TabPage TokenizerPage;
  private TabPage AssemblerPage;
  private Button LoadRulesButton;
  private DataGridView TokenDataGrid;
  private DataGridView TokenRuleDataGrid;
  private DataGridViewTextBoxColumn lastPositionDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn LengthTokenColumn;
  private DataGridViewTextBoxColumn IndexTokenColumn;
  private DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
  private BindingSource TokenBindingSource;
  private Label TokenLabel;
  private Label TokenCountLabel;
  private Label TokenRuleCountLabel;
  private Label TokenRuleLabel;
  private DataGridViewComboBoxColumn TypeGridColumnCombo;
  private DataGridViewTextBoxColumn RuleDataColumnText;
  private DataGridViewTextBoxColumn AssignTypeColumnText;
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
}
