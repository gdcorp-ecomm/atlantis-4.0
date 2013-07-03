using Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileDefault;
using Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileRich;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Factories
{
  public static class DotTypeFormFieldTypeFactory
  {
    public static IDotTypeFormFieldTypeHandler GetFormFieldTypeHandler(ViewTypes viewType, FormFieldTypes fieldType)
    {
      IDotTypeFormFieldTypeHandler fieldTypeHandler = null;

      switch (viewType)
      {
        case ViewTypes.MobileRich:
          switch (fieldType)
          {
            case FormFieldTypes.Claims:
              fieldTypeHandler = new MobileRichClaimFieldTypeHandler();
              break;
            case FormFieldTypes.Checkbox:
              fieldTypeHandler = new MobileRichCheckboxFieldTypeHandler();
              break;
            case FormFieldTypes.Select:
              fieldTypeHandler = new MobileRichSelectFieldTypeHandler();
              break;
            case FormFieldTypes.Radio:
              fieldTypeHandler = new MobileRichRadioFieldTypeHandler();
              break;
            case FormFieldTypes.String:
              fieldTypeHandler = new MobileRichStringFieldTypeHandler();
              break;
            case FormFieldTypes.Number:
              fieldTypeHandler = new MobileRichNumberFieldTypeHandler();
              break;
            case FormFieldTypes.Date:
              fieldTypeHandler = new MobileRichDateFieldTypeHandler();
              break;
            case FormFieldTypes.Datetime:
              fieldTypeHandler = new MobileRichDatetimeFieldTypeHandler();
              break;
            case FormFieldTypes.Phone:
              fieldTypeHandler = new MobileRichPhoneFieldTypeHandler();
              break;
            case FormFieldTypes.Email:
              fieldTypeHandler = new MobileRichEmailFieldTypeHandler();
              break;
          }
          break;
        case ViewTypes.MobileDefault:
          switch (fieldType)
          {
            case FormFieldTypes.Claims:
              fieldTypeHandler = new MobileDefaultClaimFieldTypeHandler();
              break;
            case FormFieldTypes.Checkbox:
              fieldTypeHandler = new MobileDefaultCheckboxFieldTypeHandler();
              break;
            case FormFieldTypes.Select:
              fieldTypeHandler = new MobileDefaultSelectFieldTypeHandler();
              break;
            case FormFieldTypes.Radio:
              fieldTypeHandler = new MobileDefaultRadioFieldTypeHandler();
              break;
            case FormFieldTypes.String:
              fieldTypeHandler = new MobileDefaultStringFieldTypeHandler();
              break;
            case FormFieldTypes.Number:
              fieldTypeHandler = new MobileDefaultNumberFieldTypeHandler();
              break;
            case FormFieldTypes.Date:
              fieldTypeHandler = new MobileDefaultDateFieldTypeHandler();
              break;
            case FormFieldTypes.Datetime:
              fieldTypeHandler = new MobileDefaultDatetimeFieldTypeHandler();
              break;
            case FormFieldTypes.Phone:
              fieldTypeHandler = new MobileDefaultPhoneFieldTypeHandler();
              break;
            case FormFieldTypes.Email:
              fieldTypeHandler = new MobileDefaultEmailFieldTypeHandler();
              break;
          }
          break;
      }

      return fieldTypeHandler;
    }
  }
}
