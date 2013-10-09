using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Interface.TLDProductDomainAttributes
{
  internal class AllTLDProducts
  {
    const string PRODUCTELEMENT = "item";

    private readonly Dictionary<int, Dictionary<int, TLDProductTiers>> _dotTypeProductsByRegistryAndPriceTier;
    private readonly Dictionary<int, TLDProductTiers> _dotTypeProductsByRegistry;
    private readonly Dictionary<int, TLDProductTiers> _dotTypeProductsByPriceTier;
    private readonly TLDProductTiers _defaultDotTypeProductTiers;
    private readonly Dictionary<int, TLDProduct> _dotTypeProductsByProductId;

    internal static AllTLDProducts FromXElement(XElement productIdsXml)
    {
      return new AllTLDProducts(productIdsXml);
    }

    private AllTLDProducts(XElement productIdsXml)
    {
      _dotTypeProductsByRegistryAndPriceTier = new Dictionary<int, Dictionary<int, TLDProductTiers>>();
      _dotTypeProductsByPriceTier = new Dictionary<int, TLDProductTiers>();
      _dotTypeProductsByRegistry = new Dictionary<int, TLDProductTiers>();
      _defaultDotTypeProductTiers = new TLDProductTiers();
      _dotTypeProductsByProductId = new Dictionary<int, TLDProduct>();
      LoadDataFromXElement(productIdsXml);
    }

    private void LoadDataFromXElement(XElement productIdsXml)
    {
      foreach (XElement productElement in productIdsXml.Descendants(PRODUCTELEMENT))
      {
        TLDProduct product = TLDProduct.FromXElement(productElement);
        if (product.IsValid)
        {
          ProcessDotTypeProduct(product);
        }
      }
    }

    private void ProcessDotTypeProduct(TLDProduct product)
    {
      Dictionary<int, TLDProductTiers> dotTypeProductsByPriceTier = null;

      if (product.RegistryId != null)
      {
        if (!_dotTypeProductsByRegistryAndPriceTier.TryGetValue((int)product.RegistryId, out dotTypeProductsByPriceTier))
        {
          dotTypeProductsByPriceTier = new Dictionary<int, TLDProductTiers>();
          _dotTypeProductsByRegistryAndPriceTier[(int)product.RegistryId] = dotTypeProductsByPriceTier;
        }
      }
      
      if (dotTypeProductsByPriceTier == null)
      {
        dotTypeProductsByPriceTier = _dotTypeProductsByPriceTier;
      }

      ProcessDotTypeProductIdsByRegistry(product);
      ProcessDotTypeProductIdsByPriceTier(dotTypeProductsByPriceTier, product);
      ProcessDefaultDotTypeProductIds(product);
      _dotTypeProductsByProductId[product.UnifiedProductId] = product;
    }

    private void ProcessDefaultDotTypeProductIds(TLDProduct product)
    {
      ProcessDotTypeTiers(_defaultDotTypeProductTiers, product);
    }

    private void ProcessDotTypeProductIdsByRegistry(TLDProduct product)
    {
      if (product.RegistryId != null)
      {
        TLDProductTiers productTiers;
        if (!_dotTypeProductsByRegistry.TryGetValue((int) product.RegistryId, out productTiers))
        {
          productTiers = new TLDProductTiers {RegistryId = (int) product.RegistryId};
          _dotTypeProductsByRegistry[(int) product.RegistryId] = productTiers;
        }

        if (productTiers != null)
        {
          ProcessDotTypeTiers(productTiers, product);
        }
      }
    }

    private void ProcessDotTypeProductIdsByPriceTier(Dictionary<int, TLDProductTiers> dotTypeProductsByPriceTier, TLDProduct product)
    {
      TLDProductTiers productTiers = null;
      if (product.PriceTierId != null && !dotTypeProductsByPriceTier.TryGetValue((int) product.PriceTierId, out productTiers))
      {
        productTiers = new TLDProductTiers {PriceTierId = (int) product.PriceTierId};

        dotTypeProductsByPriceTier[productTiers.PriceTierId] = productTiers;
        UpdateDotTypeProductsByPriceTierIfNeeded(productTiers);
      }

      if (productTiers != null)
      {
        ProcessDotTypeTiers(productTiers, product);
      }
    }

    private void UpdateDotTypeProductsByPriceTierIfNeeded(TLDProductTiers productTiers)
    {
      if (!_dotTypeProductsByPriceTier.ContainsKey(productTiers.PriceTierId))
      {
        _dotTypeProductsByPriceTier[productTiers.PriceTierId] = productTiers;
      }
    }

    private void ProcessDotTypeTiers(TLDProductTiers productIdTiers, TLDProduct product)
    {
      TLDProductTier tier;
      if (!productIdTiers.TryGetTier(product.MinDomainCount, out tier))
      {
        tier = new TLDProductTier(product.MinDomainCount);
        productIdTiers.AddDotTypeTier(tier);
      }

      tier.AddProduct(product);
    }

    public bool TryGetProductByProductId(int productId, out TLDProduct product)
    {
      return _dotTypeProductsByProductId.TryGetValue(productId, out product);
    }

    public TLDProductTiers GetDefaultProductTiers()
    {
      return _defaultDotTypeProductTiers;
    }

    public TLDProductTiers GetProductTiersByPriceTier(int priceTierId)
    {
      TLDProductTiers result;

      _dotTypeProductsByPriceTier.TryGetValue(priceTierId, out result);

      return result;
    }

    public TLDProductTiers GetProductTiersByRegistry(int registryId)
    {
      TLDProductTiers result;

      _dotTypeProductsByRegistry.TryGetValue(registryId, out result);

      return result;
    }

    public TLDProductTiers GetProductTiersByRegistryAndPriceTier(int registryId, int priceTierId)
    {
      TLDProductTiers result = null;

      if (_dotTypeProductsByRegistryAndPriceTier.ContainsKey(registryId))
      {
        _dotTypeProductsByRegistryAndPriceTier[registryId].TryGetValue(priceTierId, out result);
      }

      return result;
    }
  }
}
