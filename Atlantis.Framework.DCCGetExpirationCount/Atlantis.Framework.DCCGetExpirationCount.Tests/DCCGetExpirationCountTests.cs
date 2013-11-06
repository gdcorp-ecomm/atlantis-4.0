using System;
using Atlantis.Framework.DCCGetExpirationCount.Interface;
using Atlantis.Framework.DCCGetExpirationCount.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCGetExpirationCount.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.DCCGetExpirationCount.Impl.dll")]
  [DeploymentItem("atlantis.config")]
  public class DCCGetExpirationCountTests
  {
    [TestMethod]
    public void DCCGetExpirationCountRequestDataProperties()
    {
      var request = new DCCGetExpirationCountRequestData("832652", "FOS", 90);
      Assert.AreEqual("832652", request.ShopperID);
      Assert.AreEqual("FOS", request.ApplicationName);
      Assert.AreEqual(90, request.DaysFromExpiration);
    }

    [TestMethod]
    public void DCCGetExpirationCountResponseDataProperties()
    {
      var response = DCCGetExpirationCountResponseData.FromResponseXml("832652", Resources.SuccessResponse);
      Assert.AreEqual(7,response.TotalDomains);
      Assert.AreEqual(8, response.AlreadyExpiredDomains);
      Assert.AreEqual(9, response.ExpiringDomains);
      Assert.IsTrue(response.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DCCGetExpirationCountResponseDataPropertiesNonSuccess()
    {
      var response = DCCGetExpirationCountResponseData.FromResponseXml("832652", Resources.NonSuccessResponse);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DCCGetExpirationCountResponseDataPropertiesMissingSuccess()
    {
      var response = DCCGetExpirationCountResponseData.FromResponseXml("832652", Resources.MissingSuccessResponse);
    }

    [TestMethod]
    public void DCCGetExpirationCountResponseDataPropertiesMissingShopper()
    {
      var response = DCCGetExpirationCountResponseData.FromResponseXml("832652", Resources.MissingShopperResponse);
      Assert.IsTrue(ReferenceEquals(DCCGetExpirationCountResponseData.None, response));
    }


    private const int _REQUESTTYPE = 120;

    [TestMethod]
    public void DCCGetExpirationCountBasic()
    {
      var request = new DCCGetExpirationCountRequestData("832652", "FOS", 91);
      request.RequestTimeout = TimeSpan.FromSeconds(10);
      var response = (DCCGetExpirationCountResponseData) Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsValid);
    }

    [TestMethod]
    public void DCCGetExpirationCountEmptyShopper()
    {
      var request = new DCCGetExpirationCountRequestData(string.Empty, "FOS", 91);
      request.RequestTimeout = TimeSpan.FromSeconds(10);
      var response = (DCCGetExpirationCountResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsValid);
      Assert.IsTrue(ReferenceEquals(DCCGetExpirationCountResponseData.None, response));
    }

  }
}
