using System;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVD.Interface.Helpers;
using Atlantis.Framework.HDVDGetAccountList.Impl.Aries;
using Atlantis.Framework.HDVDGetAccountList.Interface;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetAccountList.Impl
{
  public class HDVDGetAccountListRequest : IRequest
  {

    private const string statusSuccess = "success";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AriesAccountListResponse response = null;
      HDVDGetAccountListResponseData responseData = null;
      HDVDGetAccountListRequestData request = requestData as HDVDGetAccountListRequestData;

      HCCAPIServiceAries service = new HCCAPIServiceAries();
      try
      {
        using (service)
        {

          service.Url = ((WsConfigElement)config).WSURL;
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.GetAccountList(request.ProductType, request.ShopperID, request.PageSize, request.PageNumber,
                                              request.Filter, request.SortField, request.SortOrder);

            if (response != null)
              responseData = new HDVDGetAccountListResponseData(
                HDVDObjectConverter<HDVDAccountListItem>.ConvertAll(response.AccountList, typeof(HDVDAccountListItem)),
                response.ResellerID,
                response.TotalRowCount
                );

          }
        }
      }
      catch (Exception ex)
      {
        responseData = new HDVDGetAccountListResponseData(request, ex);

      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return responseData;
    }


    #endregion
  }
}
