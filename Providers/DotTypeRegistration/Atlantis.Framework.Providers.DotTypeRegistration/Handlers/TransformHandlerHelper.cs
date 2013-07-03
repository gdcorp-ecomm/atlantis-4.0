using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers
{
  public class TransformHandlerHelper
  {
    public static FormFieldTypes GetFormFieldType(string fieldType)
    {
      FormFieldTypes formFieldType;
      switch (fieldType.ToLowerInvariant())
      {
        case "claims":
          formFieldType = FormFieldTypes.Claims;
          break;
        case "checkbox":
          formFieldType = FormFieldTypes.Checkbox;
          break;
        case "select":
          formFieldType = FormFieldTypes.Select;
          break;
        case "radio":
          formFieldType = FormFieldTypes.Radio;
          break;
        case "string":
          formFieldType = FormFieldTypes.String;
          break;
        case "number":
          formFieldType = FormFieldTypes.Number;
          break;
        case "date":
          formFieldType = FormFieldTypes.Date;
          break;
        case "datetime":
          formFieldType = FormFieldTypes.Datetime;
          break;
        case "email":
          formFieldType = FormFieldTypes.Email;
          break;
        case "phone":
          formFieldType = FormFieldTypes.Phone;
          break;
        default:
          formFieldType = FormFieldTypes.None;
          break;
      }
      return formFieldType;
    }

    public static bool SetFieldTypeData(FormFieldTypes formFieldType, IProviderContainer providerContainer, string[] domains, IDotTypeFormsField field)
    {
      var result = true;

      try
      {
        switch (formFieldType)
        {
          case FormFieldTypes.Claims:
            var tuple = new Tuple<IDotTypeFormsField, string[]>(field, domains);
            providerContainer.SetData(FieldTypeDataKeyConstants.CLAIM_DATA_KEY, tuple);
            break;
          case FormFieldTypes.Checkbox:
            providerContainer.SetData(FieldTypeDataKeyConstants.CHECKBOX_DATA_KEY, field);
            break;
          case FormFieldTypes.Select:
            providerContainer.SetData(FieldTypeDataKeyConstants.SELECT_DATA_KEY, field);
            break;
          case FormFieldTypes.Radio:
            providerContainer.SetData(FieldTypeDataKeyConstants.RADIO_DATA_KEY, field);
            break;
          case FormFieldTypes.String:
            providerContainer.SetData(FieldTypeDataKeyConstants.STRING_DATA_KEY, field);
            break;
          case FormFieldTypes.Number:
            providerContainer.SetData(FieldTypeDataKeyConstants.NUMBER_DATA_KEY, field);
            break;
          case FormFieldTypes.Date:
            providerContainer.SetData(FieldTypeDataKeyConstants.DATE_DATA_KEY, field);
            break;
          case FormFieldTypes.Datetime:
            providerContainer.SetData(FieldTypeDataKeyConstants.DATETIME_DATA_KEY, field);
            break;
          case FormFieldTypes.Phone:
            providerContainer.SetData(FieldTypeDataKeyConstants.PHONE_DATA_KEY, field);
            break;
          case FormFieldTypes.Email:
            providerContainer.SetData(FieldTypeDataKeyConstants.EMAIL_DATA_KEY, field);
            break;
          default:
            providerContainer.SetData(FieldTypeDataKeyConstants.CHECKBOX_DATA_KEY, field);
            break;
        }      
      }
      catch (Exception)
      {
        var exception = new AtlantisException("TransformHandlerHelper.SetFieldTypeData", "0", "Invalid data passed: Field type - " + formFieldType.ToString("F"), null, null, null);
        Engine.Engine.LogAtlantisException(exception);

        result = false;
      }

      return result;
    }
  }
}
