namespace FormObjectViewer;

partial class TokenSequenceNode
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

  #region Component Designer generated code

  /// <summary> 
  /// Required method for Designer support - do not modify 
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent ()
  {
    components = new Container();
    label1 = new Label();
    panel1 = new Panel();
    label2 = new Label();
    button1 = new Button();
    comboBox1 = new ComboBox();
    ChkTokenBindingSource = new BindingSource(components);
    label3 = new Label();
    textBox1 = new TextBox();
    button2 = new Button();
    button3 = new Button();
    checkBox1 = new CheckBox();
    checkBox2 = new CheckBox();
    checkBox3 = new CheckBox();
    label4 = new Label();
    textBox2 = new TextBox();
    checkBox4 = new CheckBox();
    panel1.SuspendLayout();
    ((ISupportInitialize) ChkTokenBindingSource).BeginInit();
    SuspendLayout();
    // 
    // label1
    // 
    label1.AutoSize = true;
    label1.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
    label1.Location = new Point(0, 0);
    label1.Name = "label1";
    label1.Size = new Size(119, 19);
    label1.TabIndex = 0;
    label1.Text = "{TokenType} [#]";
    // 
    // panel1
    // 
    panel1.AutoSize = true;
    panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    panel1.BorderStyle = BorderStyle.Fixed3D;
    panel1.CausesValidation = false;
    panel1.Controls.Add(checkBox4);
    panel1.Controls.Add(checkBox3);
    panel1.Controls.Add(checkBox2);
    panel1.Controls.Add(checkBox1);
    panel1.Controls.Add(textBox2);
    panel1.Controls.Add(textBox1);
    panel1.Controls.Add(label4);
    panel1.Controls.Add(comboBox1);
    panel1.Controls.Add(label3);
    panel1.Controls.Add(button3);
    panel1.Controls.Add(button2);
    panel1.Controls.Add(button1);
    panel1.Controls.Add(label2);
    panel1.Controls.Add(label1);
    panel1.Dock = DockStyle.Fill;
    panel1.Location = new Point(0, 0);
    panel1.Name = "panel1";
    panel1.RightToLeft = RightToLeft.No;
    panel1.Size = new Size(345, 155);
    panel1.TabIndex = 1;
    // 
    // label2
    // 
    label2.AutoSize = true;
    label2.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
    label2.Location = new Point(56, 24);
    label2.Name = "label2";
    label2.Size = new Size(46, 19);
    label2.TabIndex = 0;
    label2.Text = "Type:";
    label2.TextAlign = ContentAlignment.MiddleRight;
    // 
    // button1
    // 
    button1.Font = new Font("Bahnschrift", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    button1.Location = new Point(272, 120);
    button1.Name = "button1";
    button1.Size = new Size(67, 31);
    button1.TabIndex = 1;
    button1.Text = "Save";
    button1.UseVisualStyleBackColor = true;
    // 
    // comboBox1
    // 
    comboBox1.FormattingEnabled = true;
    comboBox1.IntegralHeight = false;
    comboBox1.Location = new Point(104, 24);
    comboBox1.Name = "comboBox1";
    comboBox1.Size = new Size(121, 25);
    comboBox1.TabIndex = 2;
    comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
    // 
    // ChkTokenBindingSource
    // 
    ChkTokenBindingSource.AllowNew = true;
    ChkTokenBindingSource.DataSource = typeof(Parser.Tokens.ChkToken);
    // 
    // label3
    // 
    label3.AutoSize = true;
    label3.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
    label3.Location = new Point(32, 88);
    label3.Name = "label3";
    label3.Size = new Size(69, 19);
    label3.TabIndex = 0;
    label3.Text = "Content:";
    label3.TextAlign = ContentAlignment.MiddleRight;
    // 
    // textBox1
    // 
    textBox1.Location = new Point(104, 88);
    textBox1.Name = "textBox1";
    textBox1.PlaceholderText = "Match Literal";
    textBox1.Size = new Size(120, 25);
    textBox1.TabIndex = 3;
    textBox1.WordWrap = false;
    // 
    // button2
    // 
    button2.Font = new Font("Bahnschrift", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    button2.Location = new Point(8, 120);
    button2.Name = "button2";
    button2.Size = new Size(67, 31);
    button2.TabIndex = 1;
    button2.Text = "Cancel";
    button2.UseVisualStyleBackColor = true;
    // 
    // button3
    // 
    button3.Font = new Font("Bahnschrift", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
    button3.Location = new Point(144, 120);
    button3.Name = "button3";
    button3.Size = new Size(67, 31);
    button3.TabIndex = 1;
    button3.Text = "Check";
    button3.UseVisualStyleBackColor = true;
    // 
    // checkBox1
    // 
    checkBox1.AutoSize = true;
    checkBox1.Location = new Point(232, 16);
    checkBox1.Name = "checkBox1";
    checkBox1.Size = new Size(97, 21);
    checkBox1.TabIndex = 4;
    checkBox1.Text = "Ignore Case";
    checkBox1.UseVisualStyleBackColor = true;
    // 
    // checkBox2
    // 
    checkBox2.AutoSize = true;
    checkBox2.Location = new Point(232, 40);
    checkBox2.Name = "checkBox2";
    checkBox2.Size = new Size(74, 21);
    checkBox2.TabIndex = 4;
    checkBox2.Text = "Multiple";
    checkBox2.UseVisualStyleBackColor = true;
    // 
    // checkBox3
    // 
    checkBox3.AutoSize = true;
    checkBox3.Location = new Point(232, 64);
    checkBox3.Name = "checkBox3";
    checkBox3.Size = new Size(77, 21);
    checkBox3.TabIndex = 4;
    checkBox3.Text = "Optional";
    checkBox3.UseVisualStyleBackColor = true;
    // 
    // label4
    // 
    label4.AutoSize = true;
    label4.Font = new Font("Bahnschrift", 12F, FontStyle.Regular, GraphicsUnit.Point,  0);
    label4.Location = new Point(8, 56);
    label4.Name = "label4";
    label4.Size = new Size(92, 19);
    label4.TabIndex = 0;
    label4.Text = "Token Type:";
    label4.TextAlign = ContentAlignment.MiddleRight;
    // 
    // textBox2
    // 
    textBox2.Location = new Point(104, 56);
    textBox2.Name = "textBox2";
    textBox2.PlaceholderText = "Match Token Type";
    textBox2.Size = new Size(120, 25);
    textBox2.TabIndex = 3;
    textBox2.WordWrap = false;
    // 
    // checkBox4
    // 
    checkBox4.AutoSize = true;
    checkBox4.Location = new Point(232, 88);
    checkBox4.Name = "checkBox4";
    checkBox4.Size = new Size(48, 21);
    checkBox4.TabIndex = 4;
    checkBox4.Text = "Any";
    checkBox4.UseVisualStyleBackColor = true;
    // 
    // UserControl1
    // 
    AutoScaleDimensions = new SizeF(7F, 17F);
    AutoScaleMode = AutoScaleMode.Font;
    Controls.Add(panel1);
    Name = "UserControl1";
    Size = new Size(345, 155);
    panel1.ResumeLayout(false);
    panel1.PerformLayout();
    ((ISupportInitialize) ChkTokenBindingSource).EndInit();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private Label label1;
  private Panel panel1;
  private Button button1;
  private Label label2;
  private ComboBox comboBox1;
  private Label label3;
  private BindingSource ChkTokenBindingSource;
  private CheckBox checkBox3;
  private CheckBox checkBox2;
  private CheckBox checkBox1;
  private TextBox textBox2;
  private TextBox textBox1;
  private Label label4;
  private Button button3;
  private Button button2;
  private CheckBox checkBox4;
}
