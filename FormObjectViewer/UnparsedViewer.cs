using System.Collections.ObjectModel;
using System.Linq;

using Common;

namespace FormObjectViewer;

internal sealed partial class UnparsedViewer : Form
{
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public SectionCollection Sections { get; internal set; } = [];

  public UnparsedViewer ()
  {
    InitializeComponent();
  }

  private void UnparsedViewer_Load (object sender, EventArgs e)
  {
    SectionCollection inverse = Sections.Inverse();

    if (Sections.Count == 0)
    {
      VisualRichBox.Text = "No sections to preview.";
    }
    int count = Sections.Count;
    int inverse_count = inverse.Count;
    int inverse_chars = inverse.Sum(s => s.Length);

    SectionCountTextBox.Text = $"{count}";
    InverseCountTextBox.Text = $"{inverse_count}";
    InverseCharsTextBox.Text = $"{inverse_chars}";

    VisualRichBox.Text = Sections.FullText;

    Collection<bool> colorList = inverse.GetGetParsedFromSections();

    Color falseColor = Color.Red;
    Color trueColor = VisualRichBox.ForeColor;

    VisualRichBox.SuspendLayout();
    VisualRichBox.HideSelection = true;
    VisualRichBox.WordWrap = false;
    VisualRichBox.DetectUrls = false;
    VisualRichBox.ReadOnly = false;
    try
    {
      foreach (Section section in inverse)
      {
        VisualRichBox.Select(section.Start, section.Length);
        VisualRichBox.SelectionColor = falseColor;
      }
      VisualRichBox.Select(0, 0);
      VisualRichBox.SelectionColor = trueColor;
    }
    finally
    {
      VisualRichBox.HideSelection = false;
      VisualRichBox.ReadOnly = true;
      VisualRichBox.ResumeLayout();
      VisualRichBox.Invalidate();
    }
  }
}
