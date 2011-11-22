using System;
using Atlantis.Framework.EEMGetAuthenticationGuid.Impl.CampaignBlazerWS;
using Atlantis.Framework.EEMGetAuthenticationGuid.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMGetAuthenticationGuid.Impl
{
  public class EEMGetAuthenticationGuidRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EEMGetAuthenticationGuidResponseData responseData = null;

      try
      {
        var request = (EEMGetAuthenticationGuidRequestData)requestData;
        string authGuid = string.Empty;

        using (CampaignBlazer eemWs = new CampaignBlazer())
        {
          eemWs.Url = ((WsConfigElement)config).WSURL;
          eemWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          authGuid = eemWs.GetAuthenticationGuid(request.CustomerId);

          responseData = new EEMGetAuthenticationGuidResponseData(authGuid);
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EEMGetAuthenticationGuidResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EEMGetAuthenticationGuidResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
