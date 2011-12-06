using System;
using System.Windows.Forms;
using Atlantis.Framework.Engine.Diagnostics.FirewallTest;
using System.Collections.Generic;

namespace Atlantis.Framework.Engine.Diagnostics
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void cmdFirewallTest_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;

      try
      {
        gridResults.DataSource = null;
        List<FirewallResult> results = FirewallTester.RunFirewallTest();
        gridResults.DataSource = results;
        gridResults.Refresh();
      }
      finally
      {
        Cursor = Cursors.Default;
      }
    }

    private void cmdCopyResults_Click(object sender, EventArgs e)
    {
      gridResults.SelectAll();
      Clipboard.SetDataObject(gridResults.GetClipboardContent(), true);
      gridResults.ClearSelection();
    }
  }
}
