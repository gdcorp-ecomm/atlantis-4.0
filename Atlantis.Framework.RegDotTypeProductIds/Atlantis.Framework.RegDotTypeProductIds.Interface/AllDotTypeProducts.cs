using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.RegDotTypeProductIds.Interface
{
  internal class AllDotTypeProducts
  {
    const int _MAXYEARS = 10;
    const string _PRODUCTELEMENT = "product";

    private Dictionary<string, Dictionary<DotTypeProductTypes, DotTypeProductTiers>> _dotTypeProductsByRegistry;
    private Dictionary<DotTypeProductTypes, DotTypeProductTiers> _defaultDotTypeProductsByType;
    private Dictionary<int, DotTypeProduct> _dotTypeProductsByProductId;

    internal static AllDotTypeProducts FromXElement(XElement productIdsXml)
    {
      return new AllDotTypeProducts(productIdsXml);
    }

    private AllDotTypeProducts(XElement productIdsXml)
    {
      _dotTypeProductsByRegistry = new Dictionary<string, Dictionary<DotTypeProductTypes, DotTypeProductTiers>>();
      _defaultDotTypeProductsByType = new Dictionary<DotTypeProductTypes, DotTypeProductTiers>();
      _dotTypeProductsByProductId = new Dictionary<int, DotTypeProduct>();
      LoadDataFromXElement(productIdsXml);
    }

    private void LoadDataFromXElement(XElement productIdsXml)
    {
      foreach (XElement productElement in productIdsXml.Descendants(_PRODUCTELEMENT))
      {
        DotTypeProduct product = DotTypeProduct.FromXElement(productElement);
        if (product.IsValid)
        {
          ProcessDotTypeProduct(product);
        }
      }
    }

    private void ProcessDotTypeProduct(DotTypeProduct product)
    {
      Dictionary<DotTypeProductTypes, DotTypeProductTiers> dotTypeProductsByType;

      if (!_dotTypeProductsByRegistry.TryGetValue(product.RegistryId, out dotTypeProductsByType))
      {
        dotTypeProductsByType = new Dictionary<DotTypeProductTypes, DotTypeProductTiers>();
        _dotTypeProductsByRegistry[product.RegistryId] = dotTypeProductsByType;
      }

      ProcessDotTypeProductIdsByType(dotTypeProductsByType, product);
      _dotTypeProductsByProductId[product.ProductId] = product;
    }

    private void ProcessDotTypeProductIdsByType(Dictionary<DotTypeProductTypes, Interface.DotTypeProductTiers> dotTypeProductsByType, DotTypeProduct product)
    {
      DotTypeProductTiers productTiers;
      if (!dotTypeProductsByType.TryGetValue(product.ProductType, out productTiers))
      {
        productTiers = new DotTypeProductTiers(product.ProductType);
        dotTypeProductsByType[productTiers.ProductType] = productTiers;
        UpdateDefaultDotTypeProductIdsIfNeeded(productTiers);
      }

      ProcessDotTypeTiers(productTiers, product);
    }

    private void UpdateDefaultDotTypeProductIdsIfNeeded(DotTypeProductTiers dotTypeProductIds)
    {
      if (!_defaultDotTypeProductsByType.ContainsKey(dotTypeProductIds.ProductType))
      {
        _defaultDotTypeProductsByType[dotTypeProductIds.ProductType] = dotTypeProductIds;
      }
    }

    private void ProcessDotTypeTiers(DotTypeProductTiers productIdTiers, DotTypeProduct product)
    {
      DotTypeProductTier tier;
      if (!productIdTiers.TryGetTier(product.MinDomainCount, out tier))
      {
        tier = new DotTypeProductTier(product.MinDomainCount);
        productIdTiers.AddDotTypeTier(tier);
      }

      tier.AddProduct(product);
    }

    public bool HasProducts(DotTypeProductTypes productType)
    {
      return _defaultDotTypeProductsByType.ContainsKey(productType);
    }

    public DotTypeProductTiers GetDefaultProductTiers(DotTypeProductTypes productType)
    {
      DotTypeProductTiers result;

      if (!_defaultDotTypeProductsByType.TryGetValue(productType, out result))
      {
        result = null;
      }

      return result;
    }

    public DotTypeProductTiers GetProductTiersByRegistryId(string registryId, DotTypeProductTypes productType)
    {
      DotTypeProductTiers result = null;

      if (_dotTypeProductsByRegistry.ContainsKey(registryId))
      {
        if (!_dotTypeProductsByRegistry[registryId].TryGetValue(productType, out result))
        {
          result = null;
        }
      }

      return result;
    }

    public bool TryGetProductByProductId(int productId, out DotTypeProduct product)
    {
      return _dotTypeProductsByProductId.TryGetValue(productId, out product);
    }

  }
}
