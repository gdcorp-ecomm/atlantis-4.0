using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommBillingSync.Interface;

namespace Atlantis.Framework.EcommBillingSync.Impl.Helper_Classes
{
  public class CartUtility
  {
    #region Properties
    public bool IsCartBlocked
    {
      get
      {
        var fosBlockCart = DataCache.DataCache.GetAppSetting("FOS_BLOCK_CART");
        return (fosBlockCart.ToLowerInvariant() == "true");
      }
    }
    #endregion

    #region Cart Add Request
    public AddItemRequestData CreateAddItemRequest(EcommBillingSyncRequestData ebRequest)
    {
      var request = new AddItemRequestData(ebRequest.ShopperID
        , ebRequest.SourceURL
        , ebRequest.OrderID
        , ebRequest.Pathway
        , ebRequest.PageCount
        , ebRequest.ClientIp);

      AddNonEmptyItemAttribute(request, "isc", ebRequest.ItemTrackingCode);
      AddNonEmptyItemAttribute(request, "transactionCurrency", ebRequest.SelectedTransactionalCurrencyType);

      return request;
    }

    private static void AddNonEmptyItemAttribute(AddItemRequestData itemRequest, string name, string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        itemRequest.SetItemRequestAttribute(name, value);
      }
    }
    #endregion

    #region Item Addition
    public void AddToCartRequest(AddItemRequestData request, AddToCartItem item)
    {
      if (!string.IsNullOrEmpty(item.CustomXml))
      {
        request.AddItem(item, item.CustomXml);
      }
      else
      {
        request.AddItem(item);
      }
    }
    #endregion

    #region Post to Cart
    public CartResponse PostToCart(AddItemRequestData request, EcommBillingSyncRequestData ebsRequest)
    {
      CartResponse cartResponse;
      if (IsCartBlocked)
      {
        cartResponse = new CartResponse("86", "Cart is blocked via app setting.");
      }
      else
      {
        var responseData = (AddItemResponseData)Engine.Engine.ProcessRequest(request, ebsRequest.AddItemRequestType);
        cartResponse = new CartResponse(responseData.ToXML());
      }

      if (!cartResponse.Success)
      {
        var errorNumber = "0";
        var errorDescription = string.Empty;

        if (cartResponse.Errors.Count == 0)
        {
          errorDescription = "Unknown error.";
        }
        else
        {
          var first = true;
          foreach (var error in cartResponse.Errors)
          {
            if (first)
            {
              errorNumber = error.Key;
              errorDescription = error.Value;
              first = false;
            }
            else
            {
              errorNumber = errorNumber + "," + error.Key;
              errorDescription = errorDescription + "," + error.Value;
            }
          }

        }

        throw new AtlantisException(ebsRequest
          , "EcommBillingSync.CartUtility::PostToCart"
          , string.Format("ErrorNumber - {0}: {1}", errorNumber, errorDescription)
          , ebsRequest.ToXML());

      }

      return cartResponse;
    }
    #endregion
  }
}
