using Atlantis.Framework.EcommGiftCardIsValid.Impl.WscGiftCard;
using Atlantis.Framework.EcommGiftCardIsValid.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommGiftCardIsValid.Impl
{
  public class EcommGiftCardIsValidRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      EcommGiftCardIsValidRequestData request = (EcommGiftCardIsValidRequestData)oRequestData;
      EcommGiftCardIsValidResponseData oResponseData = null;
      try
      {
        using (wscGiftCardService oSvc = new wscGiftCardService())
        {
          oSvc.Url = ((WsConfigElement)oConfig).WSURL;
          oSvc.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          string error = string.Empty;
          int resourceId = oSvc.GetGiftCardID(request.ShopperID, request.AccountNumber, out error);
          oResponseData = new EcommGiftCardIsValidResponseData(resourceId, error);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommGiftCardIsValidResponseData(oRequestData, exAtlantis);
      }

      return oResponseData;
    }

    #endregion

  }
}
