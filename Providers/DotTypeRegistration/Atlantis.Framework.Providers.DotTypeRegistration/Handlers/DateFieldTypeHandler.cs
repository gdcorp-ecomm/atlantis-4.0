using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers
{
  public class DateFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderDotTypeFormField(DotTypeFormFieldTypes fieldType, IProviderContainer providerContainer, out IList<IFormField> formFields)
    {
      var result = false;
      formFields = new List<IFormField>();

      try
      {
        var field = providerContainer.GetData<IDotTypeFormsField>(FieldTypeDataKeyConstants.DATE_DATA_KEY, null);
        if (field != null)
        {
          formFields = ConvertToFormFields(field);
          result = true;
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string SOURCE = "RenderField - MobileRichDateFieldTypeHandler";
        var aex = new AtlantisException(SOURCE, "0", message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);

        result = false;
      }

      return result;
    }

    private static IList<IFormField> ConvertToFormFields(IDotTypeFormsField field)
    {
      IList<IFormField> result = new List<IFormField>();

      var formField = new FormField { Name = field.FieldName, LabelText = field.FieldLabel, Type = FormFieldTypes.InputDate };
      result.Add(formField);

      return result;
    }
  }
}
