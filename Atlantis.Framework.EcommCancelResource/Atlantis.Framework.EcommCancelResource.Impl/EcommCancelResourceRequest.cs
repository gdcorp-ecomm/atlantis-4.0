using System;

using Atlantis.Framework.EcommCancelResource.Impl.WscgdCancellation;
using Atlantis.Framework.EcommCancelResource.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCancelResource.Impl
{
  public class EcommCancelResourceRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommCancelResourceResponseData responseData;

      try
      {
        var ecommCancelResourceRequestData = (EcommCancelResourceRequestData) requestData;
        using (var cancellationWS = new wscgdCancellationService())
        {
          cancellationWS.Url = (((WsConfigElement) config).WSURL);
          cancellationWS.Timeout = (int) ecommCancelResourceRequestData.RequestTimeout.TotalMilliseconds;

          string responseXml = cancellationWS.QueueCancelMsg(ecommCancelResourceRequestData.RequestXml);

          responseData = new EcommCancelResourceResponseData(responseXml);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EcommCancelResourceResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EcommCancelResourceResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
