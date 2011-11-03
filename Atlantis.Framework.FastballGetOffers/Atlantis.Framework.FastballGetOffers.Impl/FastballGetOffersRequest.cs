using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.FastballGetOffers.Interface;

namespace Atlantis.Framework.FastballGetOffers.Impl
{
  public class FastballGetOffersRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;

      try
      {
        FastballGetOffersRequestData requestData = (FastballGetOffersRequestData)oRequestData;

        OffersAPIWS.Service offersWS = new OffersAPIWS.Service();
        offersWS.Url = ((WsConfigElement)oConfig).WSURL;
        offersWS.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

        string offersResponse = offersWS.GetOffers(requestData.ChannelRequestXml, requestData.CandidateRequestXml);
        result = new FastballGetOffersResponseData(offersResponse);
      }
      catch (Exception ex)
      {
        result = new FastballGetOffersResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion
  }
}
