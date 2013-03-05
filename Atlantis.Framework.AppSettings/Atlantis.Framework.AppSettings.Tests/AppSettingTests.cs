using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.AppSettings.Interface;
using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.AppSettings.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class AppSettingTests
  {
    [TestMethod]
    public void AppSettingCacheKey()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "blue");
      AppSettingRequestData request2 = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "blue");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void AppSettingCacheKeyCaseInsensitive()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "blue");
      AppSettingRequestData request2 = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Blue");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void AppSettingCacheKeyNull()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
      Assert.AreEqual(AppSettingRequestData.InvalidSettingName, request.SettingName);
      Assert.AreEqual(AppSettingRequestData.InvalidSettingName, request.GetCacheMD5());
    }

    [TestMethod]
    public void AppSettingRequestXml()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "blue");
      string xml = request.ToXML();
      Assert.IsTrue(xml.Contains("blue"));
      XElement.Parse(xml);
    }

    [TestMethod]
    public void AppSettingEmptySettingObject()
    {
      Assert.AreEqual(string.Empty, AppSettingResponseData.EmptySetting.SettingValue);
    }

    [TestMethod]
    public void AppSettingNullResponse()
    {
      AppSettingResponseData response = AppSettingResponseData.FromSettingValue(null);
      Assert.AreEqual(AppSettingResponseData.EmptySetting, response);
    }

    [TestMethod]
    public void AppSettingEmptyResponse()
    {
      AppSettingResponseData response = AppSettingResponseData.FromSettingValue(string.Empty);
      Assert.AreEqual(AppSettingResponseData.EmptySetting, response);
    }

    [TestMethod]
    public void AppSettingValidResponse()
    {
      AppSettingResponseData response = AppSettingResponseData.FromSettingValue("blueValue");
      Assert.AreEqual("blueValue", response.SettingValue);
    }

    [TestMethod]
    public void AppSettingExceptionResponse()
    {
      AtlantisException ex = new AtlantisException("AppSettingExceptionResponse.Test", "0", "TestError", "TestData", null, null);
      AppSettingResponseData response = AppSettingResponseData.FromException(ex);
      Assert.AreEqual("TestError", response.GetException().Message);
      Assert.AreEqual("TestData", response.GetException().ExData);
    }

    [TestMethod]
    public void AppSettingResponseXml()
    {
      AppSettingResponseData response = AppSettingResponseData.FromSettingValue("blueValue");
      string xml = response.ToXML();
      Assert.IsTrue(xml.Contains("blueValue"));
      XElement.Parse(xml);
    }

    const int _APPSETTINGREQUESTTYPE = 658;

    [TestMethod]
    public void AppSettingGet()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "SALES_VALID_COUNTRY_SUBDOMAINS");
      AppSettingResponseData response = (AppSettingResponseData)Engine.Engine.ProcessRequest(request, _APPSETTINGREQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.SettingValue));
    }

    [TestMethod]
    public void AppSettingGetNull()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, null);
      AppSettingResponseData response = (AppSettingResponseData)Engine.Engine.ProcessRequest(request, _APPSETTINGREQUESTTYPE);
      Assert.AreEqual(AppSettingResponseData.EmptySetting, response);
    }

    [TestMethod]
    public void AppSettingGetMissing()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "Dont-ever-Create-This-App-Setting-in-dev");
      AppSettingResponseData response = (AppSettingResponseData)Engine.Engine.ProcessRequest(request, _APPSETTINGREQUESTTYPE);
      Assert.AreEqual(AppSettingResponseData.EmptySetting, response);
    }

    [TestMethod]
    public void AppSettingGetMixedCaseKey()
    {
      AppSettingRequestData request = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "SALES_VALID_COUNTRY_SUBDOMAINS");
      AppSettingResponseData response = (AppSettingResponseData)Engine.Engine.ProcessRequest(request, _APPSETTINGREQUESTTYPE);

      AppSettingRequestData request2 = new AppSettingRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "sales_VALID_COUNTRY_SUBDOMAINS");
      AppSettingResponseData response2 = (AppSettingResponseData)Engine.Engine.ProcessRequest(request2, _APPSETTINGREQUESTTYPE);

      Assert.AreEqual(response.SettingValue, response2.SettingValue);
    }

    [TestMethod] 
    [ExpectedException(typeof(AtlantisException))]
    public void AppSettingException()
    {
      NotAppSettingRequestData request = new NotAppSettingRequestData();
      AppSettingResponseData response = (AppSettingResponseData)Engine.Engine.ProcessRequest(request, _APPSETTINGREQUESTTYPE);
    }

    private class NotAppSettingRequestData : RequestData
    {
      public NotAppSettingRequestData() 
        : base(string.Empty, string.Empty, string.Empty, string.Empty, 0)
      {

      }

      public override string GetCacheMD5()
      {
        return "test";
      }
    }

  }
}
