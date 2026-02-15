#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1812 // Unused class
using Common.Extensions;

namespace FormObjectViewer;

internal sealed partial class ArrayForm : Form
{
  [ListBindable(BindableSupport.Yes)]
  [DSVA(DesignerSerializationVisibility.Hidden)]
  public BindingList<object> Array { get; set; } = [];

  public ArrayForm ()
  {
    InitializeComponent();
    Repopulate();
  }
  public ArrayForm (IEnumerable<object> list)
  {
    Array.AddRange(list);
    InitializeComponent();
    Repopulate();
  }

  internal void Repopulate ()
  {
    ArrayListBox.Items.Clear();
    foreach (object item in Array)
    {
      _ = ArrayListBox.Items.Add(item);
    }
    ArrayListBox.Refresh();
  }

  private void UpdateButton_Click (object sender, EventArgs e)
  {
    Repopulate();
  }

  private void ArrayListBox_MouseDoubleClick (object sender, MouseEventArgs e)
  {
    //TODO: Open said item in its own form.
  }
}
