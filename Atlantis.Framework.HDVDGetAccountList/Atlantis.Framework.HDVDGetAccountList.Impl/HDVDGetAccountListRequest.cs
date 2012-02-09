using System;
using Atlantis.Framework.HDVD.Interface;
using Atlantis.Framework.HDVD.Interface.Aries;
using Atlantis.Framework.HDVD.Interface.Helpers;
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

      HCCAPIServiceAries service = ServiceHelper.GetServiceReference(((WsConfigElement)config).WSURL);

      try 
      {
        using (service)
        {
          if (request != null)
          {
            service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

            response = service.GetAccountList(request.ProductType, request.ShopperID, request.PageSize, request.PageNumber,
                                              request.Filter, request.SortField, request.SortOrder);

            if (response != null)
              responseData = new HDVDGetAccountListResponseData((response as AriesAccountListResponse));

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
