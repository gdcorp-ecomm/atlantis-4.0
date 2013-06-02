using System.Collections.Generic;

namespace LoadTest
{
  public class LoadTester
  {
    private List<LoadTestWorker> m_lstWorkers = new List<LoadTestWorker>();

    public void Start()
    {
      foreach (LoadTestWorker oWorker in m_lstWorkers)
        oWorker.Start();
    }

    public void Stop()
    {
      foreach (LoadTestWorker oWorker in m_lstWorkers)
        oWorker.Stop();
    }

    public void Reset()
    {
      foreach (LoadTestWorker oWorker in m_lstWorkers)
        oWorker.Reset();
    }

    public void Clear()
    {
      Stop();
      m_lstWorkers.Clear();
    }

    public void Add(LoadTestWorker.LoadTestHandler oHandler, object oState)
    {
      LoadTestWorker oWorker = new LoadTestWorker(oHandler, oState);
      m_lstWorkers.Add(oWorker);
    }

    public void RemoveAt(int index)
    {
      if (index >= 0 && index < m_lstWorkers.Count)
      {
        LoadTestWorker oWorker = m_lstWorkers[index];
        oWorker.Stop();
        m_lstWorkers.RemoveAt(index);
      }
    }

    public int iNumWorkers
    {
      get { return m_lstWorkers.Count; }
    }

    public LoadTestWorker GetWorker(int index)
    {
      return m_lstWorkers[index];
    }

    public double dAverageTime
    {
      get
      {
        double dSumTime = 0.0;
        foreach (LoadTestWorker oWorker in m_lstWorkers)
          dSumTime += oWorker.dAverageTime;
        return dSumTime / (double)m_lstWorkers.Count;
      }
    }

    public long lNumRuns
    {
      get
      {
        long lNumRuns = 0;
        foreach (LoadTestWorker oWorker in m_lstWorkers)
          lNumRuns += oWorker.lNumRuns;
        return lNumRuns;
      }
    }
  }
}
