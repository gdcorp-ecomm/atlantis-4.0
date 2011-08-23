using System;
using Atlantis.Framework.CarmaSendInvitation.Impl.CarmaWs;
using Atlantis.Framework.CarmaSendInvitation.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CarmaSendInvitation.Impl
{
  public class CarmaSendInvitationRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CarmaSendInvitationResponseData responseData = null;
      string errors = string.Empty;

      try
      {
        CarmaSendInvitationRequestData request = (CarmaSendInvitationRequestData)requestData;

        using (WSgdCarmaService carmaWs = new WSgdCarmaService())
        {
          carmaWs.Url = ((WsConfigElement)config).WSURL;
          carmaWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          int wsResponse = carmaWs.SendInvitation(request.ShopperID, request.EmailAddress, request.FirstName, request.LastName, out errors);

          if (wsResponse.Equals(0))
          {
            responseData = new CarmaSendInvitationResponseData();
          }
          else
          {
            AtlantisException aex = new AtlantisException(requestData, "CarmaSendInvitationRequest::RequestHandler", "Error setting TrustedShoppers", errors);
            responseData = new CarmaSendInvitationResponseData(aex);
          }
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new CarmaSendInvitationResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CarmaSendInvitationResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
