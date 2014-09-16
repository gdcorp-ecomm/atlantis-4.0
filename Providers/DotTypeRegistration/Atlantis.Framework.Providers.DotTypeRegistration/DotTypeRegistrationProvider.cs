using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Providers.DomainContactValidation.Interface;
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
      var request = new ValidDotTypesRequestData();
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
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeForms", 0, ex.Message + ex.StackTrace, data);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetDotTypeFormSchemas(IDotTypeFormSchemaLookup dotTypeFormSchemaLookup, string[] domains, out IDotTypeFormFieldsByDomain dotTypeFormFieldsByDomain)
    {
      dotTypeFormFieldsByDomain = null;

      var tld = dotTypeFormSchemaLookup.Tld;
      var placement = dotTypeFormSchemaLookup.Placement;
      var phase = dotTypeFormSchemaLookup.Phase;

      var success = false;
      var language = LocalizationProvider.FullLanguage;
      var tldId = GetTldId(tld);

      try
      {
        IDotTypeFormsSchema dotTypeFormSchema;
        success = GetDotTypeFormXmlSchema(dotTypeFormSchemaLookup, out dotTypeFormSchema);

        if (success && dotTypeFormSchema != null)
        {
          IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain;
          FormItems formItems;
          success = TransformFormSchemaToFormFields(domains, dotTypeFormSchema, tldId, placement, phase, language, out formFieldsByDomain, out formItems);
          if (success)
          {
            var addtlFormFieldsByDomain = AddAdditionalFormFields(domains, dotTypeFormSchemaLookup.Placement, dotTypeFormSchemaLookup.Tld, dotTypeFormSchemaLookup.Phase, dotTypeFormSchemaLookup.FormType, formItems.ValidationLevel);

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

            dotTypeFormFieldsByDomain = new DotTypeFormFieldsByDomain(formFieldsByDomain, formItems);
          }
        }
      }
      catch (Exception ex)
      {
        var data = "tldId: " + tldId + ", placement: " + placement + ", phase: " + phase + ", language: " + language;
        var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeFormSchemas", 0, ex.Message + ex.StackTrace, data);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    private bool GetDotTypeFormXmlSchema(IDotTypeFormSchemaLookup dotTypeFormSchemaLookup, out IDotTypeFormsSchema dotTypeFormSchema)
    {
      var formType = dotTypeFormSchemaLookup.FormType;

      var success = false;
      dotTypeFormSchema = null;

      if (formType.Equals("claims", StringComparison.OrdinalIgnoreCase))
      {
        dotTypeFormSchema = new DotTypeFormsSchema();
        var dotTypeFormsForm = new DotTypeFormsForm();

        var dotTypeFormsField = new DotTypeFormsField
        {
          FieldName = "claims", //This will go as to the RegAppTokenWebSvc as the key. Should not be changed
          FieldType = "claim"   //This is used to resolve the field type handler. Should not be changed
        };
        var fieldCollection = new List<IDotTypeFormsField> {dotTypeFormsField};

        dotTypeFormsForm.FieldCollection = fieldCollection;
        dotTypeFormSchema.Form = dotTypeFormsForm;

        success = true;
      }
      else
      {
        var tld = dotTypeFormSchemaLookup.Tld;
        var placement = dotTypeFormSchemaLookup.Placement;
        var phase = dotTypeFormSchemaLookup.Phase;
        var language = LocalizationProvider.FullLanguage;
        var tldId = GetTldId(tld);

        var request = new DotTypeFormsXmlRequestData(formType, tldId, placement, phase, language, SiteContext.ContextId);
        var domainContactGroup = dotTypeFormSchemaLookup.DomainContactGroup;
        if (domainContactGroup != null)
        {
          var dotTypeFormContacts = GetDotTypeFormContacts(domainContactGroup);
          if (dotTypeFormContacts.Count > 0)
          {
            request.DotTypeFormContacts = dotTypeFormContacts;
          }
        }

        try
        {
          var response =
            (DotTypeFormsXmlResponseData)
              DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeFormsXmlRequest);
          if (response.IsSuccess)
          {
            dotTypeFormSchema = response.DotTypeFormsSchema;
            success = true;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("DotTypeRegistrationProvider.GetDotTypeFormXmlSchema", 0, ex.Message,
            request.ToXML());
          Engine.Engine.LogAtlantisException(exception);
          success = false;
        }
      }
      return success;
    }

    private static List<DotTypeFormContact> GetDotTypeFormContacts(IDomainContactGroup domainContactGroup)
    {
      var dotTypeFormContacts = new List<DotTypeFormContact>();

      var regDotTypeContact = GetDotTypeFormContactByDomainContactType(domainContactGroup, DomainContactType.Registrant);
      if (regDotTypeContact != null)
      {
        dotTypeFormContacts.Add(regDotTypeContact);
      }

      var adminDotTypeContact = GetDotTypeFormContactByDomainContactType(domainContactGroup, DomainContactType.Administrative);
      if (adminDotTypeContact != null)
      {
        dotTypeFormContacts.Add(adminDotTypeContact);
      }

      var billingDotTypeContact = GetDotTypeFormContactByDomainContactType(domainContactGroup, DomainContactType.Billing);
      if (billingDotTypeContact != null)
      {
        dotTypeFormContacts.Add(billingDotTypeContact);
      }

      var technicalDotTypeContact = GetDotTypeFormContactByDomainContactType(domainContactGroup, DomainContactType.Technical);
      if (technicalDotTypeContact != null)
      {
        dotTypeFormContacts.Add(technicalDotTypeContact);
      }

      return dotTypeFormContacts;
    }

    private static DotTypeFormContact GetDotTypeFormContactByDomainContactType(IDomainContactGroup domainContactGroup, DomainContactType contactType)
    {
      DotTypeFormContact result = null;

      var regContact = domainContactGroup.GetContact(contactType);
      if (regContact != null)
      {
        var dotTypeFormContactType = DotTypeFormContactTypes.Registrant;
        bool valid = false;
        switch (contactType)
        {
          case DomainContactType.Registrant:
            dotTypeFormContactType = DotTypeFormContactTypes.Registrant;
            valid = true;
            break;
          case DomainContactType.Administrative:
            dotTypeFormContactType = DotTypeFormContactTypes.Administrative;
            valid = true;
            break;
          case DomainContactType.Technical:
            dotTypeFormContactType = DotTypeFormContactTypes.Technical;
            valid = true;
            break;
          case DomainContactType.Billing:
            dotTypeFormContactType = DotTypeFormContactTypes.Billing;
            valid = true;
            break;
        }

        if (valid)
        {
          result = new DotTypeFormContact(dotTypeFormContactType, regContact.FirstName,
            regContact.LastName, regContact.Company,
            regContact.Address1, regContact.Address2, regContact.City, regContact.State, regContact.Zip,
            regContact.Country, regContact.Phone, regContact.Fax, regContact.Email);
        }
      }

      return result;
    }

    private bool TransformFormSchemaToFormFields(IEnumerable<string> domains, IDotTypeFormsSchema formSchema, int tldId, string placement, string phase, string language,
                                                 out IDictionary<string, IList<IList<IFormField>>> formFieldsByDomain, out FormItems formItems)
    {
      bool success = false;
      formFieldsByDomain = new Dictionary<string, IList<IList<IFormField>>>(StringComparer.OrdinalIgnoreCase);
      formItems = new FormItems();

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
                if (TransformHandlerHelper.SetFieldTypeData(formFieldType, Container, domain, field, tldId, placement, phase, language))
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
                var exception = new AtlantisException("DotTypeRegistrationProvider.TransformFormSchemaToFormFields", 0, "Invalid field type", field.FieldName);
                Engine.Engine.LogAtlantisException(exception);
              }
            }
            formFieldsByDomain[domain] = formFieldsListForDomain;

            formItems.FormDescription = form.FormDescription;
            formItems.FormName = form.FormName;
            formItems.FormType = form.FormType;
            formItems.FormLabel = form.FormLabel;
            formItems.ValidationLevel = form.ValidationLevel ?? "tld";

            if (form.FormFieldCollection != null)
            {
              formItems.FieldCollectionLabel = form.FormFieldCollection.Label;
            }
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
    
    private static Dictionary<string, IList<IList<IFormField>>> AddAdditionalFormFields(IEnumerable<string> domains, string clientApp, string tld, string phase, string formType, string validationLevel)
    {
      var result = new Dictionary<string, IList<IList<IFormField>>>(StringComparer.OrdinalIgnoreCase);

      foreach (var domain in domains)
      {
        var lst = new List<IList<IFormField>>(3)
          {
            GetHiddenFormField("clientapp", clientApp),
            GetHiddenFormField("tld", tld),
            GetHiddenFormField("phase", phase),
            GetHiddenFormField("formtype", formType),
            GetHiddenFormField("validationlevel", validationLevel)
          };

        result[domain] = lst;
      }

      return result;
    }

    private static List<IFormField> GetHiddenFormField(string name, string value)
    {
      return new List<IFormField> {new FormField {Value = value, Type = FormFieldTypes.Hidden, Name = name}};
    }

    public bool DotTypeClaimsExist(IDotTypeFormSchemaLookup dotTypeFormsLookup, string domain)
    {
      bool result = false;

      var tld = dotTypeFormsLookup.Tld;
      var placement = dotTypeFormsLookup.Placement;
      var phase = dotTypeFormsLookup.Phase;

      var fullLanguage = LocalizationProvider.FullLanguage;
      var tldId = GetTldId(tld);

      try
      {
        var request = new DotTypeClaimsRequestData(tldId, placement, phase, fullLanguage, domain);
        var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeClaimsRequest);

        if (response != null)
        {
          if (response.IsSuccess && !string.IsNullOrEmpty(response.NoticeXml))
          {
            result = true;
          }
        }
      }
      catch (Exception ex)
      {
        var data = "tldId: " + tldId + ", placement: " + placement + ", phase: " + phase + ", language: " + fullLanguage + ", domain: " + domain;
        var exception = new AtlantisException("DotTypeRegistrationProvider.DotTypeClaimsExist", 0, ex.Message + ex.StackTrace, data);
        Engine.Engine.LogAtlantisException(exception);
      }

      return result;
    }

    public bool ValidateData(string clientApplication, string tld, string phase, Dictionary<string, IDotTypeValidationFieldValueData> fields,
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
        var exception = new AtlantisException("DotTypeRegistrationProvider.ValidateDotTypeRegistration", 0, ex.Message + ex.StackTrace, data);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }
  }
}
