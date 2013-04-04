using System;
using Atlantis.Framework.Engine.Monitor.EngineCallStats;
using Atlantis.Framework.Engine.Tests.MockTriplet;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.Engine.Monitor.WebTest
{
  public partial class _default : System.Web.UI.Page
  {

    private EngineCallStatsProvider _provider;
    protected EngineCallStatsProvider statsProvider
    {
      get
      {
        if (_provider == null)
        {
          _provider = HttpProviderContainer.Instance.Resolve<EngineCallStatsProvider>();
        }

        return _provider;
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      RunEngineCalls();

      IDebugContext debug = HttpProviderContainer.Instance.Resolve<IDebugContext>();
      statsProvider.LogDebugData(debug);
    }

    private void RunEngineCalls()
    {
      for (int i = 0; i < 5; i++)
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