namespace Atlantis.Framework.DataCacheLoadTest
{
  partial class DataCacheLoadTestForm
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
      this.components = new System.ComponentModel.Container();
      this.btnClearCache = new System.Windows.Forms.Button();
      this.btnGetStats = new System.Windows.Forms.Button();
      this.btnStop = new System.Windows.Forms.Button();
      this.pbrTest = new System.Windows.Forms.ProgressBar();
      this.lblWriter = new System.Windows.Forms.Label();
      this.lblReader = new System.Windows.Forms.Label();
      this.lblTime = new System.Windows.Forms.Label();
      this.nudWriterThreads = new System.Windows.Forms.NumericUpDown();
      this.nudReaderThreads = new System.Windows.Forms.NumericUpDown();
      this.nudTime = new System.Windows.Forms.NumericUpDown();
      this.tbxStats = new System.Windows.Forms.TextBox();
      this.btnStart = new System.Windows.Forms.Button();
      this.tmrTest = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.nudWriterThreads)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudReaderThreads)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudTime)).BeginInit();
      this.SuspendLayout();
      // 
      // btnClearCache
      // 
      this.btnClearCache.Location = new System.Drawing.Point(105, 60);
      this.btnClearCache.Name = "btnClearCache";
      this.btnClearCache.Size = new System.Drawing.Size(86, 41);
      this.btnClearCache.TabIndex = 23;
      this.btnClearCache.Text = "&Clear Cache";
      this.btnClearCache.UseVisualStyleBackColor = true;
      this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click_1);
      // 
      // btnGetStats
      // 
      this.btnGetStats.Location = new System.Drawing.Point(12, 60);
      this.btnGetStats.Name = "btnGetStats";
      this.btnGetStats.Size = new System.Drawing.Size(86, 41);
      this.btnGetStats.TabIndex = 22;
      this.btnGetStats.Text = "&Get Stats";
      this.btnGetStats.UseVisualStyleBackColor = true;
      this.btnGetStats.Click += new System.EventHandler(this.btnGetStats_Click_1);
      // 
      // btnStop
      // 
      this.btnStop.Location = new System.Drawing.Point(104, 13);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new System.Drawing.Size(85, 41);
      this.btnStop.TabIndex = 21;
      this.btnStop.Text = "S&top";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click_1);
      // 
      // pbrTest
      // 
      this.pbrTest.Location = new System.Drawing.Point(12, 104);
      this.pbrTest.Name = "pbrTest";
      this.pbrTest.Size = new System.Drawing.Size(431, 22);
      this.pbrTest.Step = 1;
      this.pbrTest.TabIndex = 20;
      // 
      // lblWriter
      // 
      this.lblWriter.AutoSize = true;
      this.lblWriter.Location = new System.Drawing.Point(197, 62);
      this.lblWriter.Name = "lblWriter";
      this.lblWriter.Size = new System.Drawing.Size(80, 13);
      this.lblWriter.TabIndex = 19;
      this.lblWriter.Text = "Writer Threads:";
      // 
      // lblReader
      // 
      this.lblReader.AutoSize = true;
      this.lblReader.Location = new System.Drawing.Point(190, 36);
      this.lblReader.Name = "lblReader";
      this.lblReader.Size = new System.Drawing.Size(87, 13);
      this.lblReader.TabIndex = 18;
      this.lblReader.Text = "Reader Threads:";
      // 
      // lblTime
      // 
      this.lblTime.AutoSize = true;
      this.lblTime.Location = new System.Drawing.Point(195, 10);
      this.lblTime.Name = "lblTime";
      this.lblTime.Size = new System.Drawing.Size(82, 13);
      this.lblTime.TabIndex = 17;
      this.lblTime.Text = "Time (seconds):";
      // 
      // nudWriterThreads
      // 
      this.nudWriterThreads.Location = new System.Drawing.Point(298, 60);
      this.nudWriterThreads.Name = "nudWriterThreads";
      this.nudWriterThreads.Size = new System.Drawing.Size(145, 20);
      this.nudWriterThreads.TabIndex = 16;
      this.nudWriterThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // nudReaderThreads
      // 
      this.nudReaderThreads.Location = new System.Drawing.Point(298, 34);
      this.nudReaderThreads.Name = "nudReaderThreads";
      this.nudReaderThreads.Size = new System.Drawing.Size(145, 20);
      this.nudReaderThreads.TabIndex = 15;
      this.nudReaderThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // nudTime
      // 
      this.nudTime.Location = new System.Drawing.Point(298, 8);
      this.nudTime.Maximum = new decimal(new int[] {
            7200,
            0,
            0,
            0});
      this.nudTime.Name = "nudTime";
      this.nudTime.Size = new System.Drawing.Size(145, 20);
      this.nudTime.TabIndex = 14;
      this.nudTime.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
      // 
      // tbxStats
      // 
      this.tbxStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbxStats.Location = new System.Drawing.Point(12, 132);
      this.tbxStats.Multiline = true;
      this.tbxStats.Name = "tbxStats";
      this.tbxStats.Size = new System.Drawing.Size(431, 155);
      this.tbxStats.TabIndex = 13;
      // 
      // btnStart
      // 
      this.btnStart.Location = new System.Drawing.Point(12, 13);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new System.Drawing.Size(86, 41);
      this.btnStart.TabIndex = 12;
      this.btnStart.Text = "&Start";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new System.EventHandler(this.btnStart_Click_1);
      // 
      // tmrTest
      // 
      this.tmrTest.Tick += new System.EventHandler(this.tmrTest_Tick_1);
      // 
      // DataCacheLoadTestForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(456, 299);
      this.Controls.Add(this.btnClearCache);
      this.Controls.Add(this.btnGetStats);
      this.Controls.Add(this.btnStop);
      this.Controls.Add(this.pbrTest);
      this.Controls.Add(this.lblWriter);
      this.Controls.Add(this.lblReader);
      this.Controls.Add(this.lblTime);
      this.Controls.Add(this.nudWriterThreads);
      this.Controls.Add(this.nudReaderThreads);
      this.Controls.Add(this.nudTime);
      this.Controls.Add(this.tbxStats);
      this.Controls.Add(this.btnStart);
      this.Name = "DataCacheLoadTestForm";
      this.Text = "DataCacheLoadTestForm";
      ((System.ComponentModel.ISupportInitialize)(this.nudWriterThreads)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudReaderThreads)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudTime)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnClearCache;
    private System.Windows.Forms.Button btnGetStats;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.ProgressBar pbrTest;
    private System.Windows.Forms.Label lblWriter;
    private System.Windows.Forms.Label lblReader;
    private System.Windows.Forms.Label lblTime;
    private System.Windows.Forms.NumericUpDown nudWriterThreads;
    private System.Windows.Forms.NumericUpDown nudReaderThreads;
    private System.Windows.Forms.NumericUpDown nudTime;
    private System.Windows.Forms.TextBox tbxStats;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Timer tmrTest;
  }
}