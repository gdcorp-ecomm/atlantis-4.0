using System;
using Atlantis.Framework.EEMResellerOptOut.Impl.EemWs;
using Atlantis.Framework.EEMResellerOptOut.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMResellerOptOut.Impl
{
  public class EEMResellerOptOutRequest : IRequest 
  {

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EEMResellerOptOutResponseData response = null;

      try
      {
        EEMResellerOptOutRequestData request = (EEMResellerOptOutRequestData)requestData;
        using(EemWs.CampaignBlazer eem = new CampaignBlazer())
        {
          eem.Url = ((WsConfigElement)config).WSURL;
          eem.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          eem.ResellerOptOut(request.ToXML());
        }
        response = new EEMResellerOptOutResponseData(true);
      }
      catch (Exception ex)
      {
        response = new EEMResellerOptOutResponseData(requestData, ex);
      }

      return response;
    }

    #endregion
  }
}
