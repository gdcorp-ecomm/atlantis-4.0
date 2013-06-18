using System;
using System.Xml.Linq;
using Atlantis.Framework.EcommPricing.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.EcommPricing.Tests
{
  [TestClass]
  public class ProductIsOnSaleTests
  {
    #region RequestData tests

    [TestMethod]
    public void ProductIsOnSaleRequestDataConstructorGeneratesNewRequestDataObject()
    {
      var request = new ProductIsOnSaleRequestData(0, 1, 1, "USD", 2);
      Assert.IsNotNull(request);
      Assert.AreEqual(2, request.PriceGroupId);
    }

    [TestMethod]
    public void ProductIsOnSaleRequestDataConstructorSetsPriceGroupDefault()
    {
      var request = new ProductIsOnSaleRequestData(0, 1, 1, "USD");
      Assert.AreEqual(0, request.PriceGroupId);
    }

    [TestMethod]
    public void ProductIsOnSaleRequestDataOptionsCommaDelimited()
    {
      var request = new ProductIsOnSaleRequestData(0, 1, 2, "PHP", 8);
      Assert.AreEqual("2,PHP,8", request.Options);
    }

    [TestMethod]
    public void ProductIsOnSaleRequestDataToXml()
    {
      var request = new ProductIsOnSaleRequestData(1111, 2222, 3333, "PHP", 4444);
      string xml = request.ToXML();

      XElement requestRoot = XElement.Parse(xml);
      Assert.AreEqual("ProductIsOnSaleRequestData", requestRoot.Name);
      Assert.IsTrue(requestRoot.Attribute("UnifiedProductId").Value == "1111");
      Assert.IsTrue(requestRoot.Attribute("PrivateLabelId").Value == "2222");
      Assert.IsTrue(requestRoot.Attribute("ShopperPriceType").Value == "3333");
      Assert.IsTrue(requestRoot.Attribute("CurrencyType").Value == "PHP");
      Assert.IsTrue(requestRoot.Attribute("PriceGroupId").Value == "4444");
    }

    #endregion

    #region ResponseData tests

    [TestMethod]
    public void ProductIsOnSaleResponseDataOnSaleReturnsObject()
    {
      var response = ProductIsOnSaleResponseData.OnSale;
      Assert.IsNotNull(response);
      Assert.IsTrue(response.IsOnSale);      
    }

    [TestMethod]
    public void ProductIsOnSaleResponseDataNotOnSaleReturnsObject()
    {
      var response = ProductIsOnSaleResponseData.NotOnSale;
      Assert.IsNotNull(response);
      Assert.IsFalse(response.IsOnSale); 
    }

    [TestMethod]
    public void ProductIsOnSaleResponseDataReturnsValidXml()
    {
      bool isOnSale;
      
      var response = ProductIsOnSaleResponseData.OnSale;
      string xml = response.ToXML();
      XElement element = XElement.Parse(xml);
      Assert.AreEqual("ProductIsOnSaleResponseData", element.Name);
      Assert.IsTrue(bool.TryParse(element.Attribute("isOnSale").Value, out isOnSale));
      Assert.IsTrue(isOnSale);
    }

    #endregion

    #region Request tests

    const int _REQUESTTYPE = 692;

    [TestMethod]
    public void ProductIsOnSaleRequestExecuteValidReturnsFalse()
    {
      var request = new ProductIsOnSaleRequestData(101, 1, 0, "USD", 0);
      var response = (ProductIsOnSaleResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(response.IsOnSale);      
    }

    [TestMethod]
    public void ProductIsOnSaleRequestExecuteInvalidReturnsFalse()
    {
      var request = new ProductIsOnSaleRequestData(-101, 1, 1, "USD", 2);
      var response = (ProductIsOnSaleResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(response.IsOnSale);
    }

    [TestMethod]
    public void ProductIsOnSaleRequestExecuteExceptionReturnsFalse()
    {
      //  Invalid CurrencyType causes DataCache to throw an exception
      var request = new ProductIsOnSaleRequestData(101, 1, 1, "XXXX");
      var response = (ProductIsOnSaleResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsFalse(response.IsOnSale);
    }

    #endregion
  }
}
