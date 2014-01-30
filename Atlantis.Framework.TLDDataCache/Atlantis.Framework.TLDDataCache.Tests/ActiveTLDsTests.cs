using System.Xml.Linq;
using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class ActiveTLDsTests
  {
    const int ACTIVETLD_GOODREQUEST = 635;
    const int ACTIVETLD_BADREQUEST = 63555;

    [TestMethod]
    public void AllActiveTldsGoodRequest()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, ACTIVETLD_GOODREQUEST);
      Assert.IsTrue(response.TldSetsByActiveFlags.Any());
    }

    [TestMethod]
    public void AllActiveTldsBadRequest()
    {
      ActiveTLDsResponseData response = null;
      try
      {
        var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
        response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, ACTIVETLD_BADREQUEST);
      }
      catch (Exception)
      {
        Assert.IsTrue(response == null);
      }
    }

    [TestMethod]
    public void AllActiveTldsGoodRequestResponseToXml()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, ACTIVETLD_GOODREQUEST);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void AllActiveTldsResponseGetException()
    {
      var requestData = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      ActiveTLDsResponseData response = ActiveTLDsResponseData.FromException(requestData, new Exception("testing"));
      Assert.IsTrue(response.GetException() != null && !string.IsNullOrEmpty(response.GetException().ErrorDescription));
    }

    [TestMethod]
    public void AllActiveTldsRequestToXml()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var requestXml = request.ToXML();
      var requestXmlElement = XElement.Parse(requestXml);
      Assert.IsTrue(!string.IsNullOrEmpty(requestXml) && requestXmlElement != null);
    }

    [TestMethod]
    public void AllActiveTldsRequestGetCacheMd5()
    {
      var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var requestCacheMd5 = request.GetCacheMD5();
      Assert.IsTrue(!string.IsNullOrEmpty(requestCacheMd5));
    }
  }
}
