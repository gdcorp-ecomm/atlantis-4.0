using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.StratosphereGetMapUrl.Impl.StratosphereRequestbroker;
using Atlantis.Framework.StratosphereGetMapUrl.Interface;

namespace Atlantis.Framework.StratosphereGetMapUrl.Impl
{
  public class StratosphereGetMapUrlRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      StratosphereGetMapUrlResponseData responseData = null;
      StratosphereRequestbroker.Service stratosphereWS = null;
      string urlResponse = string.Empty;

      try
      {
        string wsURL = ((WsConfigElement)config).WSURL;
        if (!wsURL.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "ExpressCheckoutPurchaseRequest::RequestHandler", "Stratosphere WS URL in atlantis.config must use https.", string.Empty);
        }

        StratosphereGetMapUrlRequestData request = (StratosphereGetMapUrlRequestData)requestData;
        WebServiceResponse wsResponse = null;

        using (stratosphereWS = new Service())
        {
          stratosphereWS.Url = wsURL;
          stratosphereWS.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          request.Certificate.Verify();
          stratosphereWS.ClientCertificates.Add(request.Certificate);
          wsResponse = stratosphereWS.GetMapUrl(request.MapType, request.LookupValue, out urlResponse);
        }

        if (wsResponse!=null && wsResponse.ResultCode == 0)
        {
          responseData = new StratosphereGetMapUrlResponseData(urlResponse);
        }
        else
        {
          string data = string.Format("Error invoking GetMapUrl web method.  Error Code: {0}", wsResponse.ResultCode);
          AtlantisException aex = new AtlantisException(requestData, "StratosphereGetMapUrlRequest::RequestHandler", wsResponse.Error, data);
          responseData = new StratosphereGetMapUrlResponseData(aex);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new StratosphereGetMapUrlResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new StratosphereGetMapUrlResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
