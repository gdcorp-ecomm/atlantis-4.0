using System;
using Atlantis.Framework.Engine.Tests.MockTriplet;

namespace Atlantis.Framework.Engine.Monitor.WebTest
{
  public partial class _default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      RunEngineCalls();
    }

    private void RunEngineCalls()
    {
      for (int i = 0; i < 500; i++)
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