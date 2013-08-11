using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Support.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Support.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Support.Impl.dll")]
  public class AppSettingTests
  {
    [TestMethod]
    public void SupportPhoneCacheKey()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1, "us");
      SupportPhoneRequestData request2 = new SupportPhoneRequestData(1, "us");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void SupportPhoneExceptionResponse()
    {
      AtlantisException ex = new AtlantisException("AppSettingExceptionResponse.Test", "0", "TestError", "TestData", null, null);
      SupportPhoneResponseData response = SupportPhoneResponseData.FromException(ex);
      Assert.AreEqual("TestError", response.GetException().Message);
      Assert.AreEqual("TestData", response.GetException().ExData);
    }

    [TestMethod]
    public void SupportPhoneRequestToXml()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1, "us");
      Assert.IsTrue(!string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void SupportPhoneResponseToXml()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1, "us");
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void SupportPhoneNoCountryCode()
    {
      try
      {
        SupportPhoneRequestData request = new SupportPhoneRequestData(1, "");
        SupportPhoneResponseData response =
          (SupportPhoneResponseData) Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex.Message.StartsWith("Null or empty country code"));
      }
    }

    private const int SUPPORTPHONEREQUESTTYPE = 733;

    [TestMethod]
    public void SupportPhoneGdUs()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1, "us");
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.SupportPhoneData.Number));
      Assert.IsFalse(response.SupportPhoneData.IsInternational);
      Assert.AreEqual(response.SupportPhoneData.Number, "(480) 505-8877");
    }

    [TestMethod]
    public void SupportPhoneGdInternational()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1, "au");
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.SupportPhoneData.Number));
      Assert.IsTrue(response.SupportPhoneData.IsInternational);
      Assert.AreEqual(response.SupportPhoneData.Number, "02 8023 8592");
    }

    [TestMethod]
    public void SupportPhoneResellerUs()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(5, "us");
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.SupportPhoneData.Number));
      Assert.IsFalse(response.SupportPhoneData.IsInternational);
      Assert.AreEqual(response.SupportPhoneData.Number, "(480) 624-2500");
    }

    [TestMethod]
    public void SupportPhoneResellerInternation()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(5, "in");
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      Assert.IsFalse(string.IsNullOrEmpty(response.SupportPhoneData.Number));
      Assert.IsTrue(response.SupportPhoneData.IsInternational);
      Assert.AreEqual(response.SupportPhoneData.Number, "1-800-121-0120");
    }
  }
}
