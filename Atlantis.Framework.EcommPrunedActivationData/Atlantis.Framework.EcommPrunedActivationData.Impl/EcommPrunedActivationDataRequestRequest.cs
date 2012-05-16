using System;
using System.Xml;
using Atlantis.Framework.EcommPrunedActivationData.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPrunedActivationData.Impl
{
  public class EcommPrunedActivationDataRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommPrunedActivationDataResponseData responseData = null;

      try
      {
        string sResponseXML = "";
        XmlDocument responseDoc = new XmlDocument();
        try
        {
          EcommPrunedActivationDataRequestData ecommActivationDataRequestData = (EcommPrunedActivationDataRequestData)requestData;
          sResponseXML = string.Empty;
          using (gdComActivationSvc.Service1 activationSvc = new gdComActivationSvc.Service1())
          {
            activationSvc.Url = ((WsConfigElement)config).WSURL;
            activationSvc.Timeout = (int)ecommActivationDataRequestData.RequestTimeout.TotalMilliseconds;
            WsConfigElement configElement = (WsConfigElement)config;
            activationSvc.ClientCertificates.Add(configElement.GetClientCertificate("CertificateName"));
            sResponseXML = activationSvc.GetSetupDataLite(requestData.ShopperID, requestData.OrderID);
            responseDoc.LoadXml(sResponseXML);
          }
          if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(requestData,
                                                                 "EcommPrunedActivationDataRequest.RequestHandler",
                                                                 sResponseXML,
                                                                 requestData.ToXML());

            responseData = new EcommPrunedActivationDataResponseData(sResponseXML, exAtlantis);
          }
          else
          {
            responseData = new EcommPrunedActivationDataResponseData(responseDoc);
          }
        }
        catch (AtlantisException exAtlantis)
        {
          responseData = new EcommPrunedActivationDataResponseData(sResponseXML, exAtlantis);
        }
        catch (Exception ex)
        {
          responseData = new EcommPrunedActivationDataResponseData(requestData, ex);
        }

        return responseData;
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommPrunedActivationDataResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommPrunedActivationDataResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
