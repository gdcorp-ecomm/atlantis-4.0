using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine.Monitor
{
  internal class Stats : IMonitor
  {
    public XDocument GetMonitorData()
    {
      XDocument result = new XDocument();
      XElement root = new XElement("ConfigElements");

      root.Add(GetProcessId(), GetMachineName(), GetEngineVersion(), GetInterfaceVersion());

      try
      {
        var configElements = Engine.GetConfigElements();
        foreach (ConfigElement configItem in configElements)
        {
          XElement configItemData = GetStatsElement(configItem);
          root.Add(configItemData);
        }
      }
      catch (Exception ex)
      {
        root.Add(new XElement("error", ex.Message));
      }

      result.Add(root);
      return result;
    }

    private XAttribute GetProcessId()
    {
      XAttribute result = null;

      try
      {
        result = new XAttribute("processid", Process.GetCurrentProcess().Id);
      }
      catch { }

      return result;
    }

    private XAttribute GetEngineVersion()
    {
      return new XAttribute("engineversion", Engine.EngineVersion);
    }

    private XAttribute GetInterfaceVersion()
    {
      return new XAttribute("interfaceversion", Engine.InterfaceVersion);
    }

    private XAttribute GetMachineName()
    {
      return new XAttribute("machinename", Environment.MachineName);
    }

    private XElement GetStatsElement(ConfigElement configItem)
    {
      XElement result = new XElement("ConfigElement");

      result.Add(
        new XAttribute("requesttype", configItem.RequestType),
        new XAttribute("requesthandler", configItem.ProgID.Replace("Atlantis.Framework.", "A.F.")),
        new XAttribute("assembly", Path.GetFileName(configItem.Assembly).Replace("Atlantis.Framework.", "A.F.")),
        new XAttribute("assemblydescription", configItem.AssemblyDescription),
        new XAttribute("assemblyfileversion", configItem.AssemblyFileVersion));
      
      CalculatedStats stats = new CalculatedStats(configItem.Stats);

      result.Add(
        new XAttribute("callsperminute", stats.CallsPerMinute.ToString("F2")),
        new XAttribute("succeeded", stats.Succeeded),
        new XAttribute("failed", stats.Failed),
        new XAttribute("failurerate", stats.FailureRate.ToString("0.0%")),
        new XAttribute("avgsuccessms", stats.AverageSuccessTime.TotalMilliseconds.ToString("F2")),
        new XAttribute("avgfailms", stats.AverageFailTime.TotalMilliseconds.ToString("F2")),
        new XAttribute("runminutes", stats.RunTime.TotalMinutes.ToString("F2")));

      return result;
    }

    private class CalculatedStats
    {
      public int Failed { get; private set; }
      public int Succeeded { get; private set; }
      public double CallsPerMinute { get; private set; }
      public double FailureRate { get; private set; }

      public TimeSpan RunTime { get; private set; }
      public TimeSpan AverageFailTime { get; private set; }
      public TimeSpan AverageSuccessTime { get; private set; }

      public CalculatedStats(ConfigElementStats stats)
      {
        Failed = stats.Failed;
        Succeeded = stats.Succeeded;

        AverageFailTime = stats.CalculateAverageFailTime();
        AverageSuccessTime = stats.CalculateAverageSuccessTime();

        RunTime = DateTime.Now.Subtract(stats.StartTime);
        int total = Succeeded + Failed;

        if (RunTime.TotalMinutes > 0)
        {
          CallsPerMinute = total / RunTime.TotalMinutes;
        }
        else
        {
          CallsPerMinute = 0;
        }

        if (total > 0)
        {
          FailureRate = (double)Failed / total;
        }
        else
        {
          FailureRate = 0;
        }
      }
    }
  }
}
