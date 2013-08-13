using Atlantis.Framework.GetBasketPrice.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Web;
using Atlantis.Framework.Providers.BasketOrder.Interface;

namespace Atlantis.Framework.Providers.BasketOrder
{
  public class BasketOrderProvider : ProviderBase, IBasketOrderProvider
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext;

    public BasketOrderProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }

    public bool TryGetBasketOrderTrackingData(string orderId, out IBasketOrderTrackingData basketOrder,
                                              string basketType = null)
    {
      var success = false;

      var request = new GetBasketPriceRequestData(_shopperContext.Value.ShopperId,
                                                  HttpContext.Current.Request.Url.ToString(), string.Empty,
                                                  _siteContext.Value.Pathway, _siteContext.Value.PageCount)
        {
          BasketType = BasketTypes.GoDaddy
        };

      try
      {
        var response =
          (GetBasketPriceResponseData) Engine.Engine.ProcessRequest(request, BasketOrderEngineRequests.BasketGetPrice);
        if (response.IsSuccess)
        {
          var orderXml = response.ToXML();
          basketOrder = new BasketOrderTrackingData(orderXml);
          success = true;
        }
        else
        {
          basketOrder = BasketOrderTrackingData.EmptyBasketOrder;
          success = false;
        }
      }
      catch (Exception ex)
      {
        LogException("Error retrieving or initializing BasketOrderTrackingData", string.Format("Order ID: {0}", orderId),
                     "BasketOrderProvider.TryGetBasketOrderTrackingData");
        basketOrder = BasketOrderTrackingData.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    public bool TryGetBasketOrder(string orderId, out IBasketOrder basketOrder, string basketType = null)
    {
      var success = false;

      try
      {
        var request = new GetBasketPriceRequestData(_shopperContext.Value.ShopperId,
                                                    HttpContext.Current.Request.Url.ToString(), string.Empty,
                                                    _siteContext.Value.Pathway, _siteContext.Value.PageCount)
        {
          BasketType = BasketTypes.GoDaddy
        };

        var response =
          (GetBasketPriceResponseData) Engine.Engine.ProcessRequest(request, BasketOrderEngineRequests.BasketGetPrice);
        if (response.IsSuccess)
        {
          var orderXml = response.ToXML();
          basketOrder = new BasketOrder(orderXml);
          success = true;
        }
        else
        {
          basketOrder = BasketOrder.EmptyBasketOrder;
          success = false;
        }
      }
      catch (Exception ex)
      {
        LogException("Error retrieving or initializing BasketOrder", string.Format("Order ID: {0}", orderId),
                     "BasketOrderProvider.TryGetBasketOrder");
        basketOrder = BasketOrder.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    private void LogException(string message, string data, string sourceFunction)
    {
      try
      {
        Engine.Engine.LogAtlantisException(new AtlantisException(sourceFunction, "0", message, data, null, null));
      }
      catch
      {
      }
    }
  }
}
