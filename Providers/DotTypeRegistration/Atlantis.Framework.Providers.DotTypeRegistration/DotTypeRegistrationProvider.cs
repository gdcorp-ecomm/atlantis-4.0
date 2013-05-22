using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class DotTypeRegistrationProvider : ProviderBase, IDotTypeRegistrationProvider
  {
    private readonly IProviderContainer _container;

    private ISiteContext SiteContext
    {
      get
      {
        return _container.Resolve<ISiteContext>();
      }
    }

    private IShopperContext ShopperContext
    {
      get
      {
        return _container.Resolve<IShopperContext>();
      }
    }

    public DotTypeRegistrationProvider(IProviderContainer container) : base(container)
    {
      _container = container;
    }

    public bool GetDotTypeFormsSchema(int tldId, string placement, out IDotTypeFormsSchema dotTypeFormsSchema)
    {
      var success = false;
      dotTypeFormsSchema = null;

      try
      {
        var request = new DotTypeFormsXmlSchemaRequestData(tldId, placement);
        var response = (DotTypeFormsXmlSchemaResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeFormsXmlRequest);
        if (response.IsSuccess)
        {
          dotTypeFormsSchema = response.DotTypeFormsSchema;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "tldId: " + tldId + ", placement: " + placement;
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeForms", "0", ex.Message + ex.StackTrace, data, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetDotTypeClaims(int tldId, out string claimsXml)
    {
      var success = false;
      claimsXml = string.Empty;

      try
      {
        var request = new DotTypeClaimsRequestData(tldId);
        var response = (DotTypeClaimsResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeClaimsRequest);
        if (response.IsSuccess)
        {
          claimsXml = response.ClaimsXml;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "tldId: " + tldId;
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeClaims", "0", ex.Message + ex.StackTrace, data, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool ValidateDotTypeForms(string clientApplication, string serverName, int tldId, string phase, Dictionary<string, string> fields,
      out bool hasErrors, out Dictionary<string, string> validationErrors, out string token)
    {
      var success = false;
      hasErrors = false;
      token = string.Empty;
      validationErrors = null;

      try
      {
        var request = new DotTypeValidationRequestData(clientApplication, serverName, tldId, phase, fields);
        var response = (DotTypeValidationResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeValidationRequest);
        if (response.IsSuccess)
        {
          if (response.HasErrors)
          {
            hasErrors = true;
            validationErrors = response.ValidationErrors;
          }
          else
          {
            token = response.Token;
          }
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "clientApplcation: " + clientApplication + ", serverName: " + serverName + ", tldId: " + tldId + ", phase: " + phase + ", fields: " + fields;
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeForms", "0", ex.Message + ex.StackTrace, data, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }
  }
}
