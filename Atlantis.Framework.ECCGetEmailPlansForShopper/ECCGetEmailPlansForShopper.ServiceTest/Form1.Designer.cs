namespace ServiceTester
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
      this.btnTest = new System.Windows.Forms.Button();
      this.txtResults = new System.Windows.Forms.RichTextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txtShopper = new System.Windows.Forms.TextBox();
      this.btnSerialize = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // btnTest
      // 
      this.btnTest.Location = new System.Drawing.Point(248, 254);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new System.Drawing.Size(75, 23);
      this.btnTest.TabIndex = 0;
      this.btnTest.Text = "Run Test";
      this.btnTest.UseVisualStyleBackColor = true;
      this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
      // 
      // txtResults
      // 
      this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtResults.Location = new System.Drawing.Point(12, 60);
      this.txtResults.Name = "txtResults";
      this.txtResults.ReadOnly = true;
      this.txtResults.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
      this.txtResults.Size = new System.Drawing.Size(311, 178);
      this.txtResults.TabIndex = 1;
      this.txtResults.Text = "";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Shopper:";
      // 
      // txtShopper
      // 
      this.txtShopper.Location = new System.Drawing.Point(69, 25);
      this.txtShopper.Name = "txtShopper";
      this.txtShopper.Size = new System.Drawing.Size(100, 20);
      this.txtShopper.TabIndex = 3;
      this.txtShopper.Text = "858421";
      // 
      // btnSerialize
      // 
      this.btnSerialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSerialize.Location = new System.Drawing.Point(15, 254);
      this.btnSerialize.Name = "btnSerialize";
      this.btnSerialize.Size = new System.Drawing.Size(92, 23);
      this.btnSerialize.TabIndex = 4;
      this.btnSerialize.Text = "Serialize Test";
      this.btnSerialize.UseVisualStyleBackColor = true;
      this.btnSerialize.Click += new System.EventHandler(this.btnSerialize_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(339, 291);
      this.Controls.Add(this.btnSerialize);
      this.Controls.Add(this.txtShopper);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txtResults);
      this.Controls.Add(this.btnTest);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.RichTextBox txtResults;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtShopper;
    private System.Windows.Forms.Button btnSerialize;
  }
}

