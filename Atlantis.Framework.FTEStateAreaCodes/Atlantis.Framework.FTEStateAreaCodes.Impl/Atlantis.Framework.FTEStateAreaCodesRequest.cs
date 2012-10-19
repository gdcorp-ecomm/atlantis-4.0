using System;
using System.Net;
using System.Xml;
using Atlantis.Framework.FTE.Interface;
using Atlantis.Framework.FTEStateAreaCodes.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FTEStateAreaCodes.Impl
{
  public class FTEStateAreaCodesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      FTEStateAreaCodesResponseData responseData = null;

      try
      {
        var request = (FTEStateAreaCodesRequestData)requestData;

        FteWebRequest FteApiRequestAreaCodes = new FteWebRequest();
        Properties getAPIProperties = new Properties();
        HttpWebRequest httpWebRequest = null;
        WebResponse webResponse = null;

        var nimitzAuthXml = Nimitz.NetConnect.LookupConnectInfo(config, Nimitz.ConnectLookupType.Xml);

        AuthProperties(nimitzAuthXml, getAPIProperties);

        string urlRequest = ((WsConfigElement)config).WSURL;

        FteApiRequestAreaCodes.GetFTEToken(urlRequest, getAPIProperties.Admin, getAPIProperties.Password, getAPIProperties, out webResponse, out httpWebRequest);

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
