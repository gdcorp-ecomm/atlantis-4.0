using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class DotTypeRegistrationProvider : ProviderBase, IDotTypeRegistrationProvider
  {
    public DotTypeRegistrationProvider(IProviderContainer container) : base(container)
    {
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
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeForms", "0", ex.Message + ex.StackTrace, data, null, null);
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
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeClaims", "0", ex.Message + ex.StackTrace, data, null, null);
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
        var request = new DotTypeFormsValidationRequestData(clientApplication, serverName, tldId, phase, fields);
        var response = (DotTypeFormsValidationResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeValidationRequest);
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
        var exception = new AtlantisException("DotTypeRegistrationProvider.ValidateDotTypeForms", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool ValidateDotTypeClaims(string clientApplication, string serverName, int tldId, string phase, /*Claims notice object*/
      out bool hasErrors, out Dictionary<string, string> validationErrors, out string token)
    {
      var success = false;
      hasErrors = false;
      token = string.Empty;
      validationErrors = null;

      try
      {
        var request = new DotTypeClaimsValidationRequestData(clientApplication, serverName, tldId, phase);
        var response = (DotTypeClaimsValidationResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeValidationRequest);
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
        var data = "clientApplcation: " + clientApplication + ", serverName: " + serverName + ", tldId: " + tldId + ", phase: " + phase;
        var exception = new AtlantisException("DotTypeRegistrationProvider.ValidateDotTypeClaims", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }
  }
}
