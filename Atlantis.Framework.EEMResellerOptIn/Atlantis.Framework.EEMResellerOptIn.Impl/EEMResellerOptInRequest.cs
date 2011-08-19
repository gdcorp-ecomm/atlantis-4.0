using System;
using Atlantis.Framework.EEMResellerOptIn.Impl.EemWs;
using Atlantis.Framework.EEMResellerOptIn.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMResellerOptIn.Impl
{
  public class EEMResellerOptInRequest : IRequest
  {
    private const string EEM_WS_APP_SETTING = "EEM_UNSUB_SVC";

    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EEMResellerOptInResponseData response = null;

      try
      {
        EemWs.CampaignBlazer eem = new CampaignBlazer();
        eem.Url = DataCache.DataCache.GetAppSetting(EEM_WS_APP_SETTING);
        eem.Timeout = (int)((EEMResellerOptInRequestData)requestData).RequestTimeout.TotalMilliseconds;
        eem.ResellerOptIn(requestData.ToXML());
        
        response = new EEMResellerOptInResponseData(true);
      }
      catch (Exception ex)
      {
        response = new EEMResellerOptInResponseData(requestData, ex);
      }

      return response;
    }

    #endregion
  }
}
