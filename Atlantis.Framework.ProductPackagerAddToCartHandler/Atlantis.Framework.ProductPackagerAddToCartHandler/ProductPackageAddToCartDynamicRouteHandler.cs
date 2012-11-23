using System;
using System.Collections.Generic;
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
    private static IProviderContainer _providerContainer = HttpProviderContainer.Instance;

    public static event AddOrderLevelAttributesDelegate OnAddOrderLevelAttributes;
    public static event AddItemLevelAttributesDelegate OnAddItemLevelAttributesDelegate;

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get { return _siteContext = _siteContext ?? ProviderContainer.Resolve<ISiteContext>(); }
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get { return _shopperContext = _shopperContext ?? ProviderContainer.Resolve<IShopperContext>(); }
    }

    private ILinkProvider _linkProvider;
    private ILinkProvider LinkProvider
    {
      get { return _linkProvider = _linkProvider ?? ProviderContainer.Resolve<ILinkProvider>(); }
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

    private static IList<AddOnSelection> AddOnProductPackages
    {
      get
      {
        string cdsAddOnPackageIds = HttpContext.Current.Request.Form["product-packager-selected-addon-product-packages"];
        string[] cdsAddOnPackageIdsArray = cdsAddOnPackageIds != null ? cdsAddOnPackageIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : new string[0];
        IList<AddOnSelection> cdsAddOnPackageList = new List<AddOnSelection>(cdsAddOnPackageIdsArray.Length);
        foreach (string cdsAddOnPackageValue in cdsAddOnPackageIdsArray)
        {
          cdsAddOnPackageList.Add(new AddOnSelection(cdsAddOnPackageValue));
        }

        return cdsAddOnPackageList;
      }
    }

    private static string UpSellProductPackageId
    {
      get { return HttpContext.Current.Request.Form["product-packager-selected-upsell-product-package-id"]; }
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

    public override DynamicRoutePath RoutePath
    {
      get { return new DynamicRoutePath { Name = "Atlantis.Framework.ProductPackagerAddToCartHandler", Path = "productpackager/actions/addtocart" }; }
    }

    public static IProviderContainer ProviderContainer
    {
      get { return _providerContainer; }
      set { _providerContainer = value; }
    }

    private void HandleResponse(bool success)
    {
      HttpContext.Current.Response.Clear();

      // No Cache directives
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      HttpContext.Current.Response.Cache.SetNoStore();
      HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

      HttpContext.Current.Response.ContentType = IsAjaxRequest ? "application/json; charset=utf-8" : "text/html; charset=utf-8";
      HttpContext.Current.Response.StatusCode = success ? (IsAjaxRequest ? 200 : 301) : 301;

      if(HttpContext.Current.Response.StatusCode == 301)
      {
        HttpContext.Current.Response.AppendHeader("Location", RedirectLocation);
      }

      if(IsAjaxRequest)
      {
        HttpContext.Current.Response.Write("{\"success\":true}");
      }

      HttpContext.Current.Response.End();
    }

    protected override void HandleRequest()
    {
      bool success = false;

      try
      {
        AddItemRequestData requestData = AddItemRequestHelper.CreateAddItemRequest(ProviderContainer, OnAddOrderLevelAttributes);
        AddItemRequestHelper.AddProductPackagesToRequest(ProviderContainer, OnAddItemLevelAttributesDelegate, requestData, ProductGroupId, ProductId, CartProductPackageId, AddOnProductPackages, UpSellProductPackageId);

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
