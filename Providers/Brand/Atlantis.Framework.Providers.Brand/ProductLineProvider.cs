using System;
using System.Collections.Generic;
using Atlantis.Framework.Brand.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;

namespace Atlantis.Framework.Providers.Brand
{
  public class ProductLineProvider : ProviderBase, IProductLineProvider
  {

    private readonly Lazy<ISiteContext> _siteContext;

    public ProductLineProvider(IProviderContainer container) : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
    }

    private Dictionary<string, Dictionary<string, string>> _productLineDict;
    public Dictionary<string, Dictionary<string, string>> ProductLineDict
    {
      get
      {
        if (_productLineDict == null)
        {
          var productLineNameRequestData = new ProductLineNameRequestData(_siteContext.Value.ContextId);
          var productLineNameResponseData =
            (ProductLineNameResponseData)
            Engine.Engine.ProcessRequest(productLineNameRequestData, BrandEngineRequests.ProductLineNameRequestId);

          _productLineDict = productLineNameResponseData.ProductLineNames;
        }

        return _productLineDict;
      }

    }

    public string GetProductLineName(string productLineKey, int overrideDefault = 0)
    {
      string productLineName = String.Empty;

      Dictionary<string, string> productLineValueDict;

      ProductLineDict.TryGetValue(productLineKey, out productLineValueDict);

      if (productLineValueDict != null && productLineValueDict.Count > 0)
      {
        if (overrideDefault != 0)
        {
          productLineValueDict.TryGetValue("override", out productLineName);

          if (String.IsNullOrEmpty(productLineName))
          {
            productLineValueDict.TryGetValue("default", out productLineName);
          }
        }

        else
        {
          productLineValueDict.TryGetValue("default", out productLineName);
        }

      }

      return productLineName ?? String.Empty;
    }
  }
}
