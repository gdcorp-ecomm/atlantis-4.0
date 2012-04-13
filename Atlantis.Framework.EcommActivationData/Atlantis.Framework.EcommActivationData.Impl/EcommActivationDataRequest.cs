using System;
using System.Xml;
using Atlantis.Framework.EcommActivationData.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommActivationData.Impl
{
  public class EcommActivationDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommActivationDataResponseData responseData = null;

      try
      {
        string sResponseXML = "";
        XmlDocument responseDoc = new XmlDocument();
        try
        {
          EcommActivationDataRequestData ecommActivationDataRequestData = (EcommActivationDataRequestData)requestData;
          sResponseXML = string.Empty;
          using (gdComActivationSvc.Service1 activationSvc = new gdComActivationSvc.Service1())
          {
            activationSvc.Url = ((WsConfigElement)config).WSURL;
            activationSvc.Timeout = (int)ecommActivationDataRequestData.RequestTimeout.TotalMilliseconds;
            WsConfigElement configElement = (WsConfigElement)config;
            activationSvc.ClientCertificates.Add(configElement.GetClientCertificate("CertificateName"));
            sResponseXML = activationSvc.GetSetupData(requestData.ShopperID, requestData.OrderID);
            responseDoc.LoadXml(sResponseXML);
          }
          if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(requestData,
                                                                 "EcommActivationDataRequest.RequestHandler",
                                                                 sResponseXML,
                                                                 requestData.ToXML());

            responseData = new EcommActivationDataResponseData(sResponseXML, exAtlantis);
          }
          else
          {
            responseData = new EcommActivationDataResponseData(responseDoc);
          }
        }
        catch (AtlantisException exAtlantis)
        {
          responseData = new EcommActivationDataResponseData(sResponseXML, exAtlantis);
        }
        catch (Exception ex)
        {
          responseData = new EcommActivationDataResponseData(requestData, ex);
        }

        return responseData;
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommActivationDataResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommActivationDataResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
