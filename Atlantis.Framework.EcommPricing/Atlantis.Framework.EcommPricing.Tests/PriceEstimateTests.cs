using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommPricing.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class PriceEstimateTests
  {
    const int _REQUESTTYPE = 657;
    
    [TestMethod]
    public void RequestDataConstructorGeneratesNewRequestDataObject()
    {
      var request = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty,string.Empty, 0, 1, 1, "USD", "Example", 1, 54);
      Assert.IsNotNull(request);
    }

    [TestMethod]
    public void RequestCacheKeyCaseInsenstive()
    {
      var request = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 1, "USD", "EXAMPLE", 1, 54);
      var request2 = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 1, "USD", "example", 1, 54);
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void GroupRequestCacheKeyDifferent()
    {
      var request = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 1, "USD", "Example1", 1, 54);
      var request2 = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 1, "USD", "Example2", 1, 54);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void LongPromoCodeIsTruncatedAtTwentyCharacters()
    {
      var request = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 1, "USD", "Example89012345678901234567890", 1, 54);
      var request2 = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 1, "USD", "Example8901234567890", 1, 54);
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataToXml()
    {
      string promoCode = "Example";
      var request = new PriceEstimateRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, 1, 1, "USD", promoCode, 1, 54);
      XElement.Parse(request.ToXML());
      Assert.IsTrue(request.ToXML().Contains(promoCode.ToUpperInvariant()));
    }
    
    [TestMethod]
    public void ResponseDataFromException()
    {
      var ex = new AtlantisException("PriceEstimateTests.ResponseDataFromException", "0", "Test Error Constructor", string.Empty, null, null);
      var response = PriceEstimateResponseData.FromException(ex);
      Assert.IsNotNull(response.GetException());
    }

    // Test is service response is empty string
    [TestMethod]
    public void EmptyWsResponseReturnsNoPriceFoundResponse()
    {
      var response = PriceEstimateResponseData.FromXml(string.Empty);
      Assert.IsNotNull(response);
      Assert.IsFalse(response.IsPriceFound);
    }

    // Test if service response is bad XML
    [TestMethod]
    public void InvalidWsXmlResponseReturnsNoPriceFoundResponse()
    {
      string badXml =
        "<PriceEstimate membershipLevel=\"1\" privateLabelID=\"1\" transactionCurrency=\"USD\" source_code=\"TESTHOST\"><Item pf_id=\"42002\" name=\"Hosting - Web - Economy - Linux - US Region - 1 year (recurring)\" priceGroupID=\"1\" list_price=\"6828\" _oadjust_adjustedprice=\"1200\" _icann_fee_adjusted=\"0\"/>";
      var response = PriceEstimateResponseData.FromXml(badXml);
      Assert.IsNotNull(response);
      Assert.IsFalse(response.IsPriceFound);
    }

    // Test if service response contains non-integer values for the prices
    [TestMethod]
    public void WsXmlWithNonIntegerPriceValuesReturnsNoPriceFoundResponse()
    {
      string badXml =
        "<PriceEstimate membershipLevel=\"1\" privateLabelID=\"1\" transactionCurrency=\"USD\" source_code=\"TESTHOST\"><Item pf_id=\"42002\" name=\"Hosting - Web - Economy - Linux - US Region - 1 year (recurring)\" priceGroupID=\"1\" list_price=\"6828\" _oadjust_adjustedprice=\"1200.0\" _icann_fee_adjusted=\"0.0\"/></PriceEstimate>";
      var response = PriceEstimateResponseData.FromXml(badXml);
      Assert.IsNotNull(response);
      Assert.IsFalse(response.IsPriceFound);
    }

    [TestMethod]
    public void ValidRequestReturnsValidResponse()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", "TestCode", 1, 54);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      Assert.IsNotNull(response);
      Assert.IsTrue(response.IsPriceFound);
    }

    // Test for invalid PLID
    [TestMethod]
    public void InValidPlidRequestReturnsNoPriceFoundResponse()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, -1, 1, "USD", "TestCode", 1, 54);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      Assert.IsNotNull(response);
      Assert.IsFalse(response.IsPriceFound);
    }

    // Test for Invalid PFID
    [TestMethod]
    public void InValidPfidRequestReturnsNoPriceFoundResponse()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", "TestCode", 1, 0);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      Assert.IsNotNull(response);
      Assert.IsFalse(response.IsPriceFound);
    }

    // Test for unavailable Currency
    // use "SGD"
    [TestMethod]
    public void InValidCurrencyRequestReturnsNoPriceFoundResponse()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "SGD", "TestCode", 1, 42001);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      Assert.IsNotNull(response);
      Assert.IsFalse(response.IsPriceFound);
    }

    // Test for missing promo code
    [TestMethod]
    public void ValidRequestWithoutPromoCodeReturnsValidResponse()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", string.Empty, 1, 54);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);

      Assert.IsNotNull(response);
      Assert.IsTrue(response.IsPriceFound);
    }

    // Test that valid promo code affects price
    [TestMethod]
    public void ValidRequestWithPromoCodeReturnsValidResponseAndSpecialPricing()
    {
      var request1 = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", string.Empty, 1, 42002);
      var response1 = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request1, _REQUESTTYPE);


      var request2 = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", "testhost", 1, 42002);
      var response2 = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request2, _REQUESTTYPE);

      Assert.IsNotNull(response1);
      Assert.IsNotNull(response2);
      Assert.IsTrue(response1.IsPriceFound && response2.IsPriceFound);
      Assert.AreNotEqual(response1.AdjustedPrice, response2.AdjustedPrice);
      Assert.IsTrue(response2.AdjustedPrice < response1.AdjustedPrice);
    }

    // Test that a valid promo code does not affect price for a non-targeted currency
    [TestMethod]
    public void ValidRequestWithPromoCodeForNonTargetCurrencyReturnsValidResponseAndNoSpecialPricing()
    {
      var request1 = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "MXN", string.Empty, 1, 42002);
      var response1 = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request1, _REQUESTTYPE);


      var request2 = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "MXN", "testhost", 1, 42002);
      var response2 = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request2, _REQUESTTYPE);

      Assert.IsNotNull(response1);
      Assert.IsNotNull(response2);
      Assert.IsTrue(response1.IsPriceFound && response2.IsPriceFound);
      Assert.AreEqual(response1.AdjustedPrice, response2.AdjustedPrice);
    }

    // Test that a valid promo code does not affect price for a non-targeted product
    [TestMethod]
    public void ValidRequestWithPromoCodeForNonTargetProductReturnsValidResponseAndNoSpecialPricing()
    {
      var request1 = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", string.Empty, 1, 54);
      var response1 = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request1, _REQUESTTYPE);


      var request2 = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", "testhost", 1, 54);
      var response2 = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request2, _REQUESTTYPE);

      Assert.IsNotNull(response1);
      Assert.IsNotNull(response2);
      Assert.IsTrue(response1.IsPriceFound && response2.IsPriceFound);
      Assert.AreEqual(response1.AdjustedPrice, response2.AdjustedPrice);
    }

    [TestMethod]
    public void ValidRequestWithDiscountCode()
    {
      var request1 = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", string.Empty, 1, 54, "cbwsp01");
      var response1 = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request1, _REQUESTTYPE);
      Assert.IsNotNull(response1);
      Assert.IsTrue(response1.IsPriceFound);
    }
        
    // Test the ToXml method on the response object
    [TestMethod]
    public void ResponseToXml()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", "testhost", 1, 54);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      XElement parsed = XElement.Parse(response.ToXML());
      Assert.IsNotNull(parsed);
    }

    // Test that non-https url fails
    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void RequestUsingNonHttpsServiceUrlFails()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", "testhost", 1, 54);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, 65701);
    }

    // Test that call using missing cert fails
    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void RequestUsingMissingCertFails()
    {
      var request = new PriceEstimateRequestData("859775", string.Empty, string.Empty, string.Empty, 1, 1, 1, "USD", "testhost", 1, 54);
      var response = (PriceEstimateResponseData)Engine.Engine.ProcessRequest(request, 65702);
    }
  }
}
