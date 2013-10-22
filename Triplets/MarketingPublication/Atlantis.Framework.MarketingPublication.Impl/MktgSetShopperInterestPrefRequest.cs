using Atlantis.Framework.Interface;
using Atlantis.Framework.MarketingPublication.Interface;
using System;

namespace Atlantis.Framework.MarketingPublication.Impl
{
  // Possible atlantis.config entry - remove this before peer review
  // <ConfigElement progid="Atlantis.Framework.MarketingPublication.MktgSetShopperInterestPrefRequest" assembly="Atlantis.Framework.MarketingPublication.dll" request_type="###" />

  public class MktgSetShopperInterestPrefRequest : IRequest
  {
    #region IRequest Members
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        MktgSetShopperInterestPrefRequestData mktgRequest = (MktgSetShopperInterestPrefRequestData)requestData;

        using (PreferencesWS.Service service = new PreferencesWS.Service())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)mktgRequest.RequestTimeout.TotalMilliseconds;

          responseText = service.SetShopperInterestPreference(mktgRequest.ShopperID, mktgRequest.CommPreferenceTypeId, mktgRequest.InterestTypeId, mktgRequest.OptIn);
        }

        result = new MktgSetShopperInterestPrefResponseData(responseText);
      }
      catch (AtlantisException aex)
      {
        result = new MktgSetShopperInterestPrefResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new MktgSetShopperInterestPrefResponseData(requestData, ex);
      }

      return result;
    }
    #endregion

  }
}
