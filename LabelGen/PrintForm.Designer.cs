using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LabelGen;
  partial class PrintForm
  {
    private IContainer components = null;
    private TextBox textBoxPreview;
    private Button buttonPrint;
    private Button buttonCancel;
    private Label labelPreview;
    private Label labelHost;
    private TextBox textBoxHost;
    private Label labelUser;
    private TextBox textBoxUser;
    private Label labelPass;
    private TextBox textBoxPass;
    private Label labelPort;
    private NumericUpDown numericUpDownPort;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBoxPreview = new TextBox();
      this.buttonPrint = new Button();
      this.buttonCancel = new Button();
      this.labelPreview = new Label();
      this.labelHost = new Label();
      this.textBoxHost = new TextBox();
      this.labelUser = new Label();
      this.textBoxUser = new TextBox();
      this.labelPass = new Label();
      this.textBoxPass = new TextBox();
      this.labelPort = new Label();
      this.numericUpDownPort = new NumericUpDown();
      ((ISupportInitialize) (this.numericUpDownPort)).BeginInit();
      this.SuspendLayout();
      // 
      // textBoxPreview
      // 
      this.textBoxPreview.Anchor = ((AnchorStyles) ((((AnchorStyles.Top | AnchorStyles.Bottom)
                  | AnchorStyles.Left)
                  | AnchorStyles.Right)));
      this.textBoxPreview.Location = new Point(12, 29);
      this.textBoxPreview.Multiline = true;
      this.textBoxPreview.Name = "textBoxPreview";
      this.textBoxPreview.ScrollBars = ScrollBars.Vertical;
      this.textBoxPreview.Size = new Size(560, 260);
      this.textBoxPreview.TabIndex = 0;
      // 
      // buttonPrint
      // 
      this.buttonPrint.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
      this.buttonPrint.Location = new Point(497, 360);
      this.buttonPrint.Name = "buttonPrint";
      this.buttonPrint.Size = new Size(75, 30);
      this.buttonPrint.TabIndex = 1;
      this.buttonPrint.Text = "Print";
      this.buttonPrint.UseVisualStyleBackColor = true;
      this.buttonPrint.Click += new EventHandler(this.buttonPrint_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Right)));
      this.buttonCancel.Location = new Point(416, 360);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 30);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // labelPreview
      // 
      this.labelPreview.AutoSize = true;
      this.labelPreview.Location = new Point(12, 9);
      this.labelPreview.Name = "labelPreview";
      this.labelPreview.Size = new Size(46, 15);
      this.labelPreview.TabIndex = 3;
      this.labelPreview.Text = "Preview";
      // 
      // labelHost
      // 
      this.labelHost.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.labelHost.AutoSize = true;
      this.labelHost.Location = new Point(12, 302);
      this.labelHost.Name = "labelHost";
      this.labelHost.Size = new Size(29, 15);
      this.labelHost.TabIndex = 4;
      this.labelHost.Text = "Host";
      // 
      // textBoxHost
      // 
      this.textBoxHost.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.textBoxHost.Location = new Point(12, 320);
      this.textBoxHost.Name = "textBoxHost";
      this.textBoxHost.Size = new Size(160, 23);
      this.textBoxHost.TabIndex = 5;
      // 
      // labelUser
      // 
      this.labelUser.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.labelUser.AutoSize = true;
      this.labelUser.Location = new Point(182, 302);
      this.labelUser.Name = "labelUser";
      this.labelUser.Size = new Size(29, 15);
      this.labelUser.TabIndex = 6;
      this.labelUser.Text = "User";
      // 
      // textBoxUser
      // 
      this.textBoxUser.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.textBoxUser.Location = new Point(182, 320);
      this.textBoxUser.Name = "textBoxUser";
      this.textBoxUser.Size = new Size(120, 23);
      this.textBoxUser.TabIndex = 7;
      // 
      // labelPass
      // 
      this.labelPass.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.labelPass.AutoSize = true;
      this.labelPass.Location = new Point(308, 302);
      this.labelPass.Name = "labelPass";
      this.labelPass.Size = new Size(57, 15);
      this.labelPass.TabIndex = 8;
      this.labelPass.Text = "Password";
      // 
      // textBoxPass
      // 
      this.textBoxPass.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.textBoxPass.Location = new Point(308, 320);
      this.textBoxPass.Name = "textBoxPass";
      this.textBoxPass.PasswordChar = '*';
      this.textBoxPass.Size = new Size(120, 23);
      this.textBoxPass.TabIndex = 9;
      // 
      // labelPort
      // 
      this.labelPort.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.labelPort.AutoSize = true;
      this.labelPort.Location = new Point(434, 302);
      this.labelPort.Name = "labelPort";
      this.labelPort.Size = new Size(29, 15);
      this.labelPort.TabIndex = 10;
      this.labelPort.Text = "Port";
      // 
      // numericUpDownPort
      // 
      this.numericUpDownPort.Anchor = ((AnchorStyles) ((AnchorStyles.Bottom | AnchorStyles.Left)));
      this.numericUpDownPort.Location = new Point(434, 320);
      this.numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
      this.numericUpDownPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownPort.Name = "numericUpDownPort";
      this.numericUpDownPort.Size = new Size(80, 23);
      this.numericUpDownPort.TabIndex = 11;
      this.numericUpDownPort.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
      // 
      // PrintForm
      // 
      this.ClientSize = new Size(584, 401);
      this.Controls.Add(this.numericUpDownPort);
      this.Controls.Add(this.labelPort);
      this.Controls.Add(this.textBoxPass);
      this.Controls.Add(this.labelPass);
      this.Controls.Add(this.textBoxUser);
      this.Controls.Add(this.labelUser);
      this.Controls.Add(this.textBoxHost);
      this.Controls.Add(this.labelHost);
      this.Controls.Add(this.labelPreview);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonPrint);
      this.Controls.Add(this.textBoxPreview);
      this.MinimumSize = new Size(600, 440);
      this.Name = "PrintForm";
      this.Text = "Print Preview and Send";
      ((System.ComponentModel.ISupportInitialize) (this.numericUpDownPort)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
