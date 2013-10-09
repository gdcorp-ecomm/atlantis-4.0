using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes;

namespace Atlantis.Framework.TLDDataCache.Interface
{
  public class TLDProductDomainAttributesResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _xmlData;
    readonly AllTLDProducts _allProductIds;

    public static TLDProductDomainAttributesResponseData FromException(RequestData requestData, Exception ex)
    {
      return new TLDProductDomainAttributesResponseData(requestData, ex);
    }

    private TLDProductDomainAttributesResponseData(RequestData requestData, Exception ex)
    {
      string message = ex.Message + ex.StackTrace;
      string inputData = requestData.ToXML();
      _exception = new AtlantisException(requestData, "TLDProductDomainAttributesResponseData.ctor", message, inputData);
    }

    public static TLDProductDomainAttributesResponseData FromDataCacheElement(XElement dataCacheElement)
    {
      return new TLDProductDomainAttributesResponseData(dataCacheElement);
    }

    private TLDProductDomainAttributesResponseData(XElement responseXml)
    {
      _xmlData = responseXml.ToString();

      _allProductIds = AllTLDProducts.FromXElement(responseXml);
    }

    public string ToXML()
    {
      string result = "<exception/>";
      if (_xmlData != null)
      {
        result = _xmlData;
      }
      return result;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public bool TryGetProductByProductId(int productId, out TLDProduct product)
    {
      return _allProductIds.TryGetProductByProductId(productId, out product);
    }

    public TLDProductTiers GetDefaultProductTiers()
    {
      return _allProductIds.GetDefaultProductTiers();
    }

    public TLDProductTiers GetProductTiersByPriceTier(int priceTierId)
    {
      return _allProductIds.GetProductTiersByPriceTier(priceTierId);
    }

    public TLDProductTiers GetProductTiersByRegistry(int registryId)
    {
      return _allProductIds.GetProductTiersByRegistry(registryId);
    }

    public TLDProductTiers GetProductTiersByRegistryAndPriceTier(int registryId, int priceTierId)
    {
      return _allProductIds.GetProductTiersByRegistryAndPriceTier(registryId, priceTierId);
    }
  }
}
