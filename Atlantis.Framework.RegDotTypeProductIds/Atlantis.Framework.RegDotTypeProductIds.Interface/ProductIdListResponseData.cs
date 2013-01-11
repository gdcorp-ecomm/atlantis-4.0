using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
  public class ProductIdListResponseData : IResponseData
  {
    AtlantisException _exception = null;
    XElement _responseXml = null;
    AllDotTypeProducts _allProductIds;

    private ProductIdListResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    private ProductIdListResponseData(XElement responseElement)
    {
      _responseXml = responseElement;

      XAttribute processingResult = _responseXml.Attribute("processing");
      if (!"success".Equals(processingResult.Value, StringComparison.OrdinalIgnoreCase))
      {
        throw new ArgumentException("Processing result was not 'success'.");
      }

      _allProductIds = AllDotTypeProducts.FromXElement(_responseXml);
    }

    public string ToXML()
    {
      return _responseXml.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public DotTypeProductTiers GetProductTiersForRegistry(string registryId, DotTypeProductTypes productType)
    {
      return _allProductIds.GetProductTiersByRegistryId(registryId, productType);
    }

    public DotTypeProductTiers GetDefaultProductTiers(DotTypeProductTypes productType)
    {
      return _allProductIds.GetDefaultProductTiers(productType);
    }

    public bool HasProducts(DotTypeProductTypes productType)
    {
      return _allProductIds.HasProducts(productType);
    }

    public bool TryGetProductByProductId(int productId, out DotTypeProduct product)
    {
      return _allProductIds.TryGetProductByProductId(productId, out product);
    }

    public static IResponseData FromXElement(XElement responseElement)
    {
      return new ProductIdListResponseData(responseElement);
    }

    public static IResponseData FromException(AtlantisException exception)
    {
      return new ProductIdListResponseData(exception);
    }
  }
}
