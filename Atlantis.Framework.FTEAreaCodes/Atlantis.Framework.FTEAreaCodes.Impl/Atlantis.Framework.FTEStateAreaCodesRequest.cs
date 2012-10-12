using System;
using System.Net;
using Atlantis.Framework.FTEAreaCodes.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FTEAreaCodes.Impl
{
  public class FTEStateAreaCodesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FTEStateAreaCodesResponseData responseData = null;

      try
      {
        var request = (FTEStateAreaCodesRequestData)requestData;

        FTEWebRequest FteApiRequestAreaCodes = new FTEWebRequest();
        Properties getAPIProperties = new Properties();
        HttpWebRequest httpWebRequest = null;
        WebResponse webResponse = null;

        string admin = getAPIProperties.RequestToken["admin_user_name"].ToString();
        string password = getAPIProperties.Password;
        string urlRequest = ((WsConfigElement)config).WSURL;

        FteApiRequestAreaCodes.GetFTEToken(urlRequest, admin, password, getAPIProperties, out webResponse, out httpWebRequest);

        FteApiRequestAreaCodes.StateAreaCodes(getAPIProperties, urlRequest, request.GeoCode, httpWebRequest, webResponse);

        responseData = new FTEStateAreaCodesResponseData(getAPIProperties.AvailableAreaCodes);
      }
      catch (AtlantisException aex)
      {
        responseData = new FTEStateAreaCodesResponseData(requestData, aex);
      }
      catch (Exception ex)
      {
        responseData = new FTEStateAreaCodesResponseData(requestData, ex);
      }
      return responseData;
    }
  }
}
