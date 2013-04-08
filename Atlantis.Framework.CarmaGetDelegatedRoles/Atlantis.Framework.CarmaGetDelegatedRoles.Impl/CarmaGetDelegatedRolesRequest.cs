using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CarmaGetDelegatedRoles.Interface;
using Atlantis.Framework.CarmaGetDelegatedRoles.Impl.CarmaWS;

namespace Atlantis.Framework.CarmaGetDelegatedRoles.Impl
{
    public class CarmaGetDelegatedRolesRequest : IRequest
    {
      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        CarmaGetDelegatedRolesResponseData responseData = null;
        string errors = string.Empty;

        try
        {
          CarmaGetDelegatedRolesRequestData request = (CarmaGetDelegatedRolesRequestData)requestData;

          using (WSgdCarmaService carmaWs = new WSgdCarmaService())
          {
            carmaWs.Url = ((WsConfigElement)config).WSURL;
            carmaWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
            string responseXml = carmaWs.GetDelegatedRoles(request.ApplicationId, request.ResourceId, request.ResourceType, request.ShopperID, out errors);

            if (!string.IsNullOrWhiteSpace(responseXml) && string.IsNullOrEmpty(errors))
            {
              responseData = new CarmaGetDelegatedRolesResponseData(responseXml);
            }
            else
            {
              AtlantisException aex = new AtlantisException(requestData, "CarmaGetDelegatedRolesRequest::RequestHandler", "Error calling CarmaGetDelegatedRoles", errors);
              responseData = new CarmaGetDelegatedRolesResponseData(aex);
            }
          }
        }

        catch (AtlantisException exAtlantis)
        {
          responseData = new CarmaGetDelegatedRolesResponseData(exAtlantis);
        }

        catch (Exception ex)
        {
          responseData = new CarmaGetDelegatedRolesResponseData(requestData, ex);
        }

        return responseData;
      }
    }
}
