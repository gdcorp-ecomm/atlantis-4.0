using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSetShopperCommPref.Interface;

namespace Atlantis.Framework.MktgSetShopperCommPref.Impl
{
  public class MktgSetShopperCommPrefRequest : IRequest
  {
    #region IRequest Members
    
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        MktgSetShopperCommPrefRequestData mktgRequest = (MktgSetShopperCommPrefRequestData)requestData;

        using (PreferencesWS.Service service = new PreferencesWS.Service())
        {
          service.Url = ((WsConfigElement)config).WSURL;
          service.Timeout = (int)mktgRequest.RequestTimeout.TotalMilliseconds;

          responseText = service.SetShopperCommPreference(mktgRequest.ShopperID, mktgRequest.CommPreferenceTypeId, mktgRequest.OptIn);
        }

        result = new MktgSetShopperCommPrefResponseData(responseText);
      }
      catch (AtlantisException aex)
      {
        result = new MktgSetShopperCommPrefResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new MktgSetShopperCommPrefResponseData(requestData, ex);
      }

      return result;
    }

    #endregion

  }
}
