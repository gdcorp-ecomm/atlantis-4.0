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
      root.Add(GetProcessId(), GetMachineName(), GetInterfaceVersion());
      result.Add(root);

      var fieldInfo = typeof (ConditionHandlerManager).GetField("_conditionHandlers", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
      if (fieldInfo != null)
      {
        var conditionHandlers = (Dictionary<string, IConditionHandler>)fieldInfo.GetValue(null);

        foreach (var conditionHandler in conditionHandlers)
        {
          var conditionHandlerElement = new XElement("conditionhandler");
          conditionHandlerElement.Add(new XAttribute("conditionname", conditionHandler.Key));
          conditionHandlerElement.Add(new XAttribute("classtype", conditionHandler.Value.GetType().FullName));
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
  }
}
