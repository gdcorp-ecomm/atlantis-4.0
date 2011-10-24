using Atlantis.Framework.GiftCardBalance.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GiftCardBalance.Impl
{
  public class GiftCardBalanceRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GiftCardBalanceRequestData getBalance = (GiftCardBalanceRequestData)oRequestData;
      GiftCardBalanceResponseData oResponseData = null;
      string sResponseXML = string.Empty;
      try
      {
        WscGiftCard.wscGiftCardService oSvc = new WscGiftCard.wscGiftCardService();
        oSvc.Url = ((WsConfigElement)oConfig).WSURL;
        string resultXML = string.Empty;
        int result = oSvc.GetGiftCardBalance(getBalance.AccountNumber,getBalance.OrderID);
        oResponseData = new GiftCardBalanceResponseData(result);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new GiftCardBalanceResponseData(oRequestData, exAtlantis);
      }
      return oResponseData;
    }

    #endregion
  }
}
