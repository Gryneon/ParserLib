using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;


namespace LabelGen;

  partial class MainForm
  {
    private IContainer components = null;
    private ListBox listBoxTemplates;
    private DataGridView dataGridViewVars;
    private Button buttonOpenPrintForm;
    private Button buttonRefreshTemplates;
    private Button buttonLoadTemplateFromFile;
    private Button buttonEditTemplate;
    private Button buttonNewTemplate;
    private Label labelTemplates;
    private Label labelVariables;

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
    listBoxTemplates = new ListBox();
    dataGridViewVars = new DataGridView();
    buttonOpenPrintForm = new Button();
    buttonRefreshTemplates = new Button();
    buttonLoadTemplateFromFile = new Button();
    buttonEditTemplate = new Button();
    buttonNewTemplate = new Button();
    labelTemplates = new Label();
    labelVariables = new Label();
    ((ISupportInitialize) dataGridViewVars).BeginInit();
    SuspendLayout();
    // 
    // listBoxTemplates
    // 
    listBoxTemplates.FormattingEnabled = true;
    listBoxTemplates.Location = new Point(12, 29);
    listBoxTemplates.Name = "listBoxTemplates";
    listBoxTemplates.Size = new Size(220, 274);
    listBoxTemplates.TabIndex = 0;
    listBoxTemplates.SelectedIndexChanged += listBoxTemplates_SelectedIndexChanged;
    // 
    // dataGridViewVars
    // 
    dataGridViewVars.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
    dataGridViewVars.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    dataGridViewVars.Location = new Point(248, 29);
    dataGridViewVars.Name = "dataGridViewVars";
    dataGridViewVars.Size = new Size(420, 274);
    dataGridViewVars.TabIndex = 1;
    // 
    // buttonOpenPrintForm
    // 
    buttonOpenPrintForm.Anchor =  AnchorStyles.Bottom | AnchorStyles.Right;
    buttonOpenPrintForm.Location = new Point(593, 315);
    buttonOpenPrintForm.Name = "buttonOpenPrintForm";
    buttonOpenPrintForm.Size = new Size(75, 30);
    buttonOpenPrintForm.TabIndex = 2;
    buttonOpenPrintForm.Text = "Print";
    buttonOpenPrintForm.UseVisualStyleBackColor = true;
    buttonOpenPrintForm.Click += buttonOpenPrintForm_Click;
    // 
    // buttonRefreshTemplates
    // 
    buttonRefreshTemplates.Location = new Point(12, 315);
    buttonRefreshTemplates.Name = "buttonRefreshTemplates";
    buttonRefreshTemplates.Size = new Size(75, 30);
    buttonRefreshTemplates.TabIndex = 3;
    buttonRefreshTemplates.Text = "Refresh";
    buttonRefreshTemplates.UseVisualStyleBackColor = true;
    buttonRefreshTemplates.Click += buttonRefreshTemplates_Click;
    // 
    // buttonLoadTemplateFromFile
    // 
    buttonLoadTemplateFromFile.Location = new Point(93, 315);
    buttonLoadTemplateFromFile.Name = "buttonLoadTemplateFromFile";
    buttonLoadTemplateFromFile.Size = new Size(75, 30);
    buttonLoadTemplateFromFile.TabIndex = 4;
    buttonLoadTemplateFromFile.Text = "Import";
    buttonLoadTemplateFromFile.UseVisualStyleBackColor = true;
    buttonLoadTemplateFromFile.Click += buttonLoadTemplateFromFile_Click;
    // 
    // buttonEditTemplate
    // 
    buttonEditTemplate.Location = new Point(174, 315);
    buttonEditTemplate.Name = "buttonEditTemplate";
    buttonEditTemplate.Size = new Size(75, 30);
    buttonEditTemplate.TabIndex = 5;
    buttonEditTemplate.Text = "Edit";
    buttonEditTemplate.UseVisualStyleBackColor = true;
    buttonEditTemplate.Click += buttonEditTemplate_Click;
    // 
    // buttonNewTemplate
    // 
    buttonNewTemplate.Location = new Point(255, 315);
    buttonNewTemplate.Name = "buttonNewTemplate";
    buttonNewTemplate.Size = new Size(75, 30);
    buttonNewTemplate.TabIndex = 6;
    buttonNewTemplate.Text = "New";
    buttonNewTemplate.UseVisualStyleBackColor = true;
    buttonNewTemplate.Click += buttonNewTemplate_Click;
    // 
    // labelTemplates
    // 
    labelTemplates.AutoSize = true;
    labelTemplates.Location = new Point(12, 9);
    labelTemplates.Name = "labelTemplates";
    labelTemplates.Size = new Size(61, 15);
    labelTemplates.TabIndex = 7;
    labelTemplates.Text = "Templates";
    // 
    // labelVariables
    // 
    labelVariables.AutoSize = true;
    labelVariables.Location = new Point(248, 9);
    labelVariables.Name = "labelVariables";
    labelVariables.Size = new Size(53, 15);
    labelVariables.TabIndex = 8;
    labelVariables.Text = "Variables";
    // 
    // MainForm
    // 
    ClientSize = new Size(684, 357);
    Controls.Add(labelVariables);
    Controls.Add(labelTemplates);
    Controls.Add(buttonNewTemplate);
    Controls.Add(buttonEditTemplate);
    Controls.Add(buttonLoadTemplateFromFile);
    Controls.Add(buttonRefreshTemplates);
    Controls.Add(buttonOpenPrintForm);
    Controls.Add(dataGridViewVars);
    Controls.Add(listBoxTemplates);
    MinimumSize = new Size(700, 396);
    Name = "MainForm";
    Text = "Label Printer - Template Selector";
    ((ISupportInitialize) dataGridViewVars).EndInit();
    ResumeLayout(false);
    PerformLayout();

  }
}