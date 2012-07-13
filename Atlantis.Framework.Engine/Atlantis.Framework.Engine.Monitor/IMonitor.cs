using System.Xml.Linq;

namespace Atlantis.Framework.Engine.Monitor
{
  internal interface IMonitor
  {
    XDocument GetMonitorData();
  }
}
