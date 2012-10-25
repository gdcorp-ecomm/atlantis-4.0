using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.WSTService.Impl.WSTServiceWS;
using Atlantis.Framework.WSTService.Interface;

namespace Atlantis.Framework.WSTService.Impl
{
  public class WSTCategoryInfoRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData responseData;

      try
      {
        WSTCategoryInfoRequestData wstCategoryInfoRequestData = (WSTCategoryInfoRequestData)requestData;
        WsConfigElement wsConfig = ((WsConfigElement)configElement);

        using (WSTServiceWS.WSTService wstServiceClient = new WSTServiceWS.WSTService())
        {
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
      }
      catch (Exception ex)
      {
        responseData = new WSTCategoryInfoResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
