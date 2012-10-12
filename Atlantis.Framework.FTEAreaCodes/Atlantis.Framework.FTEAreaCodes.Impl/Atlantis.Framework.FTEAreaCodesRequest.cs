using System;
using System.Net;
using Atlantis.Framework.FTEAreaCodes.Interface;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.FTEAreaCodes.Impl
{
  public class FTEAreaCodesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;

      try
      {
        Properties getAPIProperties = new Properties();
        FTEWebRequest FteApiRequestStates = new FTEWebRequest();
        HttpWebRequest httpWebRequest = null;
        WebResponse webResponse = null;

        string admin = getAPIProperties.RequestToken["admin_user_name"].ToString();
        string password = getAPIProperties.Password;
        string urlRequest = ((WsConfigElement)config).WSURL;

        FteApiRequestStates.GetFTEToken(urlRequest, admin, password, getAPIProperties, out webResponse, out httpWebRequest);

        FteApiRequestStates.StatesAvailable(getAPIProperties, urlRequest, httpWebRequest, webResponse);

        responseData = new FTEAreaCodesResponseData(getAPIProperties.ListedStates);
      }
      catch (AtlantisException aex)
      {
        responseData = new FTEAreaCodesResponseData(requestData, aex);
      }
      catch (Exception ex)
      {
        responseData = new FTEAreaCodesResponseData(requestData, ex);
      }
      return responseData;
    }
  }
}