using System;
using Atlantis.Framework.FastballGetOffers.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballGetOffers.Impl
{
  public class FastballGetOffersRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;

      OffersAPIWS.Service offersWS = null;
      try
      {
        FastballGetOffersRequestData requestData = (FastballGetOffersRequestData)oRequestData;
        offersWS = new OffersAPIWS.Service();
        offersWS.Url = ((WsConfigElement)oConfig).WSURL;
        offersWS.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);

        string offersResponse = offersWS.GetOffers(requestData.ChannelRequestXml, requestData.CandidateRequestXml);
        result = new FastballGetOffersResponseData(offersResponse);
      }
      catch (Exception ex)
      {
        result = new FastballGetOffersResponseData(oRequestData, ex);
      }
      finally
      {
        if (offersWS != null)
        {
          offersWS.Dispose();
        }
      }

      return result;
    }

    #endregion
  }
}
