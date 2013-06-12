using Atlantis.Framework.Engine.Tests.MockTriplet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.Engine.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class EngineMonitorTests
  {
    [TestMethod]
    public void MonitorData()
    {
      for (int i = 0; i < 500; i++)
      {
        try
        {
          ConfigTestRequestData request = new ConfigTestRequestData();
          ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9997);
        }
        catch { }
      }

      XDocument stats = Monitor.MonitorData.GetMonitorData(Monitor.MonitorDataTypes.Stats);
      Assert.IsNotNull(stats);
    }

    [TestMethod]
    public void FirewallTest()
    {
      XDocument firewalldata = Monitor.MonitorData.GetMonitorData(Monitor.MonitorDataTypes.FirewallTest);
      Assert.IsNotNull(firewalldata);
    }

  }
}
