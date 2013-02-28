using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommPricing.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  public class ValidateNonOrderTests
  {
    const int _REQUESTTYPE = 644;

    [TestMethod]
    public void RequestToXml()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOIPO");
      string xml = request.ToXML();
      Assert.IsTrue(xml.Contains("\"1\""));
      Assert.IsTrue(xml.Contains("\"GDPROMOIPO\""));
    }

    [TestMethod]
    public void RequestCacheKeyMatch()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOIPO");
      var request2 = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOipo");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());      
    }

    [TestMethod]
    public void RequestCacheKeyDifferentPrivateLabelId()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOIPO");
      var request2 = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 2, "GDPROMOipo");
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestCacheKeyDifferentPromoCode()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOIPO");
      var request2 = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOip0");
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestBadPrivateLabelId()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, -123, "GDPROMOIPO");
      var response = (ValidateNonOrderResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(ValidateNonOrderResponseData.InActiveResponse, response);
    }

    [TestMethod]
    public void RequestBadPromoCodeTooLong()
    {
      string twentyone = "123456789012345678901";
      string twenty = "12345678901234567890";
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, twentyone);
      Assert.AreEqual(twenty, request.PromoCode);
    }

    [TestMethod]
    public void RequestBadPromoCodeNull()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, null);
      Assert.AreEqual(string.Empty, request.PromoCode);
    }

    [TestMethod]
    public void RequestBadPromoCodeEmpty()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, string.Empty);
      Assert.AreEqual(string.Empty, request.PromoCode);
    }

    [TestMethod]
    public void RequestBadPromoCodeNonAlphaNumeric()
    {
      string promoCode = "blue-\n\rX";
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, promoCode);
      Assert.AreEqual(string.Empty, request.PromoCode);
    }

    [TestMethod]
    public void NullFromCacheDataXml()
    {
      var response = ValidateNonOrderResponseData.FromCacheDataXml(null);
      Assert.AreEqual(ValidateNonOrderResponseData.InActiveResponse, response);
    }

    [TestMethod]
    public void EmptyStringFromCacheDataXml()
    {
      var response = ValidateNonOrderResponseData.FromCacheDataXml(string.Empty);
      Assert.AreEqual(ValidateNonOrderResponseData.InActiveResponse, response);
    }

    [TestMethod]
    public void ValidEmptyFromCacheDataXml()
    {
      string text = GetTextDataResource("SampleEmptyOutput.xml");
      var response = ValidateNonOrderResponseData.FromCacheDataXml(text);
      Assert.AreEqual(ValidateNonOrderResponseData.InActiveResponse, response);
    }

    [TestMethod]
    public void FromException()
    {
      AtlantisException aex = new AtlantisException("ValidateNonOrderTests.FromException", "0", "TestException", "TestData", null, null);
      var response = ValidateNonOrderResponseData.FromException(aex);
      Assert.IsNotNull(response.GetException());
      Assert.AreEqual("TestException", response.GetException().Message);
    }

    [TestMethod]
    public void ValidFromCacheDataXml()
    {
      string text = GetTextDataResource("SampleValidOutput.xml");
      var response = ValidateNonOrderResponseData.FromCacheDataXml(text);
      Assert.AreNotEqual(ValidateNonOrderResponseData.InActiveResponse, response);
      Assert.IsTrue(response.IsActive);
      Assert.AreNotEqual(DateTime.MinValue, response.StartDate);
      Assert.AreNotEqual(DateTime.MaxValue, response.EndDate);
    }

    [TestMethod]
    public void ValidBadDateFromCacheDataXml()
    {
      string text = GetTextDataResource("SampleBadDates.xml");
      var response = ValidateNonOrderResponseData.FromCacheDataXml(text);
      Assert.AreNotEqual(ValidateNonOrderResponseData.InActiveResponse, response);
      Assert.IsTrue(response.IsActive);
      Assert.AreEqual(DateTime.MinValue, response.StartDate);
      Assert.AreEqual(DateTime.MaxValue, response.EndDate);
    }

    private static string GetTextDataResource(string dataName)
    {
      string result;
      string resourcePath = "Atlantis.Framework.EcommPricing.Tests." + dataName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        result = textReader.ReadToEnd();
      }

      return result;
    }

    [TestMethod]
    public void InActiveResponse()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOIP0");
      var response = (ValidateNonOrderResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(response.IsActive);
    }

    [TestMethod]
    public void ActiveResponse()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOIPO");
      var response = (ValidateNonOrderResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsActive);
    }

    [TestMethod]
    public void ResponseToXml()
    {
      var request = new ValidateNonOrderRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, "GDPROMOIPO");
      var response = (ValidateNonOrderResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
      XElement parsed = XElement.Parse(response.ToXML());
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void ExceptionInRequest()
    {
      var request = new WrongRequestType();
      var response = (ValidateNonOrderResponseData)DataCache.DataCache.GetProcessRequest(request, _REQUESTTYPE);
    }

    private class WrongRequestType : RequestData
    {
      public WrongRequestType()
        : base(string.Empty, string.Empty, string.Empty, string.Empty, 0)
      { }

      public override string GetCacheMD5()
      {
        return "Nothing";
      }
    }
  }
}
