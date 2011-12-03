using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ManagerCategories.Interface;

namespace Atlantis.Framework.ManagerCategories.Impl
{
  public class ManagerCategoriesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      try
      {
        ManagerCategoriesRequestData request = requestData as ManagerCategoriesRequestData;

        Dictionary<string, string> managerAttributes;
        List<int> managerCategories;

        DataCache.DataCache.GetMgrCategoriesForUser(request.ManagerUserId, out managerAttributes, out managerCategories);
        result = new ManagerCategoriesResponseData(managerAttributes, managerCategories);

      }
      catch (Exception ex)
      {
        result = new ManagerCategoriesResponseData(requestData, ex);
      }
      return result;
    }
  }
}
