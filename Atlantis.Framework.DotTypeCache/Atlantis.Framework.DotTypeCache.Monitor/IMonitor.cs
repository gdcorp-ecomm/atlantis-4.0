using System.Collections.Specialized;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal interface IMonitor
  {
    XDocument GetMonitorData(NameValueCollection qsCollection);
  }
}
