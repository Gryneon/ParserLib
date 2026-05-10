using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabelGen;

public partial class MainForm : Form
{
  private const string TemplatesFolderName = "templates";
  private readonly string templatesFolderPath;
  private string currentTemplateText = "";
  private readonly Dictionary<string, string> variables = new();

  public MainForm()
  {
    InitializeComponent();
    templatesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplatesFolderName);
    EnsureTemplatesFolder();
    LoadTemplatesList();
    UpdateUiState();
  }

  private void EnsureTemplatesFolder()
  {
    if (!Directory.Exists(templatesFolderPath))
    {
      Directory.CreateDirectory(templatesFolderPath);
    }
  }

  private void LoadTemplatesList()
  {
    listBoxTemplates.Items.Clear();
    string[] files = Directory.GetFiles(templatesFolderPath, "*.*");
    foreach (string f in files)
    {
      listBoxTemplates.Items.Add(Path.GetFileName(f));
    }
  }

  private void UpdateUiState()
  {
    buttonOpenPrintForm.Enabled = listBoxTemplates.SelectedItem != null;
  }

  private void listBoxTemplates_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (listBoxTemplates.SelectedItem == null)
    {
      currentTemplateText = "";
      variables.Clear();
      dataGridViewVars.DataSource = null;
      UpdateUiState();
      return;
    }

    string? name = listBoxTemplates.SelectedItem.ToString();
    string path = Path.Combine(templatesFolderPath, name);
    try
    {
      currentTemplateText = File.ReadAllText(path);
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Failed to load template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      currentTemplateText = "";
    }

    ParseVariablesFromTemplate();
    PopulateGridFromVariables();
    UpdateUiState();
  }

  private void ParseVariablesFromTemplate()
  {
    variables.Clear();
    if (string.IsNullOrEmpty(currentTemplateText)) return;

    // Find all occurrences of <<variable_name>>
    MatchCollection matches = Regex.Matches(currentTemplateText, @"<<(?<name>[^<>]+)>>");
    foreach (Match m in matches)
    {
      string name = m.Groups["name"].Value.Trim();
      if (!variables.ContainsKey(name))
      {
        variables[name] = "";
      }
    }
  }

  private void PopulateGridFromVariables()
  {
    var dt = new DataTable();
    dt.Columns.Add("Variable", typeof(string));
    dt.Columns.Add("Value", typeof(string));

    foreach (KeyValuePair<string, string> kv in variables)
    {
      DataRow row = dt.NewRow();
      row["Variable"] = kv.Key;
      row["Value"] = kv.Value;
      dt.Rows.Add(row);
    }

    dataGridViewVars.DataSource = dt;
    dataGridViewVars.Columns["Variable"]?.ReadOnly = true;
    dataGridViewVars.AutoResizeColumns();
  }

  private void buttonRefreshTemplates_Click(object sender, EventArgs e)
  {
    LoadTemplatesList();
  }

  private void buttonOpenPrintForm_Click(object sender, EventArgs e)
  {
    if (listBoxTemplates.SelectedItem == null)
    {
      MessageBox.Show("Please select a template first.", "No template", MessageBoxButtons.OK, MessageBoxIcon.Information);
      return;
    }

    // Read values from grid into variables dictionary
    SaveGridToVariables();

    string filled = BuildFilledTemplate();
    var printForm = new PrintForm(filled);
    // Provide ability to override FTP defaults via properties on the print form if needed
    printForm.ShowDialog(this);
  }

  private void SaveGridToVariables()
  {
    if (dataGridViewVars.DataSource is DataTable dt)
    {
      foreach (DataRow r in dt.Rows)
      {
        string name = r["Variable"].ToString()!;
        string val = r["Value"].ToString()!;
        if (variables.ContainsKey(name))
          variables[name] = val;
      }
    }
  }

  private string BuildFilledTemplate()
  {
    string result = currentTemplateText;
    foreach (KeyValuePair<string, string> kv in variables)
    {
      // Replace all occurrences of <<name>> with value
      result = result.Replace("<<" + kv.Key + ">>", kv.Value ?? "", StringComparison.OrdinalIgnoreCase);
    }
    return result;
  }

  private void buttonLoadTemplateFromFile_Click(object sender, EventArgs e)
  {
    using var ofd = new OpenFileDialog();
    ofd.Filter = string.Join("|", "Text files|*.txt", "Label FIles|*.pr1", "Intermec Label Files|*.ipl", "All files|*.*");
    ofd.InitialDirectory = templatesFolderPath;
    if (ofd.ShowDialog() == DialogResult.OK)
    {
      try
      {
        string dest = Path.Combine(templatesFolderPath, Path.GetFileName(ofd.FileName));
        File.Copy(ofd.FileName, dest, overwrite: true);
        LoadTemplatesList();
        MessageBox.Show("Template copied to templates folder.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Failed to copy template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }

  private void buttonEditTemplate_Click(object sender, EventArgs e)
  {
    if (listBoxTemplates.SelectedItem == null) return;
    string? name = listBoxTemplates.SelectedItem.ToString();
    string path = Path.Combine(templatesFolderPath, name + ".txt");
    try
    {
      Process.Start(new ProcessStartInfo()
      {
        FileName = path,
        UseShellExecute = true
      });
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Failed to open template file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  private void buttonNewTemplate_Click(object sender, EventArgs e)
  {
    using var sfd = new SaveFileDialog();
    sfd.Filter = "All Files|*.*|Intermec Label Templates|*.ipl|Label Templates|*.pr1";
    sfd.InitialDirectory = templatesFolderPath;
    sfd.FileName = "NewTemplate.ipl";
    if (sfd.ShowDialog() == DialogResult.OK)
    {
      try
      {
        File.WriteAllText(sfd.FileName, ""); // create empty template
        LoadTemplatesList();
        MessageBox.Show("New template created. Edit it from the templates folder.", "Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Failed to create template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
