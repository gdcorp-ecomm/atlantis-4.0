using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Factories;
using Atlantis.Framework.Providers.DotTypeRegistration.Handlers;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class DotTypeRegistrationProvider : ProviderBase, IDotTypeRegistrationProvider
  {
    public DotTypeRegistrationProvider(IProviderContainer container) : base(container)
    {
    }

    public bool GetDotTypeForms(int tldId, string placement, string phase, string language, out string dotTypeFormsHtml)
    {
      var success = false;
      dotTypeFormsHtml = null;

      try
      {
        var request = new DotTypeFormsHtmlRequestData(tldId, placement, phase, language);
        var response = (DotTypeFormsHtmlResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeFormsHtmlRequest);
        if (response.IsSuccess)
        {
          dotTypeFormsHtml = response.ToXML();
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "tldId: " + tldId + ", placement: " + placement + ", phase: " + phase + ", language: " + language;
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeForms", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetDotTypeFormSchemas(int tldId, string placement, string phase, string language, string[] domains, out IDictionary<string, IList<IFormField>> formFieldsByDomain)
    {
      var success = false;
      formFieldsByDomain = new Dictionary<string, IList<IFormField>>(StringComparer.OrdinalIgnoreCase);

      try
      {
        IDotTypeFormsSchema dotTypeFormSchema;
        success = GetDotTypeFormXmlSchema(tldId, placement, phase, language, out dotTypeFormSchema);

        if (success && dotTypeFormSchema != null)
        {
          success = TransformFormSchemaToFormFields(domains, dotTypeFormSchema, out formFieldsByDomain);
        }
      }
      catch (Exception ex)
      {
        var data = "tldId: " + tldId + ", placement: " + placement + ", phase: " + phase + ", language: " + language;
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeFormSchemas", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    private static bool GetDotTypeFormXmlSchema(int tldId, string placement, string phase, string language, out IDotTypeFormsSchema dotTypeFormSchema)
    {
      var success = false;
      dotTypeFormSchema = null;

      var request = new DotTypeFormsXmlRequestData(tldId, placement, phase, language);

      try
      {
        var response = (DotTypeFormsXmlResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeFormsXmlRequest);
        if (response.IsSuccess)
        {
          dotTypeFormSchema = response.DotTypeFormsSchema;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeFormXmlSchema", "0", ex.Message, request.ToXML(), null, null);
        Engine.Engine.LogAtlantisException(exception);
        success = false;
      }

      return success;
    }

    private bool TransformFormSchemaToFormFields(IEnumerable<string> domains, IDotTypeFormsSchema formSchema, out IDictionary<string, IList<IFormField>> formFieldsByDomain)
    {
      bool success = false;
      formFieldsByDomain = new Dictionary<string, IList<IFormField>>(StringComparer.OrdinalIgnoreCase);

      try
      {
        if (formSchema.Form != null)
        {
          var form = formSchema.Form;
          var fields = form.FieldCollection;

          foreach (var domain in domains)
          {
            IList<IFormField> formFieldsForDomain = new List<IFormField>();
            foreach (var field in fields)
            {
              var formFieldType = TransformHandlerHelper.GetFormFieldType(field.FieldType);
              if (formFieldType != DotTypeFormFieldTypes.None)
              {
                if (TransformHandlerHelper.SetFieldTypeData(formFieldType, Container, domain, field))
                {
                  IDotTypeFormFieldTypeHandler fieldTypeHandler = DotTypeFormFieldTypeFactory.GetFormFieldTypeHandler(formFieldType);
                  if (fieldTypeHandler != null)
                  {
                    IList<IFormField> formFieldList;
                    if (fieldTypeHandler.RenderDotTypeFormField(formFieldType, Container, out formFieldList))
                    {
                      foreach (var formField in formFieldList)
                      {
                        formFieldsForDomain.Add(formField);
                      }
                    }
                  }
                }
              }
              else
              {
                var exception = new AtlantisException("DotTypeRegistrationProvider.TransformFormSchemaToFormFields", "0", "Invalid field type", field.FieldName, null, null);
                Engine.Engine.LogAtlantisException(exception);
              }
            }
            formFieldsByDomain[domain] = formFieldsForDomain;
          }

          if (formFieldsByDomain.Count > 0)
          {
            success = true;
          }
        }
      }
      catch (Exception)
      {
        success = false;
      }

      return success;
    }

    public bool ValidateData(string clientApplication, string serverName, int tldId, string phase, string category, Dictionary<string, string> fields,
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
  }
}
