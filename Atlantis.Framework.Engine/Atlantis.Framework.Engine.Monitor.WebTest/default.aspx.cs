using System;
using Atlantis.Framework.Engine.Monitor.Trace;
using Atlantis.Framework.Engine.Tests.MockTriplet;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.Engine.Monitor.WebTest
{
  public partial class _default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      RunEngineCalls();

      var trace = statsProvider.EngineTraceStats;
    }

    private EngineTraceProvider _provider;
    protected EngineTraceProvider statsProvider
    {
      get
      {
        if (_provider == null)
        {
          _provider = HttpProviderContainer.Instance.Resolve<EngineTraceProvider>();
        }

        return _provider;
      }
    }


    private void RunEngineCalls()
    {
      for (int i = 0; i < 1; i++)
      {
        try
        {
          ConfigTestRequestData request = new ConfigTestRequestData("832652", string.Empty, string.Empty, string.Empty, 0);
          ConfigTestResponseData response = (ConfigTestResponseData)Engine.ProcessRequest(request, 9997);
        }
        catch { }
      }
    }
  }
}