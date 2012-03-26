using System;
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
        try
        {
          EcommActivationDataRequestData ecommActivationDataRequestData = (EcommActivationDataRequestData)requestData;
          sResponseXML = string.Empty;
          using (gdComActivationSvc.Service1 activationSvc = new gdComActivationSvc.Service1())
          {
            activationSvc.Url = ((WsConfigElement)config).WSURL;
            activationSvc.Timeout = (int)ecommActivationDataRequestData.RequestTimeout.TotalMilliseconds;
            activationSvc.ClientCertificates.Add(requestData.GetCertificateByConfig(config));
            sResponseXML = activationSvc.GetSetupData(requestData.ShopperID, requestData.OrderID);
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
            responseData = new EcommActivationDataResponseData(sResponseXML);
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
