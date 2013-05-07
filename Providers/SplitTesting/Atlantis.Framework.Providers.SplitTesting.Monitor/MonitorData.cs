using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.SplitTesting.Monitor
{
  public static class MonitorData
  {
    public static XDocument GetMonitorData(MonitorDataTypes dataType, NameValueCollection qsCollection)
    {
      XDocument result = null;

      string monitorClass = "Atlantis.Framework.Providers.SplitTesting.Monitor." + dataType.ToString();
      Type monitorClassType = Assembly.GetExecutingAssembly().GetType(monitorClass, false);
      if (monitorClassType != null)
      {
        Type interfaceFound = monitorClassType.GetInterface(typeof(IMonitor).Name);
        if (interfaceFound != null)
        {
          IMonitor monitor = Activator.CreateInstance(monitorClassType) as IMonitor;
          if (monitor != null)
          {
            result = monitor.GetMonitorData(qsCollection);
          }
        }
      }

      return result;
    }
  }
}
