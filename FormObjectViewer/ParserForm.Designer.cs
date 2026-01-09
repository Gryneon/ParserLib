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
    DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
    StatusStrip = new StatusStrip();
    MainTabs = new TabControl();
    SpecTab = new TabPage();
    SpecComboBox = new ComboBox();
    label1 = new Label();
    OperationsTab = new TabPage();
    dataGridView1 = new DataGridView();
    DataTab = new TabPage();
    OpenParseFileDialog = new OpenFileDialog();
    menuStrip1 = new MenuStrip();
    StatusStrip.SuspendLayout();
    MainTabs.SuspendLayout();
    SpecTab.SuspendLayout();
    OperationsTab.SuspendLayout();
    DataTab.SuspendLayout();
    menuStrip1.SuspendLayout();
    SuspendLayout();
    // 
    // StatusStrip
    // 
    StatusStrip.ImageScalingSize = new Size(20, 20);
    StatusStrip.Location = new Point(0, 410);
    StatusStrip.Name = "StatusStrip";
    StatusStrip.Size = new Size(800, 40);
    StatusStrip.TabIndex = 0;
    StatusStrip.Text = "StatusStrip";
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
    // SpecTab
    // 
    SpecTab.BorderStyle = BorderStyle.FixedSingle;
    SpecTab.Controls.Add(SpecComboBox);
    SpecTab.Controls.Add(label1);
    SpecTab.Location = new Point(4, 38);
    SpecTab.Name = "SpecTab";
    SpecTab.Padding = new Padding(3);
    SpecTab.Size = new Size(792, 328);
    SpecTab.TabIndex = 2;
    SpecTab.Text = "Spec";
    SpecTab.UseVisualStyleBackColor = true;
    // 
    // SpecComboBox
    // 
    SpecComboBox.FormattingEnabled = true;
    SpecComboBox.Location = new Point(64, 16);
    SpecComboBox.Name = "SpecComboBox";
    SpecComboBox.Size = new Size(208, 37);
    SpecComboBox.TabIndex = 1;
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Location = new Point(8, 16);
    label1.Name = "label1";
    label1.Size = new Size(66, 29);
    label1.TabIndex = 0;
    label1.Text = "Spec";
    // 
    // OperationsTab
    // 
    OperationsTab.Location = new Point(4, 38);
    OperationsTab.Name = "OperationsTab";
    OperationsTab.Padding = new Padding(3);
    OperationsTab.Size = new Size(792, 328);
    OperationsTab.TabIndex = 0;
    OperationsTab.Text = "Operate";
    OperationsTab.UseVisualStyleBackColor = true;
    // 
    // DataTab
    // 
    DataTab.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
    DataTab.Location = new Point(4, 38);
    DataTab.Name = "DataTab";
    DataTab.Padding = new Padding(3);
    DataTab.Size = new Size(792, 328);
    DataTab.TabIndex = 1;
    DataTab.Text = "Data";
    DataTab.UseVisualStyleBackColor = true;
    // 
    // ParserDataGrid
    // 
    dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
    dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
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
    menuStrip1.Location = new Point(0, 0);
    menuStrip1.Name = "menuStrip1";
    menuStrip1.Size = new Size(800, 28);
    menuStrip1.TabIndex = 2;
    menuStrip1.Text = "menuStrip1";
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
    SpecTab.ResumeLayout(false);
    SpecTab.PerformLayout();
    OperationsTab.ResumeLayout(false);
    DataTab.ResumeLayout(false);
    menuStrip1.ResumeLayout(false);
    menuStrip1.PerformLayout();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private StatusStrip StatusStrip;
  private TabControl MainTabs;
  private TabPage OperationsTab;
  private TabPage DataTab;
  private OpenFileDialog OpenParseFileDialog;
  private ComboBox SpecComboBox;
  private Label label1;
  public TabPage SpecTab;
  private MenuStrip menuStrip1;
}
