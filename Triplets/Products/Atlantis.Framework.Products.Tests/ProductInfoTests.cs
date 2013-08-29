using Atlantis.Framework.Products.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.Products.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class ProductInfoTests
  {
    [TestMethod]
    public void RequestDataProperties()
    {
      ProductInfoRequestData request = new ProductInfoRequestData(101, 1);
      Assert.AreEqual(1, request.PrivateLabelId);
      Assert.AreEqual(101, request.UnifiedProductId);
      Assert.IsFalse(string.IsNullOrEmpty(request.GetCacheMD5()));
      XElement.Parse(request.ToXML());
    }

    [TestMethod]
    public void RequestDataCacheKeySame()
    {
      ProductInfoRequestData request = new ProductInfoRequestData(101, 1);
      ProductInfoRequestData request2 = new ProductInfoRequestData(101, 1);
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void RequestDataCacheKeyDifferent()
    {
      ProductInfoRequestData request = new ProductInfoRequestData(101, 1);
      ProductInfoRequestData request2 = new ProductInfoRequestData(101, 2);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());

      request = new ProductInfoRequestData(101, 1);
      request2 = new ProductInfoRequestData(102, 1);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());

      request = new ProductInfoRequestData(101, 2);
      request2 = new ProductInfoRequestData(102, 1);
      Assert.AreNotEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void ResponseDataProperties()
    {
      string cacheXml = "<item pf_id=\"101\" description2=\".COM is the bomb!\" name=\".COM Domain Name Registration - 1 Year\" gdshop_product_typeID=\"2\" numberOfPeriods=\"1\" recurring_payment=\"annual\"/>";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);

      Assert.AreEqual(".COM is the bomb!", response.FriendlyDescription);
      Assert.AreEqual(".COM Domain Name Registration - 1 Year", response.Name);
      Assert.AreEqual(2, response.ProductTypeId);
      Assert.AreEqual(1, response.NumberOfPeriods);
      Assert.AreEqual(RecurringPaymentUnitType.Annual, response.RecurringPayment);

      Assert.IsNull(response.GetException());
      XElement.Parse(response.ToXML());
    }

    [TestMethod]
    public void ResponseDataPropertiesNestedXml()
    {
      string cacheXml = "<data><item pf_id=\"101\" gdshop_product_typeID=\"2\" numberOfPeriods=\"1\" recurring_payment=\"annual\"/></data>";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);

      Assert.AreEqual(2, response.ProductTypeId);
      Assert.AreEqual(1, response.NumberOfPeriods);
      Assert.AreEqual(RecurringPaymentUnitType.Annual, response.RecurringPayment);
    }

    [TestMethod]
    public void ResponseDataDefaults()
    {
      string cacheXml = "<item pf_id=\"101\" name=\".COM Domain Name Registration - 1 Year\" />";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);

      Assert.AreEqual(".COM Domain Name Registration - 1 Year", response.FriendlyDescription);
      Assert.AreEqual(".COM Domain Name Registration - 1 Year", response.Name);
      Assert.AreEqual(-1, response.ProductTypeId);
      Assert.AreEqual(0, response.NumberOfPeriods);
      Assert.AreEqual(RecurringPaymentUnitType.Unknown, response.RecurringPayment);
    }

    [TestMethod]
    public void ResponseDataDefaultsNoName()
    {
      string cacheXml = "<item pf_id=\"101\" />";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);

      Assert.AreEqual("101", response.Name);
      Assert.AreEqual(response.Name, response.FriendlyDescription);
    }

    [TestMethod]
    public void ResponseDataMonthly()
    {
      string cacheXml = "<item pf_id=\"101\" recurring_payment=\"monthly\" />";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);
      Assert.AreEqual(RecurringPaymentUnitType.Monthly, response.RecurringPayment);
    }

    [TestMethod]
    public void ResponseDataSemiAnnual()
    {
      string cacheXml = "<item pf_id=\"101\" recurring_payment=\"semiannual\" />";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);
      Assert.AreEqual(RecurringPaymentUnitType.SemiAnnual, response.RecurringPayment);
    }

    [TestMethod]
    public void ResponseDataQuarterly()
    {
      string cacheXml = "<item pf_id=\"101\" recurring_payment=\"quarterly\" />";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);
      Assert.AreEqual(RecurringPaymentUnitType.Quarterly, response.RecurringPayment);
    }

    [TestMethod]
    public void ResponseDataBadXml()
    {
      string cacheXml = "<item pf_id=\"101\" recurring_payment=\"quarterly\" >";
      ProductInfoResponseData response = ProductInfoResponseData.FromCacheData(cacheXml);
      Assert.IsTrue(ReferenceEquals(ProductInfoResponseData.None, response));
    }

    const int _REQUESTTYPE = 702;

    [TestMethod]
    public void RequestBasic()
    {
      ProductInfoRequestData request = new ProductInfoRequestData(101, 1);
      ProductInfoResponseData response = (ProductInfoResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.AreEqual(RecurringPaymentUnitType.Annual, response.RecurringPayment);
    }

    [TestMethod]
    public void RequestException()
    {
      UnifiedProductIdRequestData request = new UnifiedProductIdRequestData(101, 1); // incorrect request type will generate exception
      ProductInfoResponseData response = (ProductInfoResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      Assert.IsTrue(ReferenceEquals(ProductInfoResponseData.None, response));
    }
  }
}
