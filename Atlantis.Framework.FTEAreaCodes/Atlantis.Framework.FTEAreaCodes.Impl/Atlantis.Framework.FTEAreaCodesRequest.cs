using System;
using System.Net;
using System.Xml;
using Atlantis.Framework.FTE.Interface;
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
        var request = (FTEAreaCodesRequestData)requestData;

        Properties getAPIProperties = new Properties();
        FteWebRequest FteApiRequestStates = new FteWebRequest();
        HttpWebRequest httpWebRequest = null;
        WebResponse webResponse = null;

        var nimitzAuthXml = Nimitz.NetConnect.LookupConnectInfo(config, Nimitz.ConnectLookupType.Xml);

        AuthProperties(nimitzAuthXml, getAPIProperties);

        string urlRequest = ((WsConfigElement)config).WSURL;

        FteApiRequestStates.GetFTEToken(urlRequest, getAPIProperties.Admin, getAPIProperties.Password, getAPIProperties, out webResponse, out httpWebRequest);

        FteApiRequestStates.StatesAvailable(getAPIProperties, urlRequest, httpWebRequest, webResponse, request.CcCode);

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

    private void AuthProperties(string nimitzAuthXml, Properties getAPIProperties)
    {

      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(nimitzAuthXml);

      XmlNode authNameNode = xdoc.SelectSingleNode("Connect/UserID");
      XmlNode authPasswordNode = xdoc.SelectSingleNode("Connect/Password");

      string authName = authNameNode.FirstChild.Value;
      string authToken = authPasswordNode.FirstChild.Value;

      getAPIProperties.GetLoginCred(authName, authToken);
    }
  }
}