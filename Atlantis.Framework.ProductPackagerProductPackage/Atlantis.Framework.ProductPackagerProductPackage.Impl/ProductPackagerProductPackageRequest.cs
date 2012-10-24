using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Impl;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;
using Atlantis.Framework.ProductPackagerProductPackage.Interface;

namespace Atlantis.Framework.ProductPackagerProductPackage.Impl
{
  public class ProductPackagerProductPackageRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      try
      {
        ProductPackagerProductPackageRequestData productPackageRequestData = (ProductPackagerProductPackageRequestData)requestData;

        if(productPackageRequestData.PackageIds == null)
        {
          throw new Exception("\"PackageIds\" cannot be null.");
        }

        Service productPackageService = ProductPackageServiceHelper.GetServiceInstance(((WsConfigElement)config).WSURL, productPackageRequestData.RequestTimeout);

        ProdPackages productPackageData;
        int returnCode = productPackageService.GetProductPackagesByPackageID(productPackageRequestData.PackageIds.ToArray(), out productPackageData);

        if(returnCode != 0 || productPackageData == null)
        {
          throw new Exception(string.Format("ProductPackageData not returned. Response Code: {0}, Package Ids: {1}", returnCode, string.Join("|", productPackageRequestData.PackageIds)));
        }

        // TODO: In the future allow the IProductPackageDataAdapter to be configured on the config element of the request
        IProductPackagerDataAdapter productPackagerDataAdapter = new FbProductPackagerDataAdapter();
        IDictionary<string, IProductPackageData> transformedProductPackageData = productPackagerDataAdapter.GetProductPackageData(productPackageData);

        if (transformedProductPackageData != null && transformedProductPackageData.Count > 0)
        {
          responseData = new ProductPackagerProductPackageResponseData(transformedProductPackageData);
        }
        else
        {
          throw new Exception(string.Format("IProductPackageData transformation was unsuccessful. Package Group Ids: {0}", string.Join("|", productPackageRequestData.PackageIds)));
        }
      }
      catch (Exception ex)
      {
        responseData = new ProductPackagerProductPackageResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
