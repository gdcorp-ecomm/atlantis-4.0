using System.Xml.Linq;
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

    #region IBasketOrderProvider Members

    public bool TryGetBasketOrder(out IBasketOrder basketOrder, string orderId = "")
    {
      var success = false;

      try
      {
        var request = new GetBasketPriceRequestData(_shopperContext.Value.ShopperId,
                                                    HttpContext.Current.Request.Url.ToString(), orderId,
                                                    _siteContext.Value.Pathway, _siteContext.Value.PageCount)
        {
          BasketType = BasketTypes.GoDaddy
        };

        var response =
          (GetBasketPriceResponseData)Engine.Engine.ProcessRequest(request, BasketOrderEngineRequests.BasketGetPrice);
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
        LogException("Error retrieving or initializing BasketOrder", string.Format("Order ID: {0}", orderId ?? "none"),
                     "BasketOrderProvider.TryGetBasketOrder");
        basketOrder = BasketOrder.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    public bool TryGetBasketOrderFromReceiptXml(out IBasketOrder basketOrder, string orderXml)
    {
      var success = false;

      try
      {
        basketOrder = new BasketOrder(orderXml);
        success = true;
      }
      catch (Exception ex)
      {
        LogException("Error initializing BasketOrder from receipt XML (string)", string.Empty,
                     "BasketOrderProvider.TryGetBasketOrderFromReceiptXml");
        basketOrder = BasketOrderTrackingData.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    public bool TryGetBasketOrderFromReceiptXml(out IBasketOrder basketOrder, XDocument orderXml)
    {
      var success = false;

      try
      {
        basketOrder = new BasketOrder(orderXml);
        success = true;
      }
      catch (Exception ex)
      {
        LogException("Error initializing BasketOrder from receipt XML (XDocument)", string.Empty,
                     "BasketOrderProvider.TryGetBasketOrderFromReceiptXml");
        basketOrder = BasketOrderTrackingData.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    public bool TryGetBasketOrderTrackingData(out IBasketOrderTrackingData basketOrder, string orderId = "")
    {
      var success = false;

      var request = new GetBasketPriceRequestData(_shopperContext.Value.ShopperId,
                                                  HttpContext.Current.Request.Url.ToString(), orderId,
                                                  _siteContext.Value.Pathway, _siteContext.Value.PageCount);

      try
      {
        var response = (GetBasketPriceResponseData)Engine.Engine.ProcessRequest(request, BasketOrderEngineRequests.BasketGetPrice);

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
        LogException("Error retrieving or initializing BasketOrderTrackingData", string.Format("Order ID: {0}", orderId ?? "none"),
                     "BasketOrderProvider.TryGetBasketOrderTrackingData");
        basketOrder = BasketOrderTrackingData.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    public bool TryGetBasketOrderTrackingDataFromReceiptXml(out IBasketOrderTrackingData basketOrder, string orderXml)
    {
      var success = false;

      try
      {
        basketOrder = new BasketOrderTrackingData(orderXml);
        success = true;
      }
      catch (Exception ex)
      {
        LogException("Error initializing BasketOrderTrackingData from receipt XML (string)", string.Empty,
                     "BasketOrderProvider.TryGetBasketOrderTrackingDataFromReceiptXml");
        basketOrder = BasketOrderTrackingData.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    public bool TryGetBasketOrderTrackingDataFromReceiptXml(out IBasketOrderTrackingData basketOrder, XDocument orderXml)
    {
      var success = false;

      try
      {
        basketOrder = new BasketOrderTrackingData(orderXml);
        success = true;
      }
      catch (Exception ex)
      {
        LogException("Error initializing BasketOrderTrackingData from receipt XML (XDocument)", string.Empty,
                     "BasketOrderProvider.TryGetBasketOrderTrackingDataFromReceiptXml");
        basketOrder = BasketOrderTrackingData.EmptyBasketOrder;
        success = false;
      }

      return success;
    }

    #endregion
  }
}
