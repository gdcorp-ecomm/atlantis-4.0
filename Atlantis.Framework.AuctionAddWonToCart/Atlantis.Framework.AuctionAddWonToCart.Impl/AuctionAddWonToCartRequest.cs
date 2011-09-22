using System;
using Atlantis.Framework.AuctionAddWonToCart.Impl.AdminUtilsWS;
using Atlantis.Framework.AuctionAddWonToCart.Interface;
using Atlantis.Framework.Interface;

/*
 o	AddItemsIveWonToCart (string shopperId, string idList, string isc, string itc)

    	Where:
 		shopperId – shopperId whose cart the items will be placed in.
 		idList – pipe (|) delimited list of auction ids to add to the cart
 		isc – isc code
    itc - itc code

•	Response:

 Returns   1  if at least one of the items in the idList were successfully added to the cart.  0 if none were added successfully.

 */


namespace Atlantis.Framework.AuctionAddWonToCart.Impl {
  public class AuctionAddWonToCartRequest : IRequest {
    #region Implementation of IRequest

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config) {
      IResponseData responseData;
      trpAdminUtilsService service = null;
      string responseString = string.Empty;

      try
      {
       
        var request = requestData as AuctionAddWonToCartRequestData;

        service = new trpAdminUtilsService();
        service.Url = ((WsConfigElement)config).WSURL;
        if (request != null) {
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          responseString = service.AddItemsIveWonToCartWithITC(request.ShopperID, request.AuctionIds, request.Isc,
                                                            request.Itc);
        }

        responseData = new AuctionAddWonToCartResponseData(responseString);

      } catch (Exception ex) {
        responseData = new AuctionAddWonToCartResponseData(requestData, ex);
      } finally {
        if (service != null) {
          service.Dispose();
        }
      }

      return responseData;
    }
    #endregion
  }
}