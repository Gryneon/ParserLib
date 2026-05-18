using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Parser;
using Parser.Tokens;

using Specification.ZDoom;

namespace ParserDebuggerApp;

public partial class MainForm : Form
{
  private Spec currentSpec;

  private readonly Dictionary<string, Color> colorCache = new();

  private IToken selectedToken;

  private readonly Color selectedBackColor = Color.Black;
  private readonly Color selectedForeColor = Color.White;

  public MainForm ()
  {
    InitializeComponent();
    Init();
  }

  private void Init ()
  {
    _ = cmbSpec.Items.Add("ZScript");
    _ = cmbSpec.Items.Add("ACS");
    _ = cmbSpec.Items.Add("UDMF");
    _ = cmbSpec.SelectedIndex = 0;

    btnRun.Click += (_, __) => RunEngine();
    treeHierarchy.AfterSelect += TreeHierarchy_AfterSelect;
  }

  // ---------------- SPEC ----------------

  private Spec GetSpec () => cmbSpec.SelectedItem?.ToString() switch
  {
    "ACS" => Definition.ACS,
    "UDMF" => Definition.UDMF,
    "ZScript" => Definition.ZScript,
    _ => DefaultSpec.Unknown,
  };

  // ---------------- ENGINE ----------------

  private TokenCollection lastTokens;
  private TokenAssemblyResult lastResult;

  private void RunEngine ()
  {
    try
    {
      currentSpec = GetSpec();

      string text = rtbMain.Text;

      TokenFactory factory = new(currentSpec);
      lastTokens = factory.Produce(text);

      TokenAssembler assembler = new(currentSpec);
      lastResult = assembler.Execute(lastTokens);

      DisplayTokens(lastTokens);
      DisplayHierarchy(lastResult);
      DisplayParents(lastResult);
    }
    catch (Exception ex)
    {
      MessageBox.Show(ex.ToString());
    }
  }

  // ---------------- TOKEN HIGHLIGHT ----------------

  private void DisplayTokens (TokenCollection tokens)
  {
    rtbMain.SelectAll();
    rtbMain.SelectionBackColor = Color.White;
    rtbMain.SelectionColor = Color.Black;

    foreach (IToken token in tokens)
    {
      rtbMain.Select(token.Index, token.Content.Length);

      Color baseColor = GetColor(token.Type);

      // Foreground color per type
      rtbMain.SelectionColor = baseColor;

      // Light background tint
      rtbMain.SelectionBackColor = Color.FromArgb(40, baseColor);
    }

    rtbMain.Select(0, 0);
  }

  private Color GetColor (string type)
  {
    if (!colorCache.ContainsKey(type))
    {
      Random rnd = new(type.GetHashCode());

      colorCache[type] = Color.FromArgb(
          255,
          50 + rnd.Next(180),
          50 + rnd.Next(180),
          50 + rnd.Next(180));
    }

    return colorCache[type];
  }

  // ---------------- TREE VIEW ----------------

  private void DisplayHierarchy (TokenAssemblyResult result)
  {
    treeHierarchy.Nodes.Clear();

    foreach (IToken token in result.Hierarchy)
    {
      _ = treeHierarchy.Nodes.Add(BuildNode(token));
    }
  }

  private TreeNode BuildNode (IToken token)
  {
    string role = token.AssignTo?.ToString() ?? "-";

    TreeNode node = new($"{token.Type} ({role}) : {token.Content}")
    {
      Tag = token
    };

    foreach (IToken child in token.Children)
    {
      _ = node.Nodes.Add(BuildNode(child));
    }

    return node;
  }

  private void TreeHierarchy_AfterSelect (object sender, TreeViewEventArgs e)
  {
    if (e.Node.Tag is not IToken token)
      return;

    selectedToken = token;

    RefreshHighlighting();
    HighlightRecursive(token);

    rtbMain.Select(token.Index, token.Content.Length);
    rtbMain.ScrollToCaret();
  }

  // ---------------- FOCUSED HIGHLIGHT ----------------

  private void HighlightRecursive (IToken token)
  {
    rtbMain.Select(token.Index, token.Content.Length);
    rtbMain.SelectionBackColor = selectedBackColor;
    rtbMain.SelectionColor = selectedForeColor;

    foreach (IToken child in token.Children)
    {
      HighlightRecursive(child);
    }
  }

  private void RefreshHighlighting ()
  {
    if (lastTokens == null)
      return;

    DisplayTokens(lastTokens);
  }

  // ---------------- PARENTS ----------------

  private void DisplayParents (TokenAssemblyResult result)
  {
    StringBuilder sb = new();

    foreach (IToken p in result.Parents)
    {
      _ = sb.AppendLine(p.Type);
    }

    rtbParents.Text = sb.ToString();
  }
}
