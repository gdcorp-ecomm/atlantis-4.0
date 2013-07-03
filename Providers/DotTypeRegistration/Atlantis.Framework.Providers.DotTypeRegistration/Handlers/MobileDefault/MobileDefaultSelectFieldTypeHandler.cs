using System;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileDefault
{
  public class MobileDefaultSelectFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderField(FormFieldTypes fieldType, IProviderContainer providerContainer, out string htmlData)
    {
      var result = false;
      htmlData = string.Empty;

      try
      {
        var field = providerContainer.GetData<IDotTypeFormsField>(FieldTypeDataKeyConstants.SELECT_DATA_KEY, null);
        if (field != null)
        {
          htmlData = ConvertToHtml(field);
          result = true;
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string SOURCE = "RenderField - MobileDefaultSelectFieldTypeHandler";
        var aex = new AtlantisException(SOURCE, "0", message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);

        result = false;
      }

      return result;
    }

    private static string ConvertToHtml(IDotTypeFormsField field)
    {
      var result = new StringBuilder();

      result.Append("<div class='form-field'>");
      result.Append("<div><label>" + field.FieldLabel + ":</label></div>");
      result.Append("<div><select name='" + field.FieldName + "'" + ">");

      foreach (var item in field.ItemCollection)
      {
        result.Append("<option value='" + item.ItemId + "'>" + item.ItemLabel);
        result.Append("</option>");
      }
      result.Append("</select></div>");
      result.Append("</div>");

      return result.ToString();
    }
  }
}
