using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Purchase.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Purchase.Tests
{
  [TestClass]
  public class PurchaseErrorTests
  {
    //Tests of architecture no processing
    [TestMethod]
    public void PurchaseErrorRequestCacheKeySame()
    {
      PurchaseErrorRequestData request = new PurchaseErrorRequestData("01","01","en","us");
      PurchaseErrorRequestData request2 = new PurchaseErrorRequestData("01", "01", "en", "us");
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }
    [TestMethod]
    public void PurchaseErrorResponseException()
    {
      AtlantisException exception = new AtlantisException("StateTests.StateResponseException", "0", "TestMessage", "TestData", null, null);
      PurchaseErrorRequestData request = new PurchaseErrorRequestData("01", "01", "en", "us");
      PurchaseErrorResponseData response = PurchaseErrorResponseData.FromException(exception,request);
      Assert.AreEqual("There was a problem processing your transaction. Please verify your payment information or use an alternate form of payment.", response.CustomerFriendlyErrorText);

    }
    [TestMethod]
    public void PurchaseErrorResponseXML()
    {
      AtlantisException exception = new AtlantisException("StateTests.StateResponseException", "0", "TestMessage", "TestData", null, null);
      PurchaseErrorRequestData request = new PurchaseErrorRequestData("01", "01", "en", "us");
      PurchaseErrorResponseData response = PurchaseErrorResponseData.FromXML("<LocaleData errorText=\"Invalid payment type\" />",request);
      Assert.AreEqual("Invalid payment type", response.CustomerFriendlyErrorText);
    }
    //Tests that process the request
    [TestMethod]
    public void GetInvalidPaymentType()
    {
      PurchaseErrorRequestData request = new PurchaseErrorRequestData("-4", "1", "en", "us");
      PurchaseErrorResponseData response = (PurchaseErrorResponseData)Engine.Engine.ProcessRequest(request, 741);
      Assert.AreEqual("Invalid payment type", response.CustomerFriendlyErrorText);
    }
  }
}
