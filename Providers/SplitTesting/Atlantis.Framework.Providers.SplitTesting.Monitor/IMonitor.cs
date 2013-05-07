using System.Collections.Specialized;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.SplitTesting.Monitor
{
  internal interface IMonitor
  {
    XDocument GetMonitorData(NameValueCollection qsCollection);
  }
}
