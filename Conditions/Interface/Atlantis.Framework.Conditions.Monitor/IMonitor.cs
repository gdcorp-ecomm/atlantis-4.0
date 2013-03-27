using System.Collections.Specialized;
using System.Xml.Linq;

namespace Atlantis.Framework.Conditions.Monitor
{
  internal interface IMonitor
  {
    XDocument GetMonitorData(NameValueCollection qsCollection);
  }
}
