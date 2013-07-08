using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Factories;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileRich
{
  public class MobileRichFormTransformHandler : IDotTypeFormTransformHandler
  {
    public bool TransformFormToHtml(IDotTypeFormsSchema formSchema, string[] domains, IProviderContainer providerContainer,
                                    out Dictionary<string,string> formSchemasHtml)
    {
      bool result = false;
      formSchemasHtml = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

      try
      {
        if (formSchema.Form != null)
        {
          var form = formSchema.Form;
          var fields = form.FieldCollection;

          foreach (var domain in domains)
          {
            var sbFormSchemaHtml = new StringBuilder();
            foreach (var field in fields)
            {
              var formFieldType = TransformHandlerHelper.GetFormFieldType(field.FieldType);
              if (formFieldType != FormFieldTypes.None)
              {
                if (TransformHandlerHelper.SetFieldTypeData(formFieldType, providerContainer, domain, field))
                {
                  IDotTypeFormFieldTypeHandler fieldTypeHandler =
                    DotTypeFormFieldTypeFactory.GetFormFieldTypeHandler(ViewTypes.MobileRich, formFieldType);
                  if (fieldTypeHandler != null)
                  {
                    string fieldHtmlData;
                    if (fieldTypeHandler.RenderField(formFieldType, providerContainer, out fieldHtmlData))
                    {
                      if (!string.IsNullOrEmpty(fieldHtmlData))
                      {
                        sbFormSchemaHtml.Append(fieldHtmlData);
                      }
                    }
                  }
                }
              }
              else
              {
                var exception = new AtlantisException("MobileRichFormTransformHandler.TransformFormToHtml", "0", "Invalid field type", field.FieldName, null, null);
                Engine.Engine.LogAtlantisException(exception);
              }
            }
            formSchemasHtml[domain] = sbFormSchemaHtml.ToString();
          }

          if (formSchemasHtml.Count > 0)
          {
            result = true;
          }
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("MobileRichFormTransformHandler.TransformFormToHtml", "0", ex.Message, formSchema.ToString(), null, null);
        Engine.Engine.LogAtlantisException(exception);
        throw;
      }

      return result;
    }
  }
}
