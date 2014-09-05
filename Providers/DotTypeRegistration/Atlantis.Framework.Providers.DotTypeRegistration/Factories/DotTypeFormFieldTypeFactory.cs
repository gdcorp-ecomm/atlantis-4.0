using Atlantis.Framework.Providers.DotTypeRegistration.Handlers;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Factories
{
  public static class DotTypeFormFieldTypeFactory
  {
    public static IDotTypeFormFieldTypeHandler GetFormFieldTypeHandler(DotTypeFormFieldTypes fieldType)
    {
      IDotTypeFormFieldTypeHandler fieldTypeHandler = null;

      switch (fieldType)
      {
        case DotTypeFormFieldTypes.Claims:
          fieldTypeHandler = new ClaimFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Checkbox:
          fieldTypeHandler = new CheckboxFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Select:
          fieldTypeHandler = new SelectFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Radio:
          fieldTypeHandler = new RadioFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.String:
          fieldTypeHandler = new StringFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Number:
          fieldTypeHandler = new NumberFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Date:
          fieldTypeHandler = new DateFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Datetime:
          fieldTypeHandler = new DatetimeFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Phone:
          fieldTypeHandler = new PhoneFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Email:
          fieldTypeHandler = new EmailFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Hidden:
          fieldTypeHandler = new HiddenFieldTypeHandler();
          break;
        case DotTypeFormFieldTypes.Label:
          fieldTypeHandler = new LabelFieldTypeHandler();
          break;
      }

      return fieldTypeHandler;
    }
  }
}
