using System;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.SplitTesting.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SplitTesting.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.SplitTesting.Impl.dll")]
  public class ActiveSplitTestDetailsTests
  {
    // <data count="1"><item SplitTestSideID="1" SideName="A" PercentAllocation="50.00"/><item SplitTestSideID="1" SideName="B" PercentAllocation="50.00" /></data>

    [TestMethod]
    public void ActiveTestDetailsRequestDataProperties()
    {
      var request = new ActiveSplitTestDetailsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      Assert.AreEqual(1, request.SplitTestId);

      var request2 = new ActiveSplitTestDetailsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      Assert.AreEqual(request2.GetCacheMD5(), request.GetCacheMD5());
    }

    [TestMethod]
    public void ActiveTestDetailsRequestDataToXml()
    {
      var request = new ActiveSplitTestDetailsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1);
      Assert.AreEqual(true, !string.IsNullOrEmpty(request.ToXML().ToString()));
    }

    [TestMethod]
    public void ActiveTestDetailsResponseDataFromBadXml()
    {
      var response = ActiveSplitTestDetailsResponseData.FromCacheXml("<data>hello");
      Assert.AreEqual(false, response.SplitTestDetails.Any());
    }

    [TestMethod]
    public void ActiveTestDetailsResponseDataNoItems()
    {
      const string data = "<data count=\"0\"></data>";
      var response = ActiveSplitTestDetailsResponseData.FromCacheXml(data);
      Assert.AreEqual(false, response.SplitTestDetails.Any());
    }

    [TestMethod]
    public void ActiveTestDetailsResponseDataWithEmptyItems()
    {
      const string data = "<data count=\"1\"><item SplitTestSideID=\"\" SideName=\"\" InitialPercentAllocation=\"50.00\"/></data>";
      var response = ActiveSplitTestDetailsResponseData.FromCacheXml(data);
      Assert.AreEqual(false, response.SplitTestDetails.Any());
    }

    [TestMethod]
    public void ActiveTestDetailsResponseDataWithValidItems()
    {
      const string data = "<data count=\"1\"><item SplitTestSideID=\"1\" SideName=\"A\" InitialPercentAllocation=\"50.00\"/><item SplitTestSideID=\"1\" SideName=\"B\" PercentAllocation=\"50.00\" /></data>";
      var response = ActiveSplitTestDetailsResponseData.FromCacheXml(data);
      Assert.AreEqual(true, response.SplitTestDetails.Any());
    }

    [TestMethod]
    public void ActiveTestDetailsResponseDataWithValidItemsToXml()
    {
      const string data = "<data count=\"1\"><item SplitTestSideID=\"1\" SideName=\"A\" InitialPercentAllocation=\"50.00\"/><item SplitTestSideID=\"1\" SideName=\"B\" PercentAllocation=\"50.00\" /></data>";
      var response = ActiveSplitTestDetailsResponseData.FromCacheXml(data);

      XElement a = XElement.Parse(response.ToXML());
      Assert.AreEqual(true, !string.IsNullOrEmpty(a.ToString()));
    }

    const int _REQUESTTYPE = 685;

    [TestMethod]
    public void RunActiveTestDetailsRequestWithBadRequestData()
    {
      var request = new Atlantis.Framework.SplitTesting.Tests.ActiveSplitTestsByCategoryNameTests.XData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

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
    public void RunActiveTestDetailsRequestWithNoData()
    {
      var request = new ActiveSplitTestDetailsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 999999);
      var response = (ActiveSplitTestDetailsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(false, response.SplitTestDetails.Any());
    }

    [TestMethod]
    public void RunActiveTestDetailsRequestWithValidData()
    {
      var request = new ActiveSplitTestDetailsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1008);
      var response = (ActiveSplitTestDetailsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      Assert.AreEqual(true, response.SplitTestDetails.Any());
    }

  }
}
