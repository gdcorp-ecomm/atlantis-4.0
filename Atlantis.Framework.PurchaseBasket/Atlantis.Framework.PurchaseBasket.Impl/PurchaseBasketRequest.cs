using Atlantis.Framework.Interface;
using Atlantis.Framework.PurchaseBasket.Interface;

namespace Atlantis.Framework.PurchaseBasket.Impl
{
  public class PurchaseBasketRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      PurchaseBasketResponseData oResponseData = null;
      PurchaseBasketRequestData oPurchaseBasketRequestData = (PurchaseBasketRequestData)oRequestData;
      string sResponseXML = "";
      object vOut=null;
      gdBasketHelperLib.WsWrapperClass oBasketHelper = null;
      try
      {
        oBasketHelper = new gdBasketHelperLib.WsWrapperClass();
        object result = oBasketHelper.PurchaseBasketEx(oPurchaseBasketRequestData.ShopperID, oPurchaseBasketRequestData.ToXML(), out vOut) as string;
        sResponseXML = vOut as string;
        oResponseData = new PurchaseBasketResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        sResponseXML = vOut as string;
        oResponseData = new PurchaseBasketResponseData(sResponseXML, exAtlantis);
      }
      catch (System.Exception ex)
      {
        sResponseXML = vOut as string;
        oResponseData = new PurchaseBasketResponseData(sResponseXML, oRequestData, ex);
      }
      finally
      {
        System.Runtime.InteropServices.Marshal.ReleaseComObject(oBasketHelper);
      }
      return oResponseData;

    }

    #endregion
  }
}
