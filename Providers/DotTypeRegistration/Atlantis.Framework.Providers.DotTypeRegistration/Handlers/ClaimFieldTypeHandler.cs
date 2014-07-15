using System;
using System.Collections.Generic;
using System.Web;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers
{
  public class ClaimFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderDotTypeFormField(DotTypeFormFieldTypes fieldType, IProviderContainer providerContainer, out IList<IFormField> formFields)
    {
      var result = false;
      formFields = new List<IFormField>();

      try
      {
        var additionalData = providerContainer.GetData(FieldTypeDataKeyConstants.CLAIM_DATA_KEY, 
                                                      new Tuple<IDotTypeFormsField, string, int, string, string, string>(new DotTypeFormsField(), string.Empty, 0, string.Empty, string.Empty, string.Empty));
        if (additionalData.Item2.Length > 0)
        {
          var field = additionalData.Item1;
          var domain = additionalData.Item2;
          var tldId = additionalData.Item3;
          var placement = additionalData.Item4;
          var phase = additionalData.Item5;
          var marketId = additionalData.Item6;

          if (tldId > 0)
          {
            var claimResponseData = LoadClaimData(tldId, placement, phase, marketId, domain);

            if (claimResponseData != null && claimResponseData.IsSuccess)
            {
              formFields = ConvertToFormFields(field, domain, claimResponseData);
              result = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string source = "RenderField - ClaimFieldTypeHandler";
        var aex = new AtlantisException(source, 0, message, string.Empty);
        Engine.Engine.LogAtlantisException(aex);

        result = false;
      }

      return result;
    }

    private static IList<IFormField> ConvertToFormFields(IDotTypeFormsField field, string domain, DotTypeClaimsResponseData claimResponse)
    {
      var result = new List<IFormField>();

      if (claimResponse !=null)
      {
        if (!string.IsNullOrEmpty(claimResponse.NoticeXml))
        {
          var formField = new FormField
          {
            Name = string.Format("claimhtml-{0}-{1}", field.FieldName, domain),
            Value = claimResponse.HtmlData,
            DescriptionText = field.FieldDescription,
            Required = field.FieldRequired,
            DefaultValue = field.FieldDefaultValue,
            Type = FormFieldTypes.Label
          };
          result.Add(formField);

          if (HttpContext.Current != null)
          {
            HttpContext.Current.Session[domain] = claimResponse.NoticeXml;
          }
          formField = new FormField { Name = string.Format("claimxml-{0}-{1}", field.FieldName, domain), Type = FormFieldTypes.Hidden };
          result.Add(formField);

          formField = new FormField { Name = "acceptedDate", Value = DateTime.Now.ToUniversalTime().ToString("o"), Type = FormFieldTypes.Hidden };
          result.Add(formField);

          formField = new FormField { Name = "acceptedDate-novalidate", Type = FormFieldTypes.Hidden };
          result.Add(formField);
        }
      }

      return result;
    }

    private static DotTypeClaimsResponseData LoadClaimData(int tldId, string placement, string phase, string marketId, string domain)
    {
      var request = new DotTypeClaimsRequestData( tldId, placement, phase, marketId, domain);
      return (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeClaimsRequest);
    }
  }
}
