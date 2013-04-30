using System;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SplitTesting.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.SplitTesting.Impl.dll")]
  public class ActiveSplitTestsByCategoryNameTests
  {
    // <data count="1"><item SplitTestID="1" VersionNumber="1" EligibilityRules="dataCenterAny(AP)" SplitTestRunID="1" /></data>

    [TestMethod]
    public void ActiveTestsRequestDataProperties()
    {
      var request = new ActiveSplitTestsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "test");
      Assert.AreEqual("test", request.CategoryName);

      var request2 = new ActiveSplitTestsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "test");
      Assert.AreEqual(request2.GetCacheMD5(), request.GetCacheMD5());
    }

    [TestMethod]
    public void ActiveTestsRequestDataToXml()
    {
      var request = new ActiveSplitTestsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "test");
      Assert.AreEqual(true, !string.IsNullOrEmpty(request.ToXML().ToString()));
    }

    [TestMethod]
    public void ActiveTestsResponseDataFromBadXml()
    {
      var response = ActiveSplitTestsResponseData.FromCacheXml("<data>hello");
      Assert.AreEqual(false, response.SplitTests.Any());
    }

    [TestMethod]
    public void ActiveTestsResponseDataNoItems()
    {
      const string data = "<data count=\"0\"></data>";
      var response = ActiveSplitTestsResponseData.FromCacheXml(data);
      Assert.AreEqual(false, response.SplitTests.Any());
    }

    [TestMethod]
    public void ActiveTestsResponseDataWithEmptyItems()
    {
      const string data = "<data count=\"1\"><item SplitTestID=\"\" VersionNumber=\"\" EligibilityRules=\"dataCenterAny(AP)\" SplitTestRunID=\"1\" TestStartDate=\"04/22/2013\" /></data>";
      var response = ActiveSplitTestsResponseData.FromCacheXml(data);
      Assert.AreEqual(false, response.SplitTests.Any());
    }

    [TestMethod]
    public void ActiveTestsResponseDataWithValidItems()
    {
      const string data = "<data count=\"1\"><item SplitTestID=\"1\" VersionNumber=\"1\" EligibilityRules=\"dataCenterAny(AP)\" SplitTestRunID=\"1\" TestStartDate=\"04/22/2013\" /></data>";
      var response = ActiveSplitTestsResponseData.FromCacheXml(data);
      Assert.AreEqual(true, response.SplitTests.Any());
    }

    [TestMethod]
    public void ActiveTestsResponseDataWithValidItemsToXml()
    {
      const string data = "<data count=\"1\"><item SplitTestID=\"1\" VersionNumber=\"1\" EligibilityRules=\"dataCenterAny(AP)\" SplitTestRunID=\"1\" TestStartDate=\"04/22/2013\" /></data>";
      var response = ActiveSplitTestsResponseData.FromCacheXml(data);

      XElement a = XElement.Parse(response.ToXML());
      Assert.AreEqual(true, !string.IsNullOrEmpty(a.ToString()));
    }

    const int _REQUESTTYPE = 684;

    [TestMethod]
    public void RunActiveTestsRequestWithBadRequestData()
    {
      var request = new XData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      try
      {
        Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.ToString()));
      }
    }

    [TestMethod]
    public void RunActiveTestsRequestWithNoData()
    {
      var request = new ActiveSplitTestsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "999999");
      var response = (ActiveSplitTestsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(false, response.SplitTests.Any());
    }

    [TestMethod]
    public void RunActiveTestsRequestWithValidData()
    {
      var request = new ActiveSplitTestsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "test");
      var response = (ActiveSplitTestsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      Assert.AreEqual(true, response.SplitTests.Any());
    }

    internal class XData : RequestData
    {
      internal XData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
        : base(shopperId, sourceURL, orderId, pathway, pageCount)
      {
        
      }

      public override string GetCacheMD5()
      {
        throw new System.NotImplementedException();
      }
    }
  }
}
