using System;
using Atlantis.Framework.EcommGiftCardBalance.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardBalance.Impl
{
  public class EcommGiftCardBalanceRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      EcommGiftCardBalanceRequestData getBalance = (EcommGiftCardBalanceRequestData)oRequestData;
      EcommGiftCardBalanceResponseData oResponseData = null;
      WscGiftCard.wscGiftCardService oSvc = null;
      try
      {
        oSvc = new WscGiftCard.wscGiftCardService();
        oSvc.Url = ((WsConfigElement)oConfig).WSURL;
        oSvc.Timeout = (int)Math.Truncate(getBalance.RequestTimeout.TotalMilliseconds);
        int result = oSvc.GetGiftCardBalance(getBalance.AccountNumber, getBalance.OrderID);
        oResponseData = new EcommGiftCardBalanceResponseData(result);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommGiftCardBalanceResponseData(oRequestData, exAtlantis);
      }
      finally
      {
        if (oSvc != null)
        {
          oSvc.Dispose();
        }
      }
      return oResponseData;
    }

    #endregion
  }
}
