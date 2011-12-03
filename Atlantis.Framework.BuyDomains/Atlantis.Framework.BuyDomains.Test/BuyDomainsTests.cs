using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.BuyDomains.Interface;
using Atlantis.Framework.Engine;
using System.Threading;

namespace Atlantis.Frameowrk.BuyDomains.Tests
{
  [TestClass]
  public class BuyDomainsTests
  {
    public BuyDomainsTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
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
    public void BuyDomainsBasicTest()
    {
      BuyDomainsRequestData requestData = new BuyDomainsRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0);
      requestData.SLD = "godaddy";
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");
      requestData.AddTLDs(PremiumTLDs);
      requestData.MaxResults = 10;
      requestData.RequestTimeout = TimeSpan.FromMilliseconds(3000);

      BuyDomainsResponseData response = (BuyDomainsResponseData)Engine.ProcessRequest(requestData, 15);
      Assert.IsTrue(response.IsSuccess);
    }

    private volatile bool _asyncSearchComplete = false;
    private BuyDomainsResponseData _asyncResponse = null;
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BuyDomainsAsyncTest()
    {
      _asyncResponse = null;

      BuyDomainsRequestData requestData = new BuyDomainsRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0);
      requestData.RequestTimeout = TimeSpan.FromMilliseconds(3000);
      requestData.SLD = "godaddy";
      List<string> PremiumTLDs = new List<string>();
      PremiumTLDs.Add("com");
      PremiumTLDs.Add("net");
      requestData.AddTLDs(PremiumTLDs);
      requestData.MaxResults = 10;
      requestData.RequestTimeout = TimeSpan.FromMilliseconds(3000);

      IAsyncResult asyncResult = Engine.BeginProcessRequest(requestData, 20, EndBuyDomainsAsyncTest, null);
      while (!_asyncSearchComplete)
      {
        Thread.Sleep(TimeSpan.FromMilliseconds(500));
      }

      Assert.IsNotNull(_asyncResponse);
      Assert.IsTrue(_asyncResponse.IsSuccess);
    }

    private void EndBuyDomainsAsyncTest(IAsyncResult result)
    {
      _asyncResponse = Engine.EndProcessRequest(result) as BuyDomainsResponseData;
      _asyncSearchComplete = true;
    }

  }
}
