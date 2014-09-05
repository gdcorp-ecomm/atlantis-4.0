using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers
{
  public class SelectFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderDotTypeFormField(DotTypeFormFieldTypes fieldType, IProviderContainer providerContainer, out IList<IFormField> formFields)
    {
      var result = false;
      formFields = new List<IFormField>();

      try
      {
        var field = providerContainer.GetData<IDotTypeFormsField>(FieldTypeDataKeyConstants.SELECT_DATA_KEY, null);
        if (field != null)
        {
          formFields = ConvertToFormFields(field);
          result = true;
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string source = "RenderField - SelectFieldTypeHandler";
        var aex = new AtlantisException(source, 0, message, string.Empty);
        Engine.Engine.LogAtlantisException(aex);

        result = false;
      }

      return result;
    }

    private static IList<IFormField> ConvertToFormFields(IDotTypeFormsField field)
    {
      IList<IFormField> result = new List<IFormField>();

      var formField = new FormField
      {
        Name = field.FieldName, 
        LabelText = field.FieldLabel,
        DescriptionText = field.FieldDescription,
        Required = field.FieldRequired,
        DefaultValue = field.FieldDefaultValue,
        Type = FormFieldTypes.Select, 
        ItemCollection = field.ItemCollection,
        DependsCollection = field.DependsCollection
      };
      result.Add(formField);

      return result;
    }

    //private static string ConvertToHtml(IDotTypeFormsField field)
    //{
    //  var result = new StringBuilder();

    //  result.Append("<div class='section-row groove orient-vert'>");
    //  result.Append("<label class='font-sm'>" + field.FieldLabel + ":</label>");
    //  result.Append("<select name='" + field.FieldName + "'" + ">");

    //  foreach (var item in field.ItemCollection)
    //  {
    //    result.Append("<option value='" + item.ItemId + "'>" + item.ItemLabel);
    //    result.Append("</option>");
    //  }
    //  result.Append("</select>");
    //  result.Append("</div>");

    //  return result.ToString();
    //}
  }
}
