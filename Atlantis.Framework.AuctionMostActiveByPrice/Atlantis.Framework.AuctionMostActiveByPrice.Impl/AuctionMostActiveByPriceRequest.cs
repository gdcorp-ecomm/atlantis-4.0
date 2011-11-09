using System;
using Atlantis.Framework.AuctionMostActiveByPrice.Impl.g1dwdnaweb01;
using Atlantis.Framework.AuctionMostActiveByPrice.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionMostActiveByPrice.Impl
{
  public class AuctionMostActiveByPriceRequest : IRequest
  {

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        var request = (AuctionMostActiveByPriceRequestData)requestData;

        using (trpLandingDomainsService service = new trpLandingDomainsService())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          responseXml = service.RetrieveMostActiveByPrice(request.Rows);
        }

        responseData = new AuctionMostActiveByPriceResponseData(responseXml);

      }
      catch (Exception ex)
      {
        responseData = new AuctionMostActiveByPriceResponseData(requestData, ex);
      }

      return responseData;
    }

    #endregion

  }
}
