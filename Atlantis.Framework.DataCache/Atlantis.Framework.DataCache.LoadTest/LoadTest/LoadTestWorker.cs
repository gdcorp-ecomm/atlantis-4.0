using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace LoadTest
{
  public class LoadTestWorker
  {
    public delegate void LoadTestHandler(object oState);

    [DllImport("Kernel32.dll")]
    [System.Security.SuppressUnmanagedCodeSecurity()]
    private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

    [DllImport("Kernel32.dll")]
    [System.Security.SuppressUnmanagedCodeSecurity()]
    private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    private long m_lFreq;
    private LoadTestHandler m_oHandler;
    private object m_oState;
    private Thread m_oThread;

    private int m_iRunning;
    private double m_dAverageTime;
    private double m_dSumTime;
    private long m_lNumRuns;

    public LoadTestWorker(LoadTestHandler oHandler, object oState)
    {
      m_oThread = new Thread(Run);
      m_oHandler = oHandler;
      m_oState = oState;
      QueryPerformanceFrequency(out m_lFreq);
      m_dAverageTime = 0.0;
      m_dSumTime = 0.0;
      m_lNumRuns = 0;
    }

    public void Start()
    {
      bRunning = true;
      m_oThread.Start();
    }

    public void Stop()
    {
      if (bRunning)
      {
        bRunning = false;
        m_oThread.Join();
      }
    }

    public void Reset()
    {
      Stop();
      m_dAverageTime = 0.0;
      m_dSumTime = 0.0;
      m_lNumRuns = 0;
    }

    private void Run()
    {
      long lStartTime = 0;
      long lStopTime = 0;

      while (bRunning)
      {
        QueryPerformanceCounter(out lStartTime);
        m_oHandler(m_oState);
        QueryPerformanceCounter(out lStopTime);
        double dTime = (double)(lStopTime - lStartTime) / (double)m_lFreq;

        Interlocked.Increment(ref m_lNumRuns);
        m_dSumTime += dTime;
        Interlocked.Exchange(ref m_dAverageTime, m_dSumTime / (double)m_lNumRuns);
      }
    }

    public double dAverageTime
    {
      get { return Interlocked.CompareExchange(ref m_dAverageTime, 0.0, 0.0); }
      set { Interlocked.Exchange(ref m_dAverageTime, value); }
    }

    public long lNumRuns
    {
      get { return Interlocked.CompareExchange(ref m_lNumRuns, 0, 0); }
      set { Interlocked.Exchange(ref m_lNumRuns, value); }
    }

    public bool bRunning
    {
      get { return Interlocked.CompareExchange(ref m_iRunning, 0, 0) != 0; }
      set { Interlocked.Exchange(ref m_iRunning, Convert.ToInt32(value)); }
    }
  }
}
