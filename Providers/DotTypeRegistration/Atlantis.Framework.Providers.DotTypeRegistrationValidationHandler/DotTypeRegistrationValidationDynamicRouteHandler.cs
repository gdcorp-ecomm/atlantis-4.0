using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Web.DynamicRouteHandler;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Newtonsoft.Json;

namespace Atlantis.Framework.Providers.DotTypeRegistrationValidationHandler
{
  public class DotTypeRegistrationValidationDynamicRouteHandler : DynamicRouteHandlerBase
  {
    private const string VALIDATE_FIELD_PREFIX = "tui-";
    private static readonly IProviderContainer _providerContainer = HttpProviderContainer.Instance;

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get { return _siteContext = _siteContext ?? _providerContainer.Resolve<ISiteContext>(); }
    }

    private IShopperContext _shopperContext;
    private IShopperContext ShopperContext
    {
      get { return _shopperContext = _shopperContext ?? _providerContainer.Resolve<IShopperContext>(); }
    }

    private IDotTypeRegistrationProvider _dotTypeRegProvider;
    private IDotTypeRegistrationProvider DotTypeRegistrationProvider
    {
      get { return _dotTypeRegProvider = _dotTypeRegProvider ?? _providerContainer.Resolve<IDotTypeRegistrationProvider>(); }
    }

    private static string ClientApplication
    {
      get { return HttpContext.Current.Request.Form[VALIDATE_FIELD_PREFIX + "clientapp"]; }
    }

    private static string Tld
    {
      get { return HttpContext.Current.Request.Form[VALIDATE_FIELD_PREFIX + "tld"]; }
    }

    private static string Phase
    {
      get { return HttpContext.Current.Request.Form[VALIDATE_FIELD_PREFIX + "phase"]; }
    }

    private static Dictionary<string, string> ValidationFields
    {
      get
      {
        var result = new Dictionary<string, string>(8);

        var nvc = HttpContext.Current.Request.Form;
        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new {key = k, value = v});
        foreach (var item in items)
        {
          if (item.key.StartsWith(VALIDATE_FIELD_PREFIX, StringComparison.OrdinalIgnoreCase) &&
              !item.key.Equals(VALIDATE_FIELD_PREFIX + "clientapp", StringComparison.OrdinalIgnoreCase) &&
              !item.key.Equals(VALIDATE_FIELD_PREFIX + "tld", StringComparison.OrdinalIgnoreCase) &&
              !item.key.Equals(VALIDATE_FIELD_PREFIX + "phase", StringComparison.OrdinalIgnoreCase))
          {
            result[item.key.Substring(4)] = item.value;
          }
        }

        return result;
      }
    }

    protected override HttpRequestMethodType AllowedRequestMethodTypes
    {
      get { return HttpRequestMethodType.Post; }
    }

    public override DynamicRoutePath RoutePath
    {
      get
      {
        return new DynamicRoutePath
        {
          Name = "Atlantis.Framework.Providers.DotTypeRegistrationValidationHandler", 
          Path = "dottyperegistration/actions/validate"
        };
      }
    }

    protected override void HandleRequest()
    {
      DotTypeValidationResponseData validationResponse = null;
      var statusCode = 200;

      try
      {
        if (string.IsNullOrEmpty(ClientApplication) || string.IsNullOrEmpty(Tld) || string.IsNullOrEmpty(Phase) || ValidationFields.Count == 0)
        {
          statusCode = 400;
        }

        DotTypeRegistrationProvider.ValidateData(ClientApplication, Tld, Phase, ValidationFields, out validationResponse);
      }
      catch (AtlantisException ex)
      {
        Engine.Engine.LogAtlantisException(ex);
        statusCode = 500;
      }
      catch (Exception ex)
      {
        Engine.Engine.LogAtlantisException(new AtlantisException("Atlantis.Framework.Providers.DotTypeRegistrationValidationHandler.HandleRequest()", "0", ex.Message, ex.StackTrace, SiteContext, ShopperContext));
        statusCode = 500;
      }

      HandleResponse(statusCode, validationResponse);
    }

    private static void HandleResponse(int statusCode, DotTypeValidationResponseData response)
    {
      HttpContext.Current.Response.Clear();

      // No Cache directives
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      HttpContext.Current.Response.Cache.SetNoStore();
      HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

      HttpContext.Current.Response.ContentType = "application/json";
      HttpContext.Current.Response.StatusCode = statusCode;

      var jsonResponse = (response != null && statusCode != 500) ? JsonConvert.SerializeObject(response) : string.Empty;
      HttpContext.Current.Response.Write(jsonResponse);

      HttpContext.Current.Response.End();
    }
  }
}
