using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LoadTest;

namespace Atlantis.Framework.DataCacheLoadTest
{
  public partial class DataCacheLoadTestForm : Form
  {

    int m_iNumSteps ;
    private LoadTester m_oLoadTester;
    private Random m_oRandom;

    public DataCacheLoadTestForm()
    {
      InitializeComponent();

      m_oRandom = new Random(123456);
      m_oLoadTester = new LoadTester();
      m_iNumSteps = 0;
    }

    int iCount = 0;
    int pfid = 100;

    private void WriterBoundHandler(object oState)
    {
      DataCache.DataCache.GetListPrice(1, pfid++, 0);
      //DataCache.RetrieveMiniCartInfo(iCount++, 1);
    }

    private void ReaderBoundHandler(object oState)
    {
      DataCache.DataCache.GetListPrice(1, m_oRandom.Next(100, pfid), 0);
      //DataCache.RetrieveMiniCartInfo(m_oRandom.Next(0, iCount), 1);
    }

    private void btnStart_Click_1(object sender, EventArgs e)
    {
      pbrTest.Value = m_iNumSteps = 0;
      pbrTest.Step = 1;
      pbrTest.Minimum = 0;
      pbrTest.Maximum = (int)nudTime.Value;

      for (int i = 0; i < nudReaderThreads.Value; ++i)
        m_oLoadTester.Add(new LoadTestWorker.LoadTestHandler(ReaderBoundHandler), null);

      for (int i = 0; i < nudWriterThreads.Value; ++i)
        m_oLoadTester.Add(new LoadTestWorker.LoadTestHandler(WriterBoundHandler), null);

      btnStart.Enabled = nudTime.Enabled = nudReaderThreads.Enabled = nudWriterThreads.Enabled = false;

      m_oLoadTester.Start();
      tmrTest.Start();
    }

    private void btnStop_Click_1(object sender, EventArgs e)
    {
      tmrTest.Stop();
      m_oLoadTester.Stop();

      StringBuilder sbResult = new StringBuilder();
      sbResult.AppendFormat("{0} threads.\r\n", m_oLoadTester.iNumWorkers);
      sbResult.AppendFormat("{0} calls in {1} seconds.\r\n", m_oLoadTester.lNumRuns, m_iNumSteps);
      sbResult.AppendFormat("{0} average call time (seconds).\r\n", m_oLoadTester.dAverageTime);
      sbResult.Append(DataCache.DataCache.GetStats());
      tbxStats.Text = sbResult.ToString();

      m_oLoadTester.Clear();
      btnStart.Enabled = nudTime.Enabled = nudReaderThreads.Enabled = nudWriterThreads.Enabled = true;
    }

    private void btnGetStats_Click_1(object sender, EventArgs e)
    {
      StringBuilder sbResult = new StringBuilder();
      sbResult.AppendFormat("{0} threads.\r\n", m_oLoadTester.iNumWorkers);
      sbResult.AppendFormat("{0} calls in {1} seconds.\r\n", m_oLoadTester.lNumRuns, m_iNumSteps);
      sbResult.AppendFormat("{0} average call time (seconds).\r\n", m_oLoadTester.dAverageTime);
      sbResult.Append(DataCache.DataCache.GetStats());
      sbResult.Append(DataCache.DataCache.DisplayInProcessCachedData("RetrieveMiniCartInfo"));
      tbxStats.Text = sbResult.ToString();
    }

    private void btnClearCache_Click_1(object sender, EventArgs e)
    {

    }

    private void tmrTest_Tick_1(object sender, EventArgs e)
    {
      if (m_iNumSteps < nudTime.Value)
      {
        pbrTest.PerformStep();
        ++m_iNumSteps;
      }
      else
      {
        tmrTest.Stop();
        m_oLoadTester.Stop();

        StringBuilder sbResult = new StringBuilder();
        sbResult.AppendFormat("{0} thread(s).\r\n", m_oLoadTester.iNumWorkers);
        sbResult.AppendFormat("{0} calls in {1} seconds.\r\n", m_oLoadTester.lNumRuns, nudTime.Value);
        sbResult.AppendFormat("{0} average call time (seconds).\r\n", m_oLoadTester.dAverageTime);
        sbResult.Append(DataCache.DataCache.GetStats());
        tbxStats.Text = sbResult.ToString();

        m_oLoadTester.Clear();
        btnStart.Enabled = nudTime.Enabled = nudReaderThreads.Enabled = nudWriterThreads.Enabled = true;
      }
    }
  }
}
