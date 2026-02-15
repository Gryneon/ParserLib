#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1303 // Do not pass literals as localized parameters
#pragma warning disable CA2000 // Dispose objects before losing scope

using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FormObjectViewer;

internal sealed class MainForm : Form, IDisposable
{
  private readonly PictureBox _pictureBox;

  public MainForm ()
  {
    Text = "Proprietary Image Viewer";
    Width = 800;
    Height = 600;

    _pictureBox = new PictureBox
    {
      Dock = DockStyle.Fill,
      SizeMode = PictureBoxSizeMode.Zoom
    };
    Controls.Add(_pictureBox);

    // Load and display image
    try
    {
      Bitmap bmp = LoadProprietaryImage("image.dat");
      _pictureBox.Image = bmp;
    }
    catch (FormatException ex)
    {
      _ = MessageBox.Show($"Error loading image: {ex.Message}");
    }
  }

  public new void Dispose ()
  {
    base.Dispose();
    _pictureBox.Dispose();
  }

  /// <summary>
  /// Reads a proprietary format and converts it to a Bitmap.
  /// Replace the parsing logic with your actual format reader.
  /// </summary>
  private static Bitmap LoadProprietaryImage (string filePath)
  {
    // Simulated proprietary format:
    // First 4 bytes: width (int)
    // Next 4 bytes: height (int)
    // Remaining: raw RGB data (width * height * 3 bytes)

    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

    if (fileBytes.Length < 8)
      throw new FormatException("Invalid file format.");

    int width = BitConverter.ToInt32(fileBytes, 0);
    int height = BitConverter.ToInt32(fileBytes, 4);

    int expectedSize = width * height * 3;
    if (fileBytes.Length - 8 != expectedSize)
      throw new FormatException("Corrupt or incomplete image data.");

    // Create a Bitmap
    Bitmap bmp = new(width, height, PixelFormat.Format24bppRgb);

    // Lock bitmap for direct memory access
    BitmapData bmpData = bmp.LockBits(
        new Rectangle(0, 0, width, height),
        ImageLockMode.WriteOnly,
        bmp.PixelFormat);

    // Copy RGB data into bitmap
    IntPtr ptr = bmpData.Scan0;
    byte[] rgbData = new byte[expectedSize];

    // Proprietary format stores RGB in sequence
    Buffer.BlockCopy(fileBytes, 8, rgbData, 0, expectedSize);

    // Copy to bitmap memory
    Marshal.Copy(rgbData, 0, ptr, rgbData.Length);

    bmp.UnlockBits(bmpData);

    return bmp;
  }

  [STAThread]
  public static void Main ()
  {
    Application.EnableVisualStyles();
    Form m = new MainForm();
    Application.Run(m);
  }
}
