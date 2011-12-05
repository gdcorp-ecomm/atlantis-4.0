using System;
using System.Collections.Generic;
using System.Threading;
using Atlantis.Framework.FabDomains.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FabDomains.Tests
{
  [TestClass]
  public class FabDomainsTest
  {
    private TestContext testContextInstance;
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BasicFabDomainsTest()
    {
      FabDomainsRequestData requestData = new FabDomainsRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0);
      requestData.SLD = "godaddy";
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");
      requestData.AddTLDs(PremiumTLDs);
      requestData.ReturnCount = 10;
      requestData.RequestTimeout = TimeSpan.FromMilliseconds(3000);

      FabDomainsResponseData response = (FabDomainsResponseData)Engine.Engine.ProcessRequest(requestData, 18);
      Assert.IsTrue(response.IsSuccess);
    }

    private volatile bool _asyncSearchComplete = false;
    private FabDomainsResponseData _asyncResponse = null;
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void FabDomainsAsyncTest()
    {
      _asyncResponse = null;

      FabDomainsRequestData requestData = new FabDomainsRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0);
      requestData.RequestTimeout = TimeSpan.FromMilliseconds(3000);
      requestData.SLD = "godaddy";
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");
      requestData.AddTLDs(PremiumTLDs);
      requestData.ReturnCount = 10;
      requestData.RequestTimeout = TimeSpan.FromMilliseconds(3000);

      IAsyncResult asyncResult = Engine.Engine.BeginProcessRequest(requestData, 23, EndFabDomainsAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse.IsSuccess);
    }

    private void EndFabDomainsAsyncTest(IAsyncResult result)
    {
      _asyncResponse = Engine.Engine.EndProcessRequest(result) as FabDomainsResponseData;
      _asyncSearchComplete = true;
    }

  }
}
