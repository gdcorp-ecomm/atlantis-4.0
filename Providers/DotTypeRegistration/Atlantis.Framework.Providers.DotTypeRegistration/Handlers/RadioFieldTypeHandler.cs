using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers
{
  public class RadioFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderDotTypeFormField(DotTypeFormFieldTypes fieldType, IProviderContainer providerContainer, out IList<IFormField> formFields)
    {
      var result = false;
      formFields = new List<IFormField>();

      try
      {
        var field = providerContainer.GetData<IDotTypeFormsField>(FieldTypeDataKeyConstants.RADIO_DATA_KEY, null);
        if (field != null)
        {
          formFields = ConvertToFormFields(field);
          result = true;
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string SOURCE = "RenderField - MobileRichRadioFieldTypeHandler";
        var aex = new AtlantisException(SOURCE, "0", message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);

        result = false;
      }

      return result;
    }

    private static IList<IFormField> ConvertToFormFields(IDotTypeFormsField field)
    {
      IList<IFormField> result = new List<IFormField>();

      var formField = new FormField { Name = field.FieldName, LabelText = field.FieldLabel, Type = FormFieldTypes.Radio, ItemCollection = field.ItemCollection };
      result.Add(formField);

      return result;
    }

    //private static string ConvertToHtml(IDotTypeFormsField field)
    //{
    //  var result = new StringBuilder();

    //  if (field.ItemCollection.Count > 0)
    //  {
    //    result.Append("<div class='section-row groove orient-vert'>");
    //    foreach (var item in field.ItemCollection)
    //    {
    //      result.Append("<div>");
    //      result.Append("<input type='radio' name='" + field.FieldName + "' value='" + item.ItemId + ">'</input>");
    //      result.Append("<label class='font-sm'>" + field.FieldLabel + "</label>");
    //      result.Append("</div>");
    //    }
    //    result.Append("</div>");
    //  }

    //  return result.ToString();
    //}
  }
}
