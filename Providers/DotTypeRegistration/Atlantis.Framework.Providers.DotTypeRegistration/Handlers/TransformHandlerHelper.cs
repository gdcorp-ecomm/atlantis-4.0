using System;
using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers
{
  public class TransformHandlerHelper
  {
    public static DotTypeFormFieldTypes GetFormFieldType(string fieldType)
    {
      DotTypeFormFieldTypes formFieldType;
      switch (fieldType.ToLowerInvariant())
      {
        case "claim":
          formFieldType = DotTypeFormFieldTypes.Claims;
          break;
        case "checkbox":
          formFieldType = DotTypeFormFieldTypes.Checkbox;
          break;
        case "select":
          formFieldType = DotTypeFormFieldTypes.Select;
          break;
        case "radio":
          formFieldType = DotTypeFormFieldTypes.Radio;
          break;
        case "string":
          formFieldType = DotTypeFormFieldTypes.String;
          break;
        case "number":
          formFieldType = DotTypeFormFieldTypes.Number;
          break;
        case "date":
          formFieldType = DotTypeFormFieldTypes.Date;
          break;
        case "datetime":
          formFieldType = DotTypeFormFieldTypes.Datetime;
          break;
        case "email":
          formFieldType = DotTypeFormFieldTypes.Email;
          break;
        case "phone":
          formFieldType = DotTypeFormFieldTypes.Phone;
          break;
        case "hidden":
          formFieldType = DotTypeFormFieldTypes.Hidden;
          break;
        case "label":
          formFieldType = DotTypeFormFieldTypes.Label;
          break;
        default:
          formFieldType = DotTypeFormFieldTypes.None;
          break;
      }
      return formFieldType;
    }

    public static bool SetFieldTypeData(DotTypeFormFieldTypes formFieldType, IProviderContainer providerContainer, string domain, IDotTypeFormsField field,
                                        int tldId, string placement, string phase, string language)
    {
      var result = true;

      try
      {
        switch (formFieldType)
        {
          case DotTypeFormFieldTypes.Claims:
            var tuple = new Tuple<IDotTypeFormsField, string, int, string, string, string>(field, domain, tldId, placement, phase, language);
            providerContainer.SetData(FieldTypeDataKeyConstants.CLAIM_DATA_KEY, tuple);
            break;
          case DotTypeFormFieldTypes.Checkbox:
            providerContainer.SetData(FieldTypeDataKeyConstants.CHECKBOX_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Select:
            providerContainer.SetData(FieldTypeDataKeyConstants.SELECT_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Radio:
            providerContainer.SetData(FieldTypeDataKeyConstants.RADIO_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.String:
            providerContainer.SetData(FieldTypeDataKeyConstants.STRING_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Number:
            providerContainer.SetData(FieldTypeDataKeyConstants.NUMBER_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Date:
            providerContainer.SetData(FieldTypeDataKeyConstants.DATE_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Datetime:
            providerContainer.SetData(FieldTypeDataKeyConstants.DATETIME_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Phone:
            providerContainer.SetData(FieldTypeDataKeyConstants.PHONE_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Email:
            providerContainer.SetData(FieldTypeDataKeyConstants.EMAIL_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Hidden:
            providerContainer.SetData(FieldTypeDataKeyConstants.HIDDEN_DATA_KEY, field);
            break;
          case DotTypeFormFieldTypes.Label:
            providerContainer.SetData(FieldTypeDataKeyConstants.LABEL_DATA_KEY, field);
            break;
          default:
            providerContainer.SetData(FieldTypeDataKeyConstants.CHECKBOX_DATA_KEY, field);
            break;
        }      
      }
      catch (Exception)
      {
        var exception = new AtlantisException("TransformHandlerHelper.SetFieldTypeData", 0, "Invalid data passed: Field type - " + formFieldType.ToString("F"), null);
        Engine.Engine.LogAtlantisException(exception);

        result = false;
      }

      return result;
    }
  }
}
