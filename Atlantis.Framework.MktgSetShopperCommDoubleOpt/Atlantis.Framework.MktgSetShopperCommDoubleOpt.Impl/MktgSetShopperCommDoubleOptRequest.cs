using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MktgSetShopperCommDoubleOpt.Interface;

namespace Atlantis.Framework.MktgSetShopperCommDoubleOpt.Impl
{
  public class MktgSetShopperCommDoubleOptRequest : IRequest
  {


    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;
      string responseText = string.Empty;

      try
      {
        MktgSetShopperCommDoubleOptRequestData mktgRequest = (MktgSetShopperCommDoubleOptRequestData)requestData;

        PreferencesWS.Service service = new PreferencesWS.Service();
        service.Url = ((WsConfigElement)config).WSURL;
        service.Timeout = (int)mktgRequest.RequestTimeout.TotalMilliseconds;
        responseText = service.SetShopperCommDoubleOptIn(mktgRequest.ShopperID, mktgRequest.CommPreferenceTypeId);
        result = new MktgSetShopperCommDoubleOptResponseData(responseText);
      }
      catch (AtlantisException aex)
      {
        result = new MktgSetShopperCommDoubleOptResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new MktgSetShopperCommDoubleOptResponseData(requestData, ex);
      }

      return result;
    }

    #endregion

  }
}
