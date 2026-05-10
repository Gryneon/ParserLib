using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace LabelGen;

public partial class PrintForm : Form
{
  private string filledLabelText;

  // Defaults as requested
  private readonly string defaultFtpHost = "10.68.89.233";

  private readonly string defaultFtpUser = "sato";
  private readonly string defaultFtpPass = "pass";
  private readonly int defaultFtpPort = 21;

  public PrintForm(string filledLabel)
  {
    InitializeComponent();
    filledLabelText = filledLabel ?? "";
    textBoxPreview.Text = filledLabelText;
    textBoxHost.Text = defaultFtpHost;
    textBoxUser.Text = defaultFtpUser;
    textBoxPass.Text = defaultFtpPass;
    numericUpDownPort.Value = defaultFtpPort;
  }

  private void buttonCancel_Click(object sender, EventArgs e)
  {
    Close();
  }

  private void buttonPrint_Click(object? sender, EventArgs e)
  {
    // Update preview from the text box in case user edited it
    filledLabelText = textBoxPreview.Text;

    // Create label.pr1 in a temp location
    string tempPath = Path.Combine(Path.GetTempPath(), "label.pr1");
    try
    {
      File.WriteAllText(tempPath, filledLabelText, Encoding.UTF8);
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Failed to create label file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      return;
    }

    // FTP upload
    string host = textBoxHost.Text.Trim();
    string user = textBoxUser.Text;
    string pass = textBoxPass.Text;
    int port = (int)numericUpDownPort.Value;

    if (string.IsNullOrEmpty(host))
    {
      MessageBox.Show("Please provide an FTP host/IP.", "Missing host", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      return;
    }

    var uri = new Uri($"ftp://{host}:{port}/label.pr1");

    try
    {
      UploadFileToFtp(uri, tempPath, user, pass);
      MessageBox.Show("Label uploaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
      Close();
    }
    catch (Exception ex)
    {
      MessageBox.Show($"FTP upload failed: {ex.Message}", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
      // Attempt to delete temp file
      try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
    }
  }

  private void UploadFileToFtp(Uri uri, string localFilePath, string username, string password)
  {
    var request = WebRequest.Create(uri) as FtpWebRequest;
    request.Method = WebRequestMethods.Ftp.UploadFile;
    request.Credentials = new NetworkCredential(username, password);
    request.UseBinary = true;
    request.UsePassive = true;
    request.KeepAlive = false;

    byte[] fileContents = File.ReadAllBytes(localFilePath);
    request.ContentLength = fileContents.Length;

    using (Stream requestStream = request.GetRequestStream())
    {
      requestStream.Write(fileContents, 0, fileContents.Length);
    }

    using var response = (FtpWebResponse) request.GetResponse();
    // Optionally check response.StatusDescription
  }
}
