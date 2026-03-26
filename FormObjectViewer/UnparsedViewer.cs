using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Common;

namespace FormObjectViewer;

internal sealed partial class UnparsedViewer : Form
{
  private SectionCollection Inverse => Sections.Inverse();

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [AllowNull]
  public SectionCollection Sections { get; internal set; }

  public UnparsedViewer () => InitializeComponent();

  private void UnparsedViewer_Load (object sender, EventArgs e)
  {
    if (Sections is null || Sections.Count == 0)
    {
      VisualRichBox.Text = "No sections to preview.";
      return;
    }
    SectionCollection inverse = Inverse;

    int count = Sections.Count;
    int inverse_count = inverse.Count;
    int inverse_chars = inverse.Select<Pos, int>(p => p.Length).Sum();

    SectionCountTextBox.Text = $"{count}";
    InverseCountTextBox.Text = $"{inverse_count}";
    InverseCharsTextBox.Text = $"{inverse_chars}";

    VisualRichBox.Text = Sections.FullText;

    Highlight(Inverse, Color.Red, Color.LightPink);
  }

  private void ParsedButton_Click (object sender, EventArgs e)
  {
    VisualRichBox.Text = Sections.FullText;

    Highlight(Sections, Color.Blue, Color.LightBlue);
  }

  private void UnparsedButton_Click (object sender, EventArgs e)
  {
    VisualRichBox.Text = Sections.FullText;

    Highlight(Inverse, Color.Red, Color.LightPink);
  }

  private void Highlight (SectionCollection sections, Color text, Color back)
  {
    VisualRichBox.SuspendLayout();
    VisualRichBox.HideSelection = true;
    VisualRichBox.WordWrap = false;
    VisualRichBox.DetectUrls = false;
    VisualRichBox.ReadOnly = false;
    try
    {
      foreach (Pos section in sections)
      {
        VisualRichBox.Select(section.Start, section.Length);
        VisualRichBox.SelectionColor = text;
        VisualRichBox.SelectionBackColor = back;
      }
      VisualRichBox.Select(0, 0);
      VisualRichBox.SelectionColor = VisualRichBox.ForeColor;
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
