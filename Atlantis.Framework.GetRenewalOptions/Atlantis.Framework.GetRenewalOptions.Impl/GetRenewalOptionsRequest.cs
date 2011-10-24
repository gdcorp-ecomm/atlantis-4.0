using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Atlantis.Framework.GetRenewalOptions.Impl.BonsaiManager;
using Atlantis.Framework.GetRenewalOptions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetRenewalOptions.Impl
{
  public class GetRenewalOptionsRequest : IRequest
  {
    static XmlWriterSettings settings = GetXmlWriterSettings();

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetRenewalOptionsRequestData requestData = (GetRenewalOptionsRequestData)oRequestData;
      GetRenewalOptionsResponseData responseData = new GetRenewalOptionsResponseData();
      WsConfigElement config = (WsConfigElement)oConfig;

      BonsaiManager.Service manager = new BonsaiManager.Service();
      manager.Url = config.WSURL;

      try
      {
        RenewalResponse response = manager.GetRenewalOptions(requestData.ResourceID, 
                          requestData.ResourceType, requestData.IDType, requestData.PrivateLabelID);
        
        if (response.ResultCode < 0)
        {
          string data = string.Format("Result: {4}, Resource ID: {0}, Resource Type: {1}, IDType: {2}, PrivateLabelID: {3}",
            requestData.ResourceID, requestData.ResourceType, requestData.IDType,
            requestData.PrivateLabelID.ToString(), response.ResultCode.ToString());

          responseData.AtlException = new AtlantisException(oRequestData,
            "GetRenewalOptionsRequest.RequestHandler", "Could not get renewal options", data);
        }
        else
        {
          StringBuilder objectXML = new StringBuilder();
          XmlSerializer serializer = new XmlSerializer(typeof(RenewalResponse));
          serializer.Serialize(XmlWriter.Create(new StringWriter(objectXML), settings), response);
          responseData.XML = objectXML.ToString();
        }
      }
      catch (Exception ex)
      {
        string data = string.Format("Resource ID: {0}, Resource Type: {1}, IDType: {2}, PrivateLabelID: {3}",
            requestData.ResourceID, requestData.ResourceType, requestData.IDType,
            requestData.PrivateLabelID.ToString());

        responseData.AtlException = new AtlantisException(oRequestData,
          "GetRenewalOptionsRequest.RequestHandler", "Could not get renewal options", data, ex);
      }

      return responseData;
    }


    private static XmlWriterSettings GetXmlWriterSettings()
    {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.OmitXmlDeclaration = true;
      settings.Indent = false;
      return settings;
    }
  }
}
