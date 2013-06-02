using System;
using System.Text;
using System.Windows.Forms;
using LoadTest;

namespace Atlantis.Framework.DataCacheLoadTest
{
  public partial class DataCacheLoadTestForm : Form
  {
    int _numSteps;
    private LoadTester _loadTester;
    private Random _random;
    int _productId = 100;

    public DataCacheLoadTestForm()
    {
      InitializeComponent();

      _random = new Random(123456);
      _loadTester = new LoadTester();
      _numSteps = 0;
    }


    private void WriterBoundHandler(object oState)
    {
      DataCache.DataCache.GetPFIDByUnifiedID(++_productId, 2);
    }

    private void ReaderBoundHandler(object oState)
    {
      DataCache.DataCache.GetPFIDByUnifiedID(_random.Next(100, _productId), 2);
    }

    private void btnStart_Click_1(object sender, EventArgs e)
    {
      pbrTest.Value = _numSteps = 0;
      pbrTest.Step = 1;
      pbrTest.Minimum = 0;
      pbrTest.Maximum = (int)nudTime.Value;

      for (int i = 0; i < nudReaderThreads.Value; ++i)
        _loadTester.Add(new LoadTestWorker.LoadTestHandler(ReaderBoundHandler), null);

      for (int i = 0; i < nudWriterThreads.Value; ++i)
        _loadTester.Add(new LoadTestWorker.LoadTestHandler(WriterBoundHandler), null);

      btnStart.Enabled = nudTime.Enabled = nudReaderThreads.Enabled = nudWriterThreads.Enabled = false;

      _loadTester.Start();
      tmrTest.Start();
    }

    private void btnStop_Click_1(object sender, EventArgs e)
    {
      tmrTest.Stop();
      _loadTester.Stop();

      StringBuilder sbResult = new StringBuilder();
      sbResult.AppendFormat("{0} threads.\r\n", _loadTester.iNumWorkers);
      sbResult.AppendFormat("{0} calls in {1} seconds.\r\n", _loadTester.lNumRuns, _numSteps);
      sbResult.AppendFormat("{0} average call time (seconds).\r\n", _loadTester.dAverageTime);
      tbxStats.Text = sbResult.ToString();

      _loadTester.Clear();
      btnStart.Enabled = nudTime.Enabled = nudReaderThreads.Enabled = nudWriterThreads.Enabled = true;
    }

    private void btnGetStats_Click_1(object sender, EventArgs e)
    {
      StringBuilder sbResult = new StringBuilder();
      sbResult.AppendFormat("{0} threads.\r\n", _loadTester.iNumWorkers);
      sbResult.AppendFormat("{0} calls in {1} seconds.\r\n", _loadTester.lNumRuns, _numSteps);
      sbResult.AppendFormat("{0} average call time (seconds).\r\n", _loadTester.dAverageTime);
      tbxStats.Text = sbResult.ToString();
    }

    private void btnClearCache_Click_1(object sender, EventArgs e)
    {

    }

    private void tmrTest_Tick_1(object sender, EventArgs e)
    {
      if (_numSteps < nudTime.Value)
      {
        pbrTest.PerformStep();
        ++_numSteps;
      }
      else
      {
        tmrTest.Stop();
        _loadTester.Stop();

        StringBuilder sbResult = new StringBuilder();
        sbResult.AppendFormat("{0} thread(s).\r\n", _loadTester.iNumWorkers);
        sbResult.AppendFormat("{0} calls in {1} seconds.\r\n", _loadTester.lNumRuns, nudTime.Value);
        sbResult.AppendFormat("{0} average call time (seconds).\r\n", _loadTester.dAverageTime);
        tbxStats.Text = sbResult.ToString();

        _loadTester.Clear();
        btnStart.Enabled = nudTime.Enabled = nudReaderThreads.Enabled = nudWriterThreads.Enabled = true;
      }
    }
  }
}
