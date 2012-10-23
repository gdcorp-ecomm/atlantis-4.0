using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductPackager.Impl;
using Atlantis.Framework.ProductPackager.Impl.ProductPackage;
using Atlantis.Framework.ProductPackager.Interface;
using Atlantis.Framework.ProductPackagerProductGroup.Interface;

namespace Atlantis.Framework.ProductPackagerProductGroup.Impl
{
  public class ProductPackagerProductGroupRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      try
      {
        ProductPackagerProductGroupRequestData productGroupRequestData = (ProductPackagerProductGroupRequestData)requestData;

        if(productGroupRequestData.PackageProductGroupIds == null)
        {
          throw new Exception("\"PackageProductGroupIds\" cannot be null.");
        }

        Service productPackageService = ProductPackageServiceHelper.GetServiceInstance(((WsConfigElement)config).WSURL, productGroupRequestData.RequestTimeout);

        ProductGroupData productGroupData;
        int returnCode = productPackageService.GetProductsByProductGroupID(productGroupRequestData.PackageProductGroupIds.ToArray(), out productGroupData);

        if(returnCode != 0 || productGroupData == null || productGroupData.ProdGroups == null || !productGroupData.ProdGroups.Any())
        {
          throw new Exception(string.Format("ProductGroupData not returned. Response Code: {0}, Package Group Ids: {1}", returnCode, string.Join("|", productGroupRequestData.PackageProductGroupIds)));
        }

        // TODO: In the future allow the IProductPackageDataAdapter to be configured on the config element of the request
        IProductPackagerDataAdapter productPackagerDataAdapter = new FbProductPackagerDataAdapter();
        IList<IProductGroup> transformedProductGroupData = productPackagerDataAdapter.GetProductGroupData(productGroupData);

        if (transformedProductGroupData != null && transformedProductGroupData.Count > 0)
        {
          responseData = new ProductPackagerProductGroupResponseData(transformedProductGroupData);
        }
        else
        {
          throw new Exception(string.Format("IProductGroupData transformation was unsuccessful. Package Group Ids: {0}", string.Join("|", productGroupRequestData.PackageProductGroupIds)));
        }
      }
      catch (Exception ex)
      {
        responseData = new ProductPackagerProductGroupResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
