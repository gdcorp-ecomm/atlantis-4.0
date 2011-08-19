using System;
using Atlantis.Framework.EEMResellerOptOut.Impl.EemWs;
using Atlantis.Framework.EEMResellerOptOut.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMResellerOptOut.Impl
{
  public class EEMResellerOptOutRequest : IRequest 
  {

    private const string EEM_WS_APP_SETTING = "EEM_UNSUB_SVC";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EEMResellerOptOutResponseData response = null;

      try
      {
        EemWs.CampaignBlazer eem = new CampaignBlazer();
        eem.Url = DataCache.DataCache.GetAppSetting(EEM_WS_APP_SETTING);
        eem.Timeout = (int)((EEMResellerOptOutRequestData)requestData).RequestTimeout.TotalMilliseconds;
        eem.ResellerOptOut(requestData.ToXML());

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
