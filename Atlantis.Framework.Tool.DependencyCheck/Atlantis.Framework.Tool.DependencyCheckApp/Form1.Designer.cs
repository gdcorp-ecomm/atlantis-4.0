namespace Atlantis.Framework.Tool.DependencyCheckApp
{
  partial class Form1
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
      this.dgResults = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
      this.SuspendLayout();
      // 
      // dgResults
      // 
      this.dgResults.AllowUserToAddRows = false;
      this.dgResults.AllowUserToDeleteRows = false;
      this.dgResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgResults.Location = new System.Drawing.Point(13, 13);
      this.dgResults.Name = "dgResults";
      this.dgResults.ReadOnly = true;
      this.dgResults.Size = new System.Drawing.Size(900, 675);
      this.dgResults.TabIndex = 0;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(925, 700);
      this.Controls.Add(this.dgResults);
      this.Name = "Form1";
      this.Text = "Dependency Check";
      ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dgResults;
  }
}

