using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class TLDProductDomainAttributesTests
  {
    [TestMethod]
    public void TldProductDomainAttributesRequestWithDataProperties()
    {
      var request = new TLDProductDomainAttributesRequestData(1, "REGREG", 1, 2);
      Assert.AreEqual(1, request.TldId);
      Assert.AreEqual("REGREG", request.TldPhase);
      Assert.AreEqual(1, request.PrivateLabelResellerTypeId);
      Assert.AreEqual(2, request.ProductTypeId);

      var request2 = new TLDProductDomainAttributesRequestData(1, "REGREG", 1, 2);
      Assert.AreEqual("1" + "REGREG" + "1" + "2", request2.GetCacheMD5());
    }

    [TestMethod]
    public void TldProductDomainAttributesRequestToXml()
    {
      var request = new TLDProductDomainAttributesRequestData(1, "REGREG", 1, 2);
      Assert.IsTrue(!string.IsNullOrEmpty(request.ToXML()));
    }

    [TestMethod]
    public void TldProductDomainAttributesRequestWithBadRequestData1()
    {
      try
      {
        var request = new TLDProductDomainAttributesRequestData(1, string.Empty, 1, 2);
        Engine.Engine.ProcessRequest(request, _TLDDOMAINATTRIBUTESREQUEST);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.Message));
      }
    }

    [TestMethod]
    public void TldProductDomainAttributesRequestWithBadRequestData2()
    {
      try
      {
        var request = new TLDProductDomainAttributesRequestData(0, "REGREG", 1, 2);
        Engine.Engine.ProcessRequest(request, _TLDDOMAINATTRIBUTESREQUEST);

      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.Message));
      }
    }

    const int _TLDDOMAINATTRIBUTESREQUEST = 745;

    [TestMethod]
    public void TldProductDomainAttributesRequestWithValidData()
    {
      var request = new TLDProductDomainAttributesRequestData(100, "REGREG", 1, 2);
      var response = (TLDProductDomainAttributesResponseData)Engine.Engine.ProcessRequest(request, _TLDDOMAINATTRIBUTESREQUEST);

      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void TldProductDomainAttributesWithBadRequestData()
    {
      var request = new XData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      try
      {
        Engine.Engine.ProcessRequest(request, _TLDDOMAINATTRIBUTESREQUEST);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.Message));
      }
    }

    private TLDProductDomainAttributesResponseData GetProductIds(int tldId, string tldphase, int privatelabelResellerTypeId, int productTypeId)
    {
      var request = new TLDProductDomainAttributesRequestData(tldId, tldphase, privatelabelResellerTypeId, productTypeId);
      var result = (TLDProductDomainAttributesResponseData)DataCache.DataCache.GetProcessRequest(request, 745);
      return result;
    }

    [TestMethod]
    public void TldProductDomainAttributesProductIdListComRegistration()
    {
      var response = GetProductIds(1, "REGREG", 1, 2);
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void TldProductDomainAttributesProductIdListComBulkRegistration()
    {
      var response = GetProductIds(1, "REGREG", 1, 4);
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void TldProductDomainAttributesGetProductByProductIdCom()
    {
      var response = GetProductIds(1, "REGREG", 1, 2);

      TLDProduct prod;
      if (response.TryGetProductByProductId(101, out prod))
      {
        Assert.AreEqual(prod.UnifiedProductId, 101);
      }
    }

    [TestMethod]
    public void TldProductDomainAttributesProductIdListO2BorgRegistration()
    {
      var response = GetProductIds(1703, "REGREG", 1, 2);
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void TldProductDomainAttributesProductIdListO2BorgBulkRegistration()
    {
      var response = GetProductIds(1703, "REGREG", 1, 4);
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void TldProductDomainAttributesProductIdListGetByTierId()
    {
      var response = GetProductIds(1703, "REGREG", 1, 2);
      Assert.IsNotNull(response.GetProductTiersByPriceTier(3) != null);
    }

    [TestMethod]
    public void TldProductDomainAttributesO2BorgGetProductFromTiers1()
    {
      var response = GetProductIds(1703, "REGREG", 1, 2);

      TLDProductTiers tiers;
      tiers = response.GetProductTiersByPriceTier(1);

      TLDProduct prod;
      if (tiers.TryGetProduct(1, 1, out prod))
      {
        Assert.IsTrue(prod.UnifiedProductId > 0);
      }
    }

    [TestMethod]
    public void TldProductDomainAttributesO2BorgGetProductFromTiers2()
    {
      var response = GetProductIds(1703, "REGREG", 1, 2);

      TLDProductTiers tiers;
      tiers = response.GetProductTiersByPriceTier(1);

      TLDProduct prod;
      if (tiers.TryGetProduct(1, 7, out prod))
      {
        Assert.IsTrue(prod.UnifiedProductId > 0);
      }
    }

    [TestMethod]
    public void TldProductDomainAttributesGetProductListCoUkTest1()
    {
      var response = GetProductIds(259, "REGREG", 1, 2);

      var tiers = response.GetProductTiersByRegistry(1);

      TLDProduct prod;
      if (tiers.TryGetProduct(2, 1, out prod))
      {
        Assert.IsTrue(prod.UnifiedProductId > 0);
      }
    }

    [TestMethod]
    public void TldProductDomainAttributesGetProductListCoUkTest2()
    {
      var response = GetProductIds(259, "REGREG", 1, 2);

      var tiers = response.GetProductTiersByRegistryAndPriceTier(1, 2);

      Assert.IsTrue(tiers == null);
    }

    internal class XData : RequestData
    {
      internal XData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
        : base(shopperId, sourceURL, orderId, pathway, pageCount)
      {

      }

      public override string GetCacheMD5()
      {
        throw new NotImplementedException();
      }
    }
  }
}
