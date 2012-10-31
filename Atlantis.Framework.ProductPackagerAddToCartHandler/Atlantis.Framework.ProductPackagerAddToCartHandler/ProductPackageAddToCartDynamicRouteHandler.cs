using System;
using System.Web;
using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Web.DynamicRouteHandler;

namespace Atlantis.Framework.ProductPackagerAddToCartHandler
{
  public class ProductPackageAddToCartDynamicRouteHandler : DynamicRouteHandlerBase
  {
    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get { return _siteContext = _siteContext ?? HttpProviderContainer.Instance.Resolve<ISiteContext>(); }
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get { return _shopperContext = _shopperContext ?? HttpProviderContainer.Instance.Resolve<IShopperContext>(); }
    }

    private ILinkProvider _linkProvider;
    private ILinkProvider LinkProvider
    {
      get { return _linkProvider = _linkProvider ?? HttpProviderContainer.Instance.Resolve<ILinkProvider>(); }
    }

    private static string ProductGroupId
    {
      get { return HttpContext.Current.Request.Form["product-packager-product-group-id"]; }
    }

    private static int ProductId
    {
      get
      {
        int productId;
        if (!int.TryParse(HttpContext.Current.Request.Form["product-packager-selected-product-id"], out productId))
        {
          productId = 0;
        }
        return productId;
      }
    }

    private static string CartProductPackageId
    {
      get { return HttpContext.Current.Request.Form["product-packager-selected-cart-product-package-id"]; }
    }

    private static string AddOnProductPackageId
    {
      get { return HttpContext.Current.Request.Form["product-packager-selected-addon-product-package-id"]; }
    }

    private static string UpSellProductPackageId
    {
      get { return HttpContext.Current.Request.Form["product-packager-selected-upsell-product-package-id"]; }
    }

    private static string ItemTrackingCode
    {
      get { return HttpContext.Current.Request.Form["product-packager-item-tracking-code"]; }
    }

    private static string FastballDiscountId
    {
      get { return HttpContext.Current.Request.Form["product-packager-fastball-discount-id"]; }
    }

    private static string FastballOfferId
    {
      get { return HttpContext.Current.Request.Form["product-packager-fastball-offer-id"]; }
    }

    private static string FastballOfferUid
    {
      get { return HttpContext.Current.Request.Form["product-packager-fastball-offer-uid"]; }
    }

    private static string FastballOrderDiscountId
    {
      get { return HttpContext.Current.Request.Form["product-packager-fastball-order-discount-id"]; }
    }

    private static string DomainYardValue
    {
      get { return HttpContext.Current.Request.Form["product-packager-domain-yard-value"]; }
    }

    private string OrderDiscountCode
    {
      get
      {
        HttpCookie orderDiscountCookie = HttpContext.Current.Request.Cookies["orderDiscountCode" + SiteContext.PrivateLabelId];
        return orderDiscountCookie != null ? orderDiscountCookie.Value : string.Empty;
      }
    }

    private bool IsAjaxRequest
    {
      get { return HttpContext.Current.Request.Headers["X-Requested-With"] != null && HttpContext.Current.Request.Headers["X-Requested-With"].Contains("XMLHttpRequest"); }
    }

    private string RedirectLocation
    {
      get
      {
        string redirectLinkType = HttpContext.Current.Request.QueryString["redirectlinktype"];
        if(string.IsNullOrEmpty(redirectLinkType))
        {
          redirectLinkType = "CARTURL";
        }

        return LinkProvider.GetUrl(redirectLinkType, string.Empty, QueryParamMode.CommonParameters, true);
      }
    }

    protected override HttpRequestMethodType AllowedRequestMethodTypes
    {
      get { return HttpRequestMethodType.Post; }
    }

    public override string RoutePath
    {
      get { return "productpackager/actions/addtocart"; }
    }

    private void HandleResponse(bool success)
    {
      HttpContext.Current.Response.Clear();

      // No Cache directives
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      HttpContext.Current.Response.Cache.SetNoStore();
      HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

      HttpContext.Current.Response.ContentType = IsAjaxRequest ? "application/json; charset=utf-8" : "text/html; charset=utf-8";
      HttpContext.Current.Response.StatusCode = success ? (IsAjaxRequest ? 200 : 301) : 500;

      if(HttpContext.Current.Response.StatusCode == 301)
      {
        HttpContext.Current.Response.AppendHeader("Location", RedirectLocation);
      }

      HttpContext.Current.Response.End();
    }

    protected override void HandleRequest()
    {
      bool success = false;

      try
      {
        AddItemRequestData requestData = AddItemRequestHelper.CreateAddItemRequest(OrderDiscountCode, FastballOrderDiscountId, DomainYardValue);
        AddItemRequestHelper.AddProductPackagesToRequest(requestData, ProductGroupId, ProductId, CartProductPackageId, AddOnProductPackageId, UpSellProductPackageId, ItemTrackingCode, FastballOfferId, FastballDiscountId, FastballOfferUid);

        AddItemResponseData responseData = (AddItemResponseData)Engine.Engine.ProcessRequest(requestData, 4);
        success = responseData.IsSuccess;
      }
      catch(AtlantisException ex)
      {
        Engine.Engine.LogAtlantisException(ex);
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("Atlantis.Framework.ProductPackagerAddToCartHandler.HandleRequest()", "0", ex.Message, ex.StackTrace, SiteContext, ShopperContext));
      }

      HandleResponse(success);
    }
  }
}
