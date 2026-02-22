#pragma warning disable CA1303 // Do not pass literals as localized parameters

using System.Runtime.Versioning;

namespace Parser.Forms;

public sealed class DataDictionaryEventArgs (string? name, object? value) : EventArgs
{
  public string? Name { get; init; } = name;
  public object? Value { get; init; } = value;
  public bool IsNull => Name is null && Value is null;
}

/// <summary>
/// A read-only WinForms control that displays a DataDictionary's keys and values in two columns.
/// The value column shows the result of <see cref="object.ToString"/>.
/// Designed for internal tooling with optional sorting, row selection, and export functionality.
/// </summary>
[SupportedOSPlatform("windows")]
[DefaultProperty(nameof(DataDictionary))]
public sealed class ReadOnlyDictionaryGrid : UserControl
{
  private readonly DataGridView _dataGridView;

  /// <summary>Initializes a new instance of the <see cref="ReadOnlyDictionaryGrid"/> class.</summary>
  public ReadOnlyDictionaryGrid ()
  {
    _dataGridView = new DataGridView
    {
      Dock = DockStyle.Fill,
      ReadOnly = true,
      AllowUserToAddRows = false,
      AllowUserToDeleteRows = false,
      AllowUserToResizeRows = false,
      AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
      SelectionMode = DataGridViewSelectionMode.FullRowSelect,
      MultiSelect = false,
      AlternatingRowsDefaultCellStyle = { BackColor = System.Drawing.Color.LightGray }
    };

    // Define columns
    _ = _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
    {
      HeaderText = "Key",
      Name = "KeyColumn"
    });

    _ = _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
    {
      HeaderText = "Value",
      Name = "ValueColumn"
    });

    Controls.Add(_dataGridView);
  }

  /// <summary>Gets or sets the dictionary to display in the grid.</summary>
  [Category("Data")]
  [Description("The DataDictionary whose keys and values will be displayed in the grid.")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IDictionary<string, object>? DataDictionary
  {
    get;
    private set
    {
      field = value;
      UpdateGrid();
    }
  }

  /// <summary>Refreshes the grid with the current dictionary contents.</summary>
  private void UpdateGrid ()
  {
    _dataGridView.Rows.Clear();

    if (DataDictionary != null)
    {
      foreach (KeyValuePair<string, object> kvp in DataDictionary)
      {
        _ = _dataGridView.Rows.Add(kvp.Key, kvp.Value?.ToString() ?? string.Empty);
      }
    }
  }

  /// <summary>Sorts the grid by Key or Value.</summary>
  /// <param name="sortByKey">True to sort by Key, false to sort by Value.</param>
  public void Sort (bool sortByKey = true)
  {
    if (DataDictionary == null) return;

    IOrderedEnumerable<KeyValuePair<string, object>> sorted = sortByKey
        ? DataDictionary.OrderBy(kvp => kvp.Key)
        : DataDictionary.OrderBy(kvp => kvp.Value?.ToString());

    _dataGridView.Rows.Clear();
    foreach (KeyValuePair<string, object> kvp in sorted)
    {
      _ = _dataGridView.Rows.Add(kvp.Key, kvp.Value?.ToString() ?? string.Empty);
    }
  }

  /// <summary>Exports the current grid contents to a CSV file.</summary>
  /// <param name="filePath">The file path to save the CSV.</param>
  public void ExportToCsv (string filePath)
  {
    if (DataDictionary == null || !DataDictionary.Any()) return;

    using StreamWriter writer = new(filePath);
    writer.WriteLine("Key,Value");
    foreach (KeyValuePair<string, object> kvp in DataDictionary)
    {
      writer.WriteLine($"{kvp.Key},{kvp.Value?.ToString() ?? string.Empty}");
    }
  }

  /// <summary>Event triggered when a row is selected.</summary>
  public event EventHandler<DataDictionaryEventArgs>? RowSelected;

  /// <inheritdoc />
  protected override void OnLoad (EventArgs e)
  {
    base.OnLoad(e);
    _dataGridView.SelectionChanged += (s, args) =>
    {
      if (_dataGridView.SelectedRows.Count > 0)
      {
        DataGridViewRow row = _dataGridView.SelectedRows[0];
        string key = row.Cells[0].Value?.ToString() ?? string.Empty;
        object? value = DataDictionary != null && DataDictionary.TryGetValue(key, out object? value1)
            ? value1 : null;
        RowSelected?.Invoke(this, new DataDictionaryEventArgs(key, value));
      }
    };
  }

  /// <summary>Disposes resources used by this control.</summary>
  /// <param name="disposing">True if managed resources should be disposed.</param>
  protected override void Dispose (bool disposing)
  {
    if (disposing)
    {
      _dataGridView?.Dispose();
    }
    base.Dispose(disposing);
  }

  public void SetDictionary (DataDictionary? data_dictionary)
  {
    DataDictionary = data_dictionary;
  }
}
