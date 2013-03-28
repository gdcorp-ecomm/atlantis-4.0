using System.Collections.Specialized;
using System.Xml.Linq;

namespace Atlantis.Framework.Tokens.Monitor
{
  internal interface IMonitor
  {
    XDocument GetMonitorData(NameValueCollection qsCollection);
  }
}
