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
    StatusStrip = new StatusStrip();
    ToolStripIcon = new ToolStripStatusLabel();
    ToolStripStatusLabel = new ToolStripStatusLabel();
    MainTabs = new TabControl();
    OperationsTab = new TabPage();
    dataGridView1 = new DataGridView();
    DataTab = new TabPage();
    ParserDataGrid = new DataGridView();
    SpecTab = new TabPage();
    SpecComboBox = new ComboBox();
    label1 = new Label();
    OpenParseFileDialog = new OpenFileDialog();
    menuStrip1 = new MenuStrip();
    fileToolStripMenuItem = new ToolStripMenuItem();
    openToolStripMenuItem = new ToolStripMenuItem();
    toolStripSeparator1 = new ToolStripSeparator();
    exitToolStripMenuItem = new ToolStripMenuItem();
    operationsToolStripMenuItem = new ToolStripMenuItem();
    advanceToolStripMenuItem = new ToolStripMenuItem();
    restartToolStripMenuItem = new ToolStripMenuItem();
    StatusStrip.SuspendLayout();
    MainTabs.SuspendLayout();
    OperationsTab.SuspendLayout();
    ((ISupportInitialize) dataGridView1).BeginInit();
    DataTab.SuspendLayout();
    ((ISupportInitialize) ParserDataGrid).BeginInit();
    SpecTab.SuspendLayout();
    menuStrip1.SuspendLayout();
    SuspendLayout();
    // 
    // StatusStrip
    // 
    StatusStrip.Items.AddRange(new ToolStripItem[] { ToolStripIcon, ToolStripStatusLabel });
    StatusStrip.Location = new Point(0, 410);
    StatusStrip.Name = "StatusStrip";
    StatusStrip.Size = new Size(800, 40);
    StatusStrip.TabIndex = 0;
    StatusStrip.Text = "StatusStrip";
    // 
    // ToolStripIcon
    // 
    ToolStripIcon.AutoSize = false;
    ToolStripIcon.BackgroundImageLayout = ImageLayout.None;
    ToolStripIcon.DisplayStyle = ToolStripItemDisplayStyle.Image;
    ToolStripIcon.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    ToolStripIcon.ImageAlign = ContentAlignment.MiddleLeft;
    ToolStripIcon.ImageScaling = ToolStripItemImageScaling.None;
    ToolStripIcon.Name = "ToolStripIcon";
    ToolStripIcon.Size = new Size(40, 35);
    ToolStripIcon.Text = "Unloaded";
    // 
    // ToolStripStatusLabel
    // 
    ToolStripStatusLabel.Font = new Font("Cascadia Code", 14.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
    ToolStripStatusLabel.Name = "ToolStripStatusLabel";
    ToolStripStatusLabel.Size = new Size(199, 35);
    ToolStripStatusLabel.Text = "No Parser Created";
    // 
    // MainTabs
    // 
    MainTabs.AccessibleRole = AccessibleRole.PageTab;
    MainTabs.Controls.Add(SpecTab);
    MainTabs.Controls.Add(OperationsTab);
    MainTabs.Controls.Add(DataTab);
    MainTabs.Dock = DockStyle.Bottom;
    MainTabs.Font = new Font("Bahnschrift", 14.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
    MainTabs.HotTrack = true;
    MainTabs.Location = new Point(0, 40);
    MainTabs.Multiline = true;
    MainTabs.Name = "MainTabs";
    MainTabs.SelectedIndex = 0;
    MainTabs.Size = new Size(800, 370);
    MainTabs.SizeMode = TabSizeMode.FillToRight;
    MainTabs.TabIndex = 1;
    // 
    // OperationsTab
    // 
    OperationsTab.Controls.Add(dataGridView1);
    OperationsTab.Location = new Point(4, 32);
    OperationsTab.Name = "OperationsTab";
    OperationsTab.Padding = new Padding(3);
    OperationsTab.Size = new Size(792, 334);
    OperationsTab.TabIndex = 0;
    OperationsTab.Text = "Operate";
    OperationsTab.UseVisualStyleBackColor = true;
    // 
    // dataGridView1
    // 
    dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    dataGridView1.Dock = DockStyle.Bottom;
    dataGridView1.Location = new Point(3, 155);
    dataGridView1.Name = "dataGridView1";
    dataGridView1.Size = new Size(786, 176);
    dataGridView1.TabIndex = 0;
    // 
    // DataTab
    // 
    DataTab.Controls.Add(ParserDataGrid);
    DataTab.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
    DataTab.Location = new Point(4, 32);
    DataTab.Name = "DataTab";
    DataTab.Padding = new Padding(3);
    DataTab.Size = new Size(792, 334);
    DataTab.TabIndex = 1;
    DataTab.Text = "Data";
    DataTab.UseVisualStyleBackColor = true;
    // 
    // ParserDataGrid
    // 
    ParserDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    ParserDataGrid.Dock = DockStyle.Bottom;
    ParserDataGrid.Location = new Point(3, 163);
    ParserDataGrid.Name = "ParserDataGrid";
    ParserDataGrid.Size = new Size(786, 168);
    ParserDataGrid.TabIndex = 0;
    // 
    // SpecTab
    // 
    SpecTab.BorderStyle = BorderStyle.FixedSingle;
    SpecTab.Controls.Add(SpecComboBox);
    SpecTab.Controls.Add(label1);
    SpecTab.Location = new Point(4, 32);
    SpecTab.Name = "SpecTab";
    SpecTab.Padding = new Padding(3);
    SpecTab.Size = new Size(792, 334);
    SpecTab.TabIndex = 2;
    SpecTab.Text = "Spec";
    SpecTab.UseVisualStyleBackColor = true;
    // 
    // SpecComboBox
    // 
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(64, 16);
    SpecComboBox.Name = "SpecComboBox";
    SpecComboBox.Size = new Size(208, 31);
    SpecComboBox.TabIndex = 1;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(8, 16);
    label1.Name = "label1";
    label1.Size = new Size(51, 23);
    label1.TabIndex = 0;
    label1.Text = "Spec";
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
    menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, operationsToolStripMenuItem });
    menuStrip1.Location = new Point(0, 0);
    menuStrip1.Name = "menuStrip1";
    menuStrip1.Size = new Size(800, 28);
    menuStrip1.TabIndex = 2;
    menuStrip1.Text = "menuStrip1";
    // 
    // fileToolStripMenuItem
    // 
    fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
    fileToolStripMenuItem.Name = "fileToolStripMenuItem";
    fileToolStripMenuItem.Size = new Size(44, 24);
    fileToolStripMenuItem.Text = "File";
    // 
    // openToolStripMenuItem
    // 
    openToolStripMenuItem.Name = "openToolStripMenuItem";
    openToolStripMenuItem.Size = new Size(180, 24);
    openToolStripMenuItem.Text = "&Open...";
    // 
    // toolStripSeparator1
    // 
    toolStripSeparator1.Name = "toolStripSeparator1";
    toolStripSeparator1.Size = new Size(177, 6);
    // 
    // exitToolStripMenuItem
    // 
    exitToolStripMenuItem.Name = "exitToolStripMenuItem";
    exitToolStripMenuItem.Size = new Size(180, 24);
    exitToolStripMenuItem.Text = "Exit";
    // 
    // operationsToolStripMenuItem
    // 
    operationsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { advanceToolStripMenuItem, restartToolStripMenuItem });
    operationsToolStripMenuItem.Name = "operationsToolStripMenuItem";
    operationsToolStripMenuItem.Size = new Size(94, 24);
    operationsToolStripMenuItem.Text = "Operations";
    // 
    // advanceToolStripMenuItem
    // 
    advanceToolStripMenuItem.Enabled = false;
    advanceToolStripMenuItem.Name = "advanceToolStripMenuItem";
    advanceToolStripMenuItem.Size = new Size(180, 24);
    advanceToolStripMenuItem.Text = "Advance";
    // 
    // restartToolStripMenuItem
    // 
    restartToolStripMenuItem.Enabled = false;
    restartToolStripMenuItem.Name = "restartToolStripMenuItem";
    restartToolStripMenuItem.Size = new Size(180, 24);
    restartToolStripMenuItem.Text = "Restart";
    // 
    // ParserForm
    // 
    AutoScaleDimensions = new SizeF(8F, 20F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(800, 450);
    Controls.Add(MainTabs);
    Controls.Add(StatusStrip);
    Controls.Add(menuStrip1);
    MainMenuStrip = menuStrip1;
    Name = "ParserForm";
    Text = "Parser Form";
    Load += ParserForm_Load;
    StatusStrip.ResumeLayout(false);
    StatusStrip.PerformLayout();
    MainTabs.ResumeLayout(false);
    OperationsTab.ResumeLayout(false);
    ((ISupportInitialize) dataGridView1).EndInit();
    DataTab.ResumeLayout(false);
    ((ISupportInitialize) ParserDataGrid).EndInit();
    SpecTab.ResumeLayout(false);
    SpecTab.PerformLayout();
    menuStrip1.ResumeLayout(false);
    menuStrip1.PerformLayout();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  protected StatusStrip StatusStrip;
  public ToolStripStatusLabel StatusLabel;
  public ToolStripStatusLabel ToolStripIcon;
  protected TabControl MainTabs;
  private TabPage OperationsTab;
  private TabPage DataTab;
  private OpenFileDialog OpenParseFileDialog;
  private ComboBox SpecComboBox;
  private Label label1;
  public TabPage SpecTab;
  private DataGridView dataGridView1;
  private DataGridViewCheckBoxColumn continueOnFailDataGridViewCheckBoxColumn;
  private DataGridViewCheckBoxColumn skipOperationDataGridViewCheckBoxColumn;
  private DataGridViewCheckBoxColumn ignoreAllLoadsDataGridViewCheckBoxColumn;
  private DataGridViewCheckBoxColumn neverExecutesDataGridViewCheckBoxColumn;
  private DataGridViewTextBoxColumn loopBreakDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn loopStartDataGridViewTextBoxColumn;
  private BindingSource operationsBindingSource;
  private DataGridView ParserDataGrid;
  private BindingSource itemActionsBindingSource;
  private DataGridViewTextBoxColumn opIndexDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn nextOpIndexDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn currentOpDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn nextOpDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn lastStatusDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn localDefaultSpecDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn specDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn labelsDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn dataDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn resultDataGridViewTextBoxColumn;
  private DataGridViewCheckBoxColumn hasResultDataGridViewCheckBoxColumn;
  private DataGridViewTextBoxColumn opCountDataGridViewTextBoxColumn;
  private DataGridViewTextBoxColumn fileDataDataGridViewTextBoxColumn;
  private BindingSource xParserBindingSource;
  private ToolStripStatusLabel ToolStripStatusLabel;
  private MenuStrip menuStrip1;
  private ToolStripMenuItem fileToolStripMenuItem;
  private ToolStripMenuItem openToolStripMenuItem;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripMenuItem exitToolStripMenuItem;
  private ToolStripMenuItem operationsToolStripMenuItem;
  private ToolStripMenuItem advanceToolStripMenuItem;
  private ToolStripMenuItem restartToolStripMenuItem;
}
