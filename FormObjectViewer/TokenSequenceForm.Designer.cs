namespace FormObjectViewer;

partial class TokenSequenceForm
{
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
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
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent ()
  {
    tableLayoutPanel1 = new TableLayoutPanel();
    textBox1 = new TextBox();
    tableLayoutPanel1.SuspendLayout();
    SuspendLayout();
    // 
    // tableLayoutPanel1
    // 
    tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
    tableLayoutPanel1.ColumnCount = 10;
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
    tableLayoutPanel1.Controls.Add(textBox1, 0, 0);
    tableLayoutPanel1.Dock = DockStyle.Fill;
    tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
    tableLayoutPanel1.ImeMode = ImeMode.NoControl;
    tableLayoutPanel1.Location = new Point(0, 0);
    tableLayoutPanel1.Name = "tableLayoutPanel1";
    tableLayoutPanel1.RowCount = 2;
    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    tableLayoutPanel1.Size = new Size(1183, 173);
    tableLayoutPanel1.TabIndex = 0;
    // 
    // textBox1
    // 
    textBox1.Anchor =  AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    textBox1.Location = new Point(4, 4);
    textBox1.Name = "textBox1";
    textBox1.Size = new Size(111, 25);
    textBox1.TabIndex = 0;
    // 
    // TokenSequenceForm
    // 
    AutoScaleDimensions = new SizeF(7F, 17F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1183, 173);
    Controls.Add(tableLayoutPanel1);
    Name = "TokenSequenceForm";
    Text = "TokenSequenceForm";
    tableLayoutPanel1.ResumeLayout(false);
    tableLayoutPanel1.PerformLayout();
    ResumeLayout(false);
  }

  #endregion

  private TableLayoutPanel tableLayoutPanel1;
  private TextBox textBox1;
}