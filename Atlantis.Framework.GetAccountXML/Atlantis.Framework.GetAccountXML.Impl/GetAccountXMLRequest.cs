using System;
using System.Xml;

using Atlantis.Framework.GetAccountXML.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetAccountXML.Impl
{
  public class GetAccountXMLRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetAccountXMLResponseData responseData = new GetAccountXMLResponseData();
      GetAccountXMLRequestData requestData = (GetAccountXMLRequestData)oRequestData;
      WsConfigElement configuration = (WsConfigElement)oConfig;

      try
      {
        XmlNode accountXML;
        int result;

        using (var manager = new BonsaiManager.Service())
        {
          manager.Url = configuration.WSURL;
          manager.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          result = manager.GetAccountXml(requestData.ResourceID, requestData.ResourceType, requestData.IDType,
            requestData.TreeID, requestData.PrivateLabelID, out accountXML); 
        }

        if (result != 0)
        {
          string data = string.Format("Result: {0}, Resource ID: {1}, Resource Type: {2}, IDType: {3}, TreeID: {4}, PrivateLabelID: {5}",
            result, requestData.ResourceID, requestData.ResourceType, requestData.IDType, 
            requestData.TreeID, requestData.PrivateLabelID);

          responseData.AtlException = new AtlantisException(oRequestData,
            "GetAccountXMLRequest.RequestHandler", "Could not retrieve account XML", data);
        }
        else
        {
          responseData.AccountXML = accountXML.OuterXml;
        }
      }
      catch (Exception ex)
      {
        string data = string.Format("Resource ID: {0}, Resource Type: {1}, IDType: {2}, TreeID: {3}, PrivateLabelID: {4}",
            requestData.ResourceID, requestData.ResourceType, requestData.IDType,
            requestData.TreeID, requestData.PrivateLabelID);

        responseData.AtlException = new AtlantisException(oRequestData, 
          "GetAccountXMLRequest.RequestHandler", "Could not retrieve account XML", data, ex);
      }

      return responseData;
    }
  }
}
