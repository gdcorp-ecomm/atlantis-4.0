using System;
using System.Xml.Linq;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommPricing.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class PromoPriceTests
  {
    #region RequestData tests

    [TestMethod]
    public void PromoPriceRequestDataConstructorGeneratesNewRequestDataObject()
    {
      var request = new PromoPriceRequestData(0, 1, 99, 1, "USD", 2);
      Assert.IsNotNull(request);
      Assert.AreEqual(2, request.PriceGroupId);
    }

    [TestMethod]
    public void PromoPriceRequestDataConstructorSetsPriceGroupDefault()
    {
      var request = new PromoPriceRequestData(0, 1, 99, 1, "USD");
      Assert.AreEqual(0, request.PriceGroupId);
    }

    [TestMethod]
    public void PromoPriceRequestDataOptionsCommaDelimited()
    {
      var request = new PromoPriceRequestData(0, 1, 99, 2, "PHP", 8);
      Assert.AreEqual("2,PHP,8", request.Options);
    }

    [TestMethod]
    public void PromoPriceRequestDataToXml()
    {
      var request = new PromoPriceRequestData(1111, 2222, 9999, 3333, "PHP", 4444);
      string xml = request.ToXML();

      XElement requestRoot = XElement.Parse(xml);
      Assert.AreEqual("PromoPriceRequestData", requestRoot.Name);
      Assert.IsTrue(requestRoot.Attribute("UnifiedProductId").Value == "1111");
      Assert.IsTrue(requestRoot.Attribute("PrivateLabelId").Value == "2222");
      Assert.IsTrue(requestRoot.Attribute("ShopperPriceType").Value == "3333");
      Assert.IsTrue(requestRoot.Attribute("CurrencyType").Value == "PHP");
      Assert.IsTrue(requestRoot.Attribute("PriceGroupId").Value == "4444");
      Assert.IsTrue(requestRoot.Attribute("Quantity").Value == "9999");
    }

    [TestMethod]
    public void PromoPriceRequestDataCacheKeyDifferent()
    {
      var request = new PromoPriceRequestData(1111, 2222, 99999, 3333, "PHP", 4444);
      var request2 = new PromoPriceRequestData(1111, 2222, 99999, 3333, "USD", 4444);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    #endregion

    #region ResponseData tests

    [TestMethod]
    public void PromoPriceResponseDataFromPriceReturnsObject()
    {
      var response = PromoPriceResponseData.FromPrice(1234, true);
      Assert.IsNotNull(response);
      Assert.AreEqual(1234, response.Price);
      Assert.IsTrue(response.IsEstimate);
      Assert.IsTrue(response.IsPriceFound);
    }

    [TestMethod]
    public void PromoPriceResponseDataReturnsValidXml()
    {
      bool priceFound;
      bool isEstimate;

      var response = PromoPriceResponseData.FromPrice(1234, true);
      string xml = response.ToXML();
      XElement element = XElement.Parse(xml);
      Assert.AreEqual("PromoPriceResponseData", element.Name);
      Assert.IsTrue(bool.TryParse(element.Attribute("priceFound").Value, out priceFound));
      Assert.AreEqual("1234", element.Attribute("price").Value);
      Assert.IsTrue(bool.TryParse(element.Attribute("isEstimate").Value, out isEstimate));
    }

    #endregion

    #region Request tests

    const int _REQUESTTYPE = 691;

    [TestMethod]
    public void PromoPriceRequestExecuteValid()
    {
      var request = new PromoPriceRequestData(101, 1, 0, 1, "USD");
      var response = (PromoPriceResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsPriceFound);
      Assert.IsTrue(response.Price >= 0);
    }

    [TestMethod]
    public void PromoPriceRequestExecuteUnifiedProductIdReturnsNotFound()
    {
      var request = new PromoPriceRequestData(-101, 1, 0, 0, "USD");
      var response = (PromoPriceResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(response.IsPriceFound);
    }

    [TestMethod]
    public void PromoPriceRequestExecuteInvalidCurrencyTypeReturnsEstimate()
    {
      //  Invalid CurrencyType causes DataCache to throw an exception
      var request = new PromoPriceRequestData(101, 1, 0, 1, "XXXX");
      var response = (PromoPriceResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsEstimate);
    }

    #endregion

  }
}
