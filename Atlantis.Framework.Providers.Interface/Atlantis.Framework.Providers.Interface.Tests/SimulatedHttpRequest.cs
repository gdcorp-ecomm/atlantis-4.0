using System;
using System.IO;
using System.Web.Hosting;

namespace Atlantis.Framework.Interface.Tests
{
  public class SimulatedHttpRequest : SimpleWorkerRequest
  {
    private readonly string _host;

    public SimulatedHttpRequest(string appVirtualDir, string appPhysicalDir, string page, string query, TextWriter output, string host)
                                : base(appVirtualDir, appPhysicalDir, page, query, output)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host", "Host cannot be null nor empty.");
      _host = host;
    }

    public override string GetServerName()
    {
      return _host;
    }

    public override string MapPath(string virtualPath)
    {
      return Path.Combine(GetAppPath(), virtualPath);
    }
  }
}
