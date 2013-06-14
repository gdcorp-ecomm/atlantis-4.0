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

    public bool GetDotTypeFormsSchema(int tldId, string placement, string phase, string language, out IDotTypeFormsSchema dotTypeFormsSchema)
    {
      var success = false;
      dotTypeFormsSchema = null;

      try
      {
        var request = new DotTypeFormsXmlRequestData(tldId, placement, phase, language);
        var response = (DotTypeFormsXmlResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeFormsXmlRequest);
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

    public bool GetClaimsSchema(string[] domains, out IDotTypeClaimsSchema dotTypeClaimsSchema)
    {
      var success = false;
      dotTypeClaimsSchema = null;

      try
      {
        var request = new DotTypeClaimsRequestData(domains);
        var response = (DotTypeClaimsResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeClaimsRequest);
        if (response.IsSuccess)
        {
          dotTypeClaimsSchema = response.DotTypeClaims;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "domains: " + string.Join(",", domains);
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeClaims", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool ValidateTrademarkData(string clientApplication, string serverName, int tldId, string phase, string category, Dictionary<string, string> fields,
      out bool hasErrors, out Dictionary<string, string> validationErrors, out string token)
    {
      var success = false;
      hasErrors = false;
      token = string.Empty;
      validationErrors = null;

      try
      {
        var request = new DotTypeValidationRequestData(clientApplication, serverName, tldId, phase, category, fields);
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
        var data = "clientApplication: " + clientApplication + ", serverName: " + serverName + ", tldId: " + tldId + ", phase: " + phase + ", fields: " + fields;
        var exception = new AtlantisException("DotTypeRegistrationProvider.ValidateDotTypeRegistration", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool ValidateClaimData(string clientApplication, string serverName, int tldId, string phase, string category, string noticeXml,
      out bool hasErrors, out Dictionary<string, string> validationErrors, out string token)
    {
      var success = false;
      hasErrors = false;
      token = string.Empty;
      validationErrors = null;

      try
      {
        var request = new DotTypeValidationRequestData(clientApplication, serverName, tldId, phase, category, noticeXml);
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
        var data = "clientApplication: " + clientApplication + ", serverName: " + serverName + ", tldId: " + tldId + ", phase: " + phase + ", fields: " + noticeXml;
        var exception = new AtlantisException("DotTypeRegistrationProvider.ValidateClaimData", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }
  }
}
