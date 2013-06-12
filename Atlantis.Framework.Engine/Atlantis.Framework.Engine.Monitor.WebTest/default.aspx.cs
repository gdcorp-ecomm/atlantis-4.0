using System;
using Atlantis.Framework.Engine.Monitor.Trace;
using Atlantis.Framework.Engine.Tests.MockTriplet;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.Engine.Monitor.WebTest
{
  public partial class _default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      RunEngineCalls();
    }

    protected string RequestTraceStats
    {
      get
      {
        var trace = HttpProviderContainer.Instance.Resolve<HttpRequestEngineTraceProvider>();

        int success = 0;
        int failed = 0;
        foreach (var completedRequest in trace.CompletedEngineRequests)
        {
          if (completedRequest.Exception != null)
          {
            failed++;
          }
          else
          {
            success++;
          }
        }

        return string.Format("successful={0} : failed={1}", success.ToString(), failed.ToString());
      }
    }

    private void RunEngineCalls()
    {
      for (int i = 0; i < 100; i++)
      {
        try
        {
          ConfigTestRequestData request = new ConfigTestRequestData();
          ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9997);
        }
        catch { }
      }
    }
  }
}