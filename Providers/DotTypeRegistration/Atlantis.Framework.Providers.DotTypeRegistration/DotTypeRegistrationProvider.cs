using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Factories;
using Atlantis.Framework.Providers.DotTypeRegistration.Handlers;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class DotTypeRegistrationProvider : ProviderBase, IDotTypeRegistrationProvider
  {
    private readonly Lazy<ValidDotTypesResponseData> _validDotTypes;

    public DotTypeRegistrationProvider(IProviderContainer container) : base(container)
    {
      _validDotTypes = new Lazy<ValidDotTypesResponseData>(LoadValidDotTypes);
    }

    private ISiteContext _siteContext;
    private ISiteContext SiteContext
    {
      get { return _siteContext ?? (_siteContext = Container.Resolve<ISiteContext>()); }
    }

    private ILocalizationProvider _localizationProvider;
    private ILocalizationProvider LocalizationProvider
    {
      get { return _localizationProvider ?? (_localizationProvider = Container.Resolve<ILocalizationProvider>()); }
    }

    private static ValidDotTypesResponseData LoadValidDotTypes()
    {
      var request = new ValidDotTypesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      return (ValidDotTypesResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.ValidDotTypesRequest);
    }

    private int GetTldId(string tld)
    {
      int tldId;
      _validDotTypes.Value.TryGetTldId(tld, out tldId);

      return tldId;
    }

    public bool GetDotTypeForms(IDotTypeFormLookup dotTypeFormsLookup, out string dotTypeFormsHtml)
    {
      var tld = dotTypeFormsLookup.Tld;
      var formType = dotTypeFormsLookup.FormType;
      var placement = dotTypeFormsLookup.Placement;
      var phase = dotTypeFormsLookup.Phase;
      var domain = dotTypeFormsLookup.Domain;

      var success = false;
      dotTypeFormsHtml = null;
      var language = LocalizationProvider.FullLanguage;
      var tldId = GetTldId(tld);

      try
      {
        var request = new DotTypeFormsHtmlRequestData(formType, tldId, placement, phase, language, SiteContext.ContextId, domain);

        var response = (DotTypeFormsHtmlResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeFormsHtmlRequest);
        if (response.IsSuccess)
        {
          dotTypeFormsHtml = response.ToXML();
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "tldId: " + tldId + ", placement: " + placement + ", phase: " + phase + ", language: " + language + ", domain: " + domain;
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeForms", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetDotTypeFormSchemas(IDotTypeFormLookup dotTypeFormsLookup, string[] domains, out IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain)
    {
      dotTypeFormFieldsByDomain = null;

      var tld = dotTypeFormsLookup.Tld;
      var formType = dotTypeFormsLookup.FormType;
      var placement = dotTypeFormsLookup.Placement;
      var phase = dotTypeFormsLookup.Phase;

      var success = false;
      var language = LocalizationProvider.FullLanguage;
      var tldId = GetTldId(tld);

      try
      {
        IDotTypeFormsSchema dotTypeFormSchema;
        success = GetDotTypeFormXmlSchema(formType, tldId, placement, phase, language, out dotTypeFormSchema);

        if (success && dotTypeFormSchema != null)
        {
          IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain;
          success = TransformFormSchemaToFormFields(domains, dotTypeFormSchema, out formFieldsByDomain);
          if (success)
          {
            var addtlFormFieldsByDomain = AddAdditionalFormFields(domains, dotTypeFormsLookup.Placement, dotTypeFormsLookup.Tld, dotTypeFormsLookup.Phase);

            foreach (var main in formFieldsByDomain)
            {
              foreach (var addtl in addtlFormFieldsByDomain)
              {
                if (addtl.Key.Equals(main.Key))
                {
                  foreach (var formFieldList in addtl.Value)
                  {
                    main.Value.Add(formFieldList);
                  }
                }
              }
            }

            dotTypeFormFieldsByDomain = new DotTypeFormFieldsByDomain(formFieldsByDomain);
          }
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

    private bool GetDotTypeFormXmlSchema(string formType, int tldId, string placement, string phase, string language, out IDotTypeFormsSchema dotTypeFormSchema)
    {
      var success = false;
      dotTypeFormSchema = null;

      var request = new DotTypeFormsXmlRequestData(formType, tldId, placement, phase, language, SiteContext.ContextId);

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

    private bool TransformFormSchemaToFormFields(IEnumerable<string> domains, IDotTypeFormsSchema formSchema, out IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain)
    {
      bool success = false;
      formFieldsByDomain = new Dictionary<string, IList<IList<IFormField>>>(StringComparer.OrdinalIgnoreCase);

      try
      {
        if (formSchema.Form != null)
        {
          var form = formSchema.Form;
          var fields = form.FieldCollection;

          foreach (var domain in domains)
          {
            var formFieldsListForDomain = new List<IList<IFormField>>();
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
                      if (formFieldList.Count > 0)
                      {
                        formFieldsListForDomain.Add(formFieldList);
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
            formFieldsByDomain[domain] = formFieldsListForDomain;
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

    private static Dictionary<string, IList<IList<IFormField>>> AddAdditionalFormFields(IEnumerable<string> domains, string clientApp, string tld, string phase)
    {
      var result = new Dictionary<string, IList<IList<IFormField>>>(StringComparer.OrdinalIgnoreCase);

      foreach (var domain in domains)
      {
        var lst = new List<IList<IFormField>>(3)
        {
          GetHiddenFormField("clientapp", clientApp),
          GetHiddenFormField("tld", tld),
          GetHiddenFormField("phase", phase)
        };

        result[domain] = lst;
      }

      return result;
    }

    private static List<IFormField> GetHiddenFormField(string name, string value)
    {
      return new List<IFormField> {new FormField {Value = value, Type = FormFieldTypes.Hidden, Name = name}};
    }

    public bool ValidateData(string clientApplication, string tld, string phase, Dictionary<string, string> fields,
      out DotTypeValidationResponseData validationResponseData)
    {
      var success = false;
      validationResponseData = null;
      var serverName = Environment.MachineName;
      const string category = "apptoken";
      var tldId = GetTldId(tld);

      try
      {
        var request = new DotTypeValidationRequestData(clientApplication, serverName, tldId, phase, category, fields);
        validationResponseData = (DotTypeValidationResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeValidationRequest);

        success = validationResponseData != null && validationResponseData.IsSuccess;
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
