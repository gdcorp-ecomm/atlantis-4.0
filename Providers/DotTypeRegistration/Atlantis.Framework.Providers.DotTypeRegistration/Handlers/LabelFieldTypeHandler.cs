using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers
{
  public class LabelFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderDotTypeFormField(DotTypeFormFieldTypes fieldType, IProviderContainer providerContainer, out IList<IFormField> formFields)
    {
      var result = false;
      formFields = new List<IFormField>();

      try
      {
        var field = providerContainer.GetData<IDotTypeFormsField>(FieldTypeDataKeyConstants.LABEL_DATA_KEY, null);
        if (field != null)
        {
          formFields = ConvertToFormFields(field);
          result = true;
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string source = "RenderField - LabelFieldTypeHandler";
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
        Type = FormFieldTypes.Label,
        DependsCollection = field.DependsCollection
      };

      result.Add(formField);

      return result;
    }
  }
}
