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
  public class ListPriceTests
  {
    #region RequestData tests

    [TestMethod]
    public void ListPriceRequestDataConstructorGeneratesNewRequestDataObject()
    {
      var request = new ListPriceRequestData(0, 1, 1, "USD", 2);
      Assert.IsNotNull(request);
      Assert.AreEqual(2, request.PriceGroupId);
    }

    [TestMethod]
    public void ListPriceRequestDataConstructorSetsPriceGroupDefault()
    {
      var request = new ListPriceRequestData(0, 1, 1, "USD");
      Assert.AreEqual(0, request.PriceGroupId);
    }

    [TestMethod]
    public void ListPriceRequestDataOptionsCommaDelimited()
    {
      var request = new ListPriceRequestData(0, 1, 2, "PHP", 8);
      Assert.AreEqual("2,PHP,8", request.Options);
    }

    [TestMethod]
    public void ListPriceRequestDataToXml()
    {
      var request = new ListPriceRequestData(1111, 2222, 3333, "PHP", 4444);
      string xml = request.ToXML();

      XElement requestRoot = XElement.Parse(xml);
      Assert.AreEqual("ListPriceRequestData", requestRoot.Name);
      Assert.IsTrue(requestRoot.Attribute("UnifiedProductId").Value == "1111");
      Assert.IsTrue(requestRoot.Attribute("PrivateLabelId").Value == "2222");
      Assert.IsTrue(requestRoot.Attribute("ShopperPriceType").Value == "3333");
      Assert.IsTrue(requestRoot.Attribute("CurrencyType").Value == "PHP");
      Assert.IsTrue(requestRoot.Attribute("PriceGroupId").Value == "4444");      
    }

    [TestMethod]
    public void ListPriceRequestDataCacheKeyDifferent()
    {
      var request = new ListPriceRequestData(1111, 2222, 3333, "PHP", 4444);
      var request2 = new ListPriceRequestData(1111, 2222, 3333, "USD", 4444);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }
    #endregion

    #region ResponseData tests

    [TestMethod]
    public void ListPriceResponseDataFromPriceReturnsObject()
    {
      var response = ListPriceResponseData.FromPrice(1234, true);
      Assert.IsNotNull(response);
      Assert.AreEqual(1234, response.Price);
      Assert.IsTrue(response.IsEstimate);
      Assert.IsTrue(response.IsPriceFound);
    }

    [TestMethod]
    public void ListPriceResponseDataReturnsValidXml()
    {
      bool priceFound;
      bool isEstimate;

      var response = ListPriceResponseData.FromPrice(1234, true);
      string xml = response.ToXML();
      XElement element = XElement.Parse(xml);
      Assert.AreEqual("ListPriceResponseData", element.Name);
      Assert.IsTrue(bool.TryParse(element.Attribute("priceFound").Value, out priceFound));
      Assert.AreEqual("1234", element.Attribute("price").Value);
      Assert.IsTrue(bool.TryParse(element.Attribute("isEstimate").Value, out isEstimate));
    }

    #endregion

    #region Request tests

    const int _REQUESTTYPE = 690;

    [TestMethod]
    public void ListPriceRequestExecuteValid()
    {
      var request = new ListPriceRequestData(101, 1, 1, "USD");
      var response = (ListPriceResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsPriceFound);
      Assert.IsTrue(response.Price >= 0);
    }

    [TestMethod]
    public void ListPriceRequestExecuteUnifiedProductIdReturnsNotFound()
    {
      var request = new ListPriceRequestData(-101, 1, 0, "USD");
      var response = (ListPriceResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(response.IsPriceFound);
    }

    [TestMethod]
    public void ListPriceRequestExecuteInvalidCurrencyTypeReturnsEstimate()
    {
      //  Invalid CurrencyType causes DataCache to throw an exception
      var request = new ListPriceRequestData(101, 1, 1, "XXXX");
      var response = (ListPriceResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(response.IsEstimate);
    }

    #endregion
  }
}
