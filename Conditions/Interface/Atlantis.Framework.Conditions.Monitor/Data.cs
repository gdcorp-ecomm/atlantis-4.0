using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Conditions.Monitor
{
  internal class Data : IMonitor
  {
    public Data()
    {
    }

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      var result = new XDocument();

      var root = new XElement("conditionhandlers");
      root.Add(GetMachineName(), GetProcessId(), GetInterfaceVersion());
      result.Add(root);

      var fieldInfo = typeof (ConditionHandlerManager).GetField("_conditionHandlers", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
      if (fieldInfo != null)
      {
        var conditionHandlers = (Dictionary<string, IConditionHandler>)fieldInfo.GetValue(null);

        foreach (var conditionHandler in conditionHandlers)
        {
          var conditionHandlerElement = new XElement("conditionhandler");
          conditionHandlerElement.Add(new XAttribute("name", conditionHandler.Key));
          conditionHandlerElement.Add(new XAttribute("handler", conditionHandler.Value.GetType().FullName.Replace("Atlantis.Framework.", "A.F.")));
          conditionHandlerElement.Add(new XAttribute("assembly", conditionHandler.Value.GetType().Assembly.GetName().Name.Replace("Atlantis.Framework.", "A.F.")));
          conditionHandlerElement.Add(new XAttribute("description", GetHandlerAssemblyDescription(conditionHandler.Value)));
          conditionHandlerElement.Add(new XAttribute("version", GetHandlerAssemblyVersion(conditionHandler.Value)));

          var statsFieldInfo = typeof (ConditionHandlerManager).GetField("_conditionHandlersStats", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
          if (statsFieldInfo != null)
          {
            var conditionHandlersStats = (Dictionary<string, ConditionHandlerStats>)statsFieldInfo.GetValue(null);

            var stats = new CalculatedStats(conditionHandlersStats[conditionHandler.Key]);
            conditionHandlerElement.Add(
              new XAttribute("callsperminute", stats.CallsPerMinute.ToString("F2")),
              new XAttribute("succeeded", stats.Succeeded),
              new XAttribute("failed", stats.Failed),
              new XAttribute("failurerate", stats.FailureRate.ToString("0.0%")),
              new XAttribute("avgsuccessms", stats.AverageSuccessTime.TotalMilliseconds.ToString("F2")),
              new XAttribute("avgfailms", stats.AverageFailTime.TotalMilliseconds.ToString("F2")),
              new XAttribute("runminutes", stats.RunTime.TotalMinutes.ToString("F2")));
          }
          root.Add(conditionHandlerElement);
        }
      }

      return result;
    }

    private XAttribute GetProcessId()
    {
      return new XAttribute("processid", Process.GetCurrentProcess().Id);
    }

    private XAttribute GetMachineName()
    {
      return new XAttribute("machinename", Environment.MachineName);
    }

    private XAttribute GetInterfaceVersion()
    {
      var interfaceVersion = string.Empty;

      Type conditionManagerType = typeof(ConditionHandlerManager);
      object[] interfaceFileVersions = conditionManagerType.Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
      if (interfaceFileVersions.Length > 0)
      {
        var interfaceFileVersion = interfaceFileVersions[0] as AssemblyFileVersionAttribute;
        if (interfaceFileVersion != null)
        {
          interfaceVersion = interfaceFileVersion.Version;
        }
      }

      return new XAttribute("conditionsinterfaceversion", interfaceVersion);
    }

    private string GetHandlerAssemblyVersion(IConditionHandler conditionHandler)
    {
      string assemblyVersion = string.Empty;

      object[] customAttributes = conditionHandler.GetType().Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true);
      if ((customAttributes != null) && (customAttributes.Length > 0))
      {
        var attribute = customAttributes[0] as AssemblyFileVersionAttribute;
        if (attribute != null)
        {
          assemblyVersion = attribute.Version;
        }
      }

      return assemblyVersion;
    }

    private string GetHandlerAssemblyDescription(IConditionHandler conditionHandler)
    {
      string assemblyDescription = string.Empty;

      object[] customAttributes = conditionHandler.GetType().Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true);
      if ((customAttributes != null) && (customAttributes.Length > 0))
      {
        var attribute = customAttributes[0] as AssemblyDescriptionAttribute;
        if (attribute != null)
        {
          assemblyDescription = attribute.Description;
        }
      }

      return assemblyDescription;
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
