using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Atlantis.Framework.TH.SSO.Tests
{
  public class TimerTestContext : TestContext
  {
    private TestContext _contextInstance;

    private Dictionary<string, Stopwatch> _stopwatches;

    public TimerTestContext(TestContext context)
    {
      _contextInstance = context;
    }

    public override System.Data.Common.DbConnection DataConnection
    {
      get
      {
        return _contextInstance.DataConnection;
      }
    }

    public override System.Data.DataRow DataRow
    {
      get
      {
        return _contextInstance.DataRow;
      }
    }

    public override System.Collections.IDictionary Properties
    {
      get
      {
        return _contextInstance.Properties;
      }
    }

    public Dictionary<string, Stopwatch> Stopwatches
    {
      get
      {
        return _stopwatches ?? (_stopwatches = new Dictionary<string, Stopwatch>());
      }
    }

    public override void AddResultFile(string fileName)
    {
      _contextInstance.AddResultFile(fileName);
    }

    public override void BeginTimer(string timerName)
    {
      WriteLine("Timer Name: {0}", timerName);
      if (!Stopwatches.ContainsKey(timerName))
        Stopwatches.Add(timerName, new Stopwatch());
      _stopwatches[timerName].Start();
      WriteLine("Start Time: {0}", DateTime.Now);
    }

    public override void EndTimer(string timerName)
    {
      if (!Stopwatches.ContainsKey(timerName))
        throw new InvalidOperationException();
      Stopwatches[timerName].Stop();
      WriteLine("End Time: {0}", DateTime.Now);
      WriteLine("Timer Duration: {0}\n", Stopwatches[timerName].Elapsed);
    }

    public override void WriteLine(string format, params object[] args)
    {
      _contextInstance.WriteLine(format, args);
    }

  }
}
