using System;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileDefault
{
  public class MobileDefaultRadioFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderField(FormFieldTypes fieldType, IProviderContainer providerContainer, out string htmlData)
    {
      var result = false;
      htmlData = string.Empty;

      try
      {
        var field = providerContainer.GetData<IDotTypeFormsField>(FieldTypeDataKeyConstants.RADIO_DATA_KEY, null);
        if (field != null)
        {
          htmlData = ConvertToHtml(field);
          result = true;
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string SOURCE = "RenderField - MobileDefaultRadioFieldTypeHandler";
        var aex = new AtlantisException(SOURCE, "0", message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);

        result = false;
      }

      return result;
    }

    private static string ConvertToHtml(IDotTypeFormsField field)
    {
      var result = new StringBuilder();

      if (field.ItemCollection.Count > 0)
      {
        result.Append("<div class='form-field'>");
        foreach (var item in field.ItemCollection)
        {
          result.Append("<div>");
          result.Append("<input type='radio' name='" + field.FieldName + "' value='" + item.ItemId + ">'</input>");
          result.Append("<label>" + field.FieldLabel + "</label>");
          result.Append("</div>");
        }
        result.Append("</div>");
      }

      return result.ToString();
    }
  }
}
