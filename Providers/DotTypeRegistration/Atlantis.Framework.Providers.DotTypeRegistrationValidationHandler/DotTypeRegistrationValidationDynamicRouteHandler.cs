using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Atlantis.Framework.DotTypeValidation.Impl;
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
    private const string VALIDATE_CLAIMXML_FIELD_PREFIX = "tui-claimxml-";
    private const string NO_VALIDATE_FIELD_POSTFIX = "-novalidate";

    private static readonly IProviderContainer _providerContainer = HttpProviderContainer.Instance;

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

    private static Dictionary<string, IDotTypeValidationFieldValueData> ValidationFields
    {
      get
      {
        var result = new Dictionary<string, IDotTypeValidationFieldValueData>(8);

        var formVars = HttpContext.Current.Request.Form;
        var items = formVars.AllKeys.SelectMany(formVars.GetValues, (k, v) => new {key = k, value = v});
        foreach (var item in items)
        {
          if (item.key.StartsWith(VALIDATE_FIELD_PREFIX, StringComparison.OrdinalIgnoreCase) &&
              !item.key.EndsWith(NO_VALIDATE_FIELD_POSTFIX, StringComparison.OrdinalIgnoreCase) &&
              !item.key.Equals(VALIDATE_FIELD_PREFIX + "clientapp", StringComparison.OrdinalIgnoreCase) &&
              !item.key.Equals(VALIDATE_FIELD_PREFIX + "tld", StringComparison.OrdinalIgnoreCase) &&
              !item.key.Equals(VALIDATE_FIELD_PREFIX + "phase", StringComparison.OrdinalIgnoreCase))
          {
            if (item.key.StartsWith(VALIDATE_CLAIMXML_FIELD_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
              var itemArray = item.key.Split(new[] {'-'}, 4, StringSplitOptions.RemoveEmptyEntries);
              if (itemArray.Length == 4)
              {
                if (HttpContext.Current.Session != null)
                {
                  var value = HttpContext.Current.Session[itemArray[3]] as string;
                  result[itemArray[2]] = GetValidationFieldValueData(item.key, formVars, value);
                }
              }
            }
            else
            {
              result[item.key.Substring(4)] = GetValidationFieldValueData(item.key, formVars, item.value);
            }
          }
        }

        return result;
      }
    }

    private static IDotTypeValidationFieldValueData GetValidationFieldValueData(string fieldName, NameValueCollection formVariables, string value)
    {
      IDotTypeValidationFieldValueData result = DotTypeValidationFieldValueData.Create(value);

      var noValidateFieldName = fieldName + NO_VALIDATE_FIELD_POSTFIX;
      foreach (string key in formVariables.Keys)
      {
        if (key.Equals(noValidateFieldName))
        {
          result.NoValidate = true;
          break;
        }
      }

      return result;
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
        var description = string.Concat("A.F.Providers.DotTypeRegistrationValidationHandler.HandleRequest()", ex.Message, ex.StackTrace);
        var data = string.Format("Tld: {0} | Phase: {1}", Tld, Phase);
        Engine.Engine.LogAtlantisException(new AtlantisException("DotTypeRegistrationValidationHandler.HandleRequest", 0, description, data));
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
