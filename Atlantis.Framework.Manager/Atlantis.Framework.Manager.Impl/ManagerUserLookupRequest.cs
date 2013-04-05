using System;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;
using Atlantis.Framework.Manager.Interface.ManagerUser;
using gdMiniEncryptLib;

namespace Atlantis.Framework.Manager.Impl
{
  public class ManagerUserLookupRequest : IRequest
  {
    #region IRequest Members
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      const string GET_MANAGER_USER_DATA = "<ManagerUserGetByNTLogin><param name=\"NTLogin\" value=\"{0}\"/></ManagerUserGetByNTLogin>";
      ManagerUserLookupResponseData responseData;

      try
      {
        var request = (ManagerUserLookupRequestData)requestData;
        var managerDataXml = DataCache.DataCache.GetCacheData(string.Format(GET_MANAGER_USER_DATA, request.UserId));
        var managerUser = CreateManagerUser(request, managerDataXml);
        responseData = new ManagerUserLookupResponseData(managerUser);
      }
      catch (Exception ex)
      {
        responseData = new ManagerUserLookupResponseData(requestData, ex);
      }

      return responseData;
    }
    #endregion

    private ManagerUserData CreateManagerUser(ManagerUserLookupRequestData request, string managerDataXml)
    {
      var userId = string.Empty;
      var userIdNum = 0;
      var firstName = string.Empty;
      var lastName = string.Empty;
      var xDoc = XDocument.Parse(managerDataXml);
      var item = xDoc.Descendants("item").FirstOrDefault();
      if (item != null)
      {
        userId = item.Attributes("ManagerUserID").FirstOrDefault() != null ? item.Attributes("ManagerUserID").FirstOrDefault().Value : string.Empty;
        firstName = item.Attributes("FirstName").FirstOrDefault() != null ? item.Attributes("FirstName").FirstOrDefault().Value : string.Empty;
        lastName = item.Attributes("LastName").FirstOrDefault() != null ? item.Attributes("LastName").FirstOrDefault().Value : string.Empty;
        if (!int.TryParse(userId, out userIdNum))
        {
          userIdNum = 0;
        }
      }

      var mgrCategoryReqeust = new ManagerCategoriesRequestData(request.ShopperID, request.SourceURL, request.OrderID, request.Pathway, request.PageCount, userIdNum);
      var mgrCategoryResponse = Engine.Engine.ProcessRequest(mgrCategoryReqeust, request.ManagerCategoriesRequestType) as ManagerCategoriesResponseData;
      var loginName = string.Empty;
      if (mgrCategoryResponse != null)
      {
        mgrCategoryResponse.TryGetManagerAttribute("login_name", out loginName);
      }

      var auth = new Authentication();
      var mstk = auth.GetMgrEncryptedValue(userId, loginName);

      return new ManagerUserData(userId, firstName, lastName, loginName, mstk);
    }
  }
}
