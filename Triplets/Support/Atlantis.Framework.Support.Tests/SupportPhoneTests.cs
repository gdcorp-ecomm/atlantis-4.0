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
      SupportPhoneRequestData request = new SupportPhoneRequestData(1);
      SupportPhoneRequestData request2 = new SupportPhoneRequestData(1);
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
      SupportPhoneRequestData request = new SupportPhoneRequestData(1);
      Assert.IsTrue(!string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void SupportPhoneResponseToXml()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1);
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void SupportPhoneInvalidResellerType()
    {
      try
      {
        SupportPhoneRequestData request = new SupportPhoneRequestData(0);
        SupportPhoneResponseData response =
          (SupportPhoneResponseData) Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex.Message.StartsWith("ResellerTypeId should be greater than 0"));
      }
    }

    private const int SUPPORTPHONEREQUESTTYPE = 733;

    [TestMethod]
    public void SupportPhoneGdUs()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1);
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);

      ISupportPhoneData supportPhoneData;
      if (response.TryGetSupportData("us", out supportPhoneData))
      {
        Assert.IsFalse(string.IsNullOrEmpty(supportPhoneData.Number));
        Assert.IsFalse(supportPhoneData.IsInternational);
        Assert.AreEqual(supportPhoneData.Number, "(480) 505-8877");
      }
      else
      {
        Assert.Fail("Failed to get support phone data");
      }
    }

    [TestMethod]
    public void SupportPhoneGdInternational()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(1);
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);

      ISupportPhoneData supportPhoneData;
      if (response.TryGetSupportData("au", out supportPhoneData))
      {
        Assert.IsFalse(string.IsNullOrEmpty(supportPhoneData.Number));
        Assert.IsTrue(supportPhoneData.IsInternational);
        Assert.AreEqual(supportPhoneData.Number, "02 8023 8592");
      }
      else
      {
        Assert.Fail("Failed to get support phone data");
      }
    }

    [TestMethod]
    public void SupportPhoneResellerUs()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(5);
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);

      ISupportPhoneData supportPhoneData;
      if (response.TryGetSupportData("us", out supportPhoneData))
      {
        Assert.IsFalse(string.IsNullOrEmpty(supportPhoneData.Number));
        Assert.IsFalse(supportPhoneData.IsInternational);
        Assert.AreEqual(supportPhoneData.Number, "(480) 624-2500");
      }
      else
      {
        Assert.Fail("Failed to get support phone data");
      }
    }

    [TestMethod]
    public void SupportPhoneResellerInternational()
    {
      SupportPhoneRequestData request = new SupportPhoneRequestData(5);
      SupportPhoneResponseData response = (SupportPhoneResponseData)Engine.Engine.ProcessRequest(request, SUPPORTPHONEREQUESTTYPE);

      ISupportPhoneData supportPhoneData;
      if (response.TryGetSupportData("in", out supportPhoneData))
      {
        Assert.IsFalse(string.IsNullOrEmpty(supportPhoneData.Number));
        Assert.IsTrue(supportPhoneData.IsInternational);
        Assert.AreEqual(supportPhoneData.Number, "1-800-121-0120");
      }
      else
      {
        Assert.Fail("Failed to get support phone data");
      }
    }
  }
}
