using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Interface;
using Atlantis.Framework.ProductPackagerProductGroup.Interface;
using Atlantis.Framework.ProductPackagerProductPackage.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  internal static class ProductPackagerHelper
  {
    private static IProductGroup GetProductGroup(string productGroupId)
    {
      IProductGroup productGroup;

      if (!string.IsNullOrEmpty(productGroupId))
      {
        ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();

        ProductPackagerProductGroupRequestData requestData = new ProductPackagerProductGroupRequestData(shopperContext.ShopperId,
                                                                                                        HttpContext.Current.Request.Url.ToString(),
                                                                                                        string.Empty,
                                                                                                        siteContext.Pathway,
                                                                                                        siteContext.PageCount,
                                                                                                        new List<string> { productGroupId });

        ProductPackagerProductGroupResponseData responseData = (ProductPackagerProductGroupResponseData)DataCache.DataCache.GetProcessRequest(requestData, 609);

        if (!responseData.ProductGroupData.TryGetValue(productGroupId, out productGroup))
        {
          throw new Exception("ProductPackagerProductGroupResponseData ProductGroup \"{0}\" was not found.");
        }
      }
      else
      {
        throw new Exception("ProductPackagerProductGroupResponseData ProductGroupId cannot be null or empty.");
      }

      return productGroup;
    }

    internal static IProductGroupPackageData GetProductGroupPackageData(string productGroupId, int productId)
    {
      IProductGroupPackageData productGroupPackageData;

      IProductGroup productGroup = GetProductGroup(productGroupId);
      if (!productGroup.ProductGroupPackageDataDictionary.TryGetValue(productId, out productGroupPackageData))
      {
        throw new Exception(string.Format("Unable to retreive product package data. ProductId: {0}, ProductGroupId: {1}", productId, productGroupId));
      }

      return productGroupPackageData;
    }

    internal static IProductPackageData GetProductPackage(string packageId)
    {
      IProductPackageData productPackageData;

      if (!string.IsNullOrEmpty(packageId))
      {
        ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();

        ProductPackagerProductPackageRequestData requestData = new ProductPackagerProductPackageRequestData(shopperContext.ShopperId,
                                                                                                            HttpContext.Current.Request.Url.ToString(),
                                                                                                            string.Empty,
                                                                                                            siteContext.Pathway,
                                                                                                            siteContext.PageCount,
                                                                                                            new List<string> { packageId });

        ProductPackagerProductPackageResponseData responseData = (ProductPackagerProductPackageResponseData)DataCache.DataCache.GetProcessRequest(requestData, 610);

        
        if (!responseData.ProductPackageData.TryGetValue(packageId, out productPackageData))
        {
          throw new Exception("ProductPackagerProductPackageResponseData ProductPackageId \"{0}\" was not found.");
        }
      }
      else
      {
        throw new Exception("ProductPackagerProductPackageResponseData ProductPackageId cannot be null or empty.");
      }

      return productPackageData;
    }
  }
}
