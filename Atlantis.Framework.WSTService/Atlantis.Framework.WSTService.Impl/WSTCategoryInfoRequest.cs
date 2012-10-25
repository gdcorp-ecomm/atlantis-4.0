using System;
using System.Collections.Generic;
using System.ServiceModel;
using Atlantis.Framework.Interface;
using Atlantis.Framework.WSTService.Impl.WSTServiceWS;
using Atlantis.Framework.WSTService.Interface;

namespace Atlantis.Framework.WSTService.Impl
{
  public class WSTCategoryInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      WSTServiceWS.WSTService wstServiceClient = null;
      IResponseData responseData;

      try
      {
        WSTCategoryInfoRequestData wstCategoryInfoRequestData = (WSTCategoryInfoRequestData)oRequestData;
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        wstServiceClient = new WSTServiceWS.WSTService();
        wstServiceClient.Url = wsConfig.WSURL;
        wstServiceClient.Timeout = (int)Math.Truncate(wstCategoryInfoRequestData.RequestTimeout.TotalMilliseconds);

        CategoryInfo[] categoryInfo = wstServiceClient.GetAllWSTCategories();

        Dictionary<int, string> categories = new Dictionary<int, string>();
        foreach (CategoryInfo category in categoryInfo)
        {
          categories.Add(category.CategoryId, category.CategoryName);
        }
        responseData = new WSTCategoryInfoResponseData(categories);
      }
      catch (Exception ex)
      {
        responseData = new WSTCategoryInfoResponseData(oRequestData, ex);
      }
      finally
      {
        if (wstServiceClient != null)
        {
          wstServiceClient.Dispose();
        }
      }

      return responseData;
    }
  }
}
