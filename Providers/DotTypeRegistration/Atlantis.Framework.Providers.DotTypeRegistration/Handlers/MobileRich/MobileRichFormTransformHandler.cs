using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Factories;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileRich
{
  public class MobileRichFormTransformHandler : IDotTypeFormTransformHandler
  {
    public bool TransformFormToHtml(IDotTypeFormsSchema formsSchema, string[] domains, IProviderContainer providerContainer,
                                    out string formSchemaHtml)
    {
      bool result = true;
      formSchemaHtml = string.Empty;
      var sbFormSchemaHtml = new StringBuilder();

      if (formsSchema.Form != null)
      {
        var form = formsSchema.Form;

        var fields = form.FieldCollection;
        foreach (var field in fields)
        {
          var formFieldType = TransformHandlerHelper.GetFormFieldType(field.FieldType);
          if (formFieldType != FormFieldTypes.None)
          {
            if (TransformHandlerHelper.SetFieldTypeData(formFieldType, providerContainer, domains, field))
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
            var exception = new AtlantisException("MobileRichFormTransformHandler.TransformFormToHtml", "0", "Invalid field type", form.FormName, null, null);
            Engine.Engine.LogAtlantisException(exception);

            result = false;
          }
        }

        formSchemaHtml = sbFormSchemaHtml.ToString();
      }
      return result;
    }
  }
}
