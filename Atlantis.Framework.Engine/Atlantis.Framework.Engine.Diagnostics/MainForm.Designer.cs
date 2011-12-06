namespace Atlantis.Framework.Engine.Diagnostics
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.cmdFirewallTest = new System.Windows.Forms.Button();
      this.gridResults = new System.Windows.Forms.DataGridView();
      this.cmdCopyResults = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
      this.SuspendLayout();
      // 
      // cmdFirewallTest
      // 
      this.cmdFirewallTest.Location = new System.Drawing.Point(13, 13);
      this.cmdFirewallTest.Name = "cmdFirewallTest";
      this.cmdFirewallTest.Size = new System.Drawing.Size(154, 23);
      this.cmdFirewallTest.TabIndex = 0;
      this.cmdFirewallTest.Text = "Run Firewall Tester";
      this.cmdFirewallTest.UseVisualStyleBackColor = true;
      this.cmdFirewallTest.Click += new System.EventHandler(this.cmdFirewallTest_Click);
      // 
      // gridResults
      // 
      this.gridResults.AllowUserToAddRows = false;
      this.gridResults.AllowUserToDeleteRows = false;
      this.gridResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridResults.Location = new System.Drawing.Point(13, 43);
      this.gridResults.Name = "gridResults";
      this.gridResults.ReadOnly = true;
      this.gridResults.Size = new System.Drawing.Size(911, 490);
      this.gridResults.TabIndex = 1;
      // 
      // cmdCopyResults
      // 
      this.cmdCopyResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdCopyResults.Location = new System.Drawing.Point(770, 12);
      this.cmdCopyResults.Name = "cmdCopyResults";
      this.cmdCopyResults.Size = new System.Drawing.Size(154, 23);
      this.cmdCopyResults.TabIndex = 2;
      this.cmdCopyResults.Text = "Copy Results";
      this.cmdCopyResults.UseVisualStyleBackColor = true;
      this.cmdCopyResults.Click += new System.EventHandler(this.cmdCopyResults_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(936, 545);
      this.Controls.Add(this.cmdCopyResults);
      this.Controls.Add(this.gridResults);
      this.Controls.Add(this.cmdFirewallTest);
      this.Name = "MainForm";
      this.Text = "Framework Diagnostics";
      ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cmdFirewallTest;
    private System.Windows.Forms.DataGridView gridResults;
    private System.Windows.Forms.Button cmdCopyResults;
  }
}

