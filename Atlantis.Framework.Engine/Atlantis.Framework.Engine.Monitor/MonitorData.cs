using System;
using System.Reflection;
using System.Xml.Linq;

namespace Atlantis.Framework.Engine.Monitor
{
  public static class MonitorData
  {
    public static XDocument GetMonitorData(MonitorDataTypes dataType)
    {
      XDocument result = null;

      string monitorClass = "Atlantis.Framework.Engine.Monitor." + dataType.ToString();
      Type monitorClassType = Assembly.GetExecutingAssembly().GetType(monitorClass, false);
      if (monitorClassType != null)
      {
        Type interfaceFound = monitorClassType.GetInterface(typeof(IMonitor).Name);
        if (interfaceFound != null)
        {
          IMonitor monitor = Activator.CreateInstance(monitorClassType) as IMonitor;
          if (monitor != null)
          {
            result = monitor.GetMonitorData();
          }
        }
      }

      return result;
    }
  }
}
