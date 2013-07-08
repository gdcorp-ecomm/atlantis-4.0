using System;
using System.Text;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileRich
{
  public class MobileRichDateFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderField(FormFieldTypes fieldType, IProviderContainer providerContainer, out string htmlData)
    {
      var result = false;
      htmlData = string.Empty;

      try
      {
        var field = providerContainer.GetData<IDotTypeFormsField>(FieldTypeDataKeyConstants.DATE_DATA_KEY, null);
        if (field != null)
        {
          htmlData = ConvertToHtml(field);
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

    private static string ConvertToHtml(IDotTypeFormsField field)
    {
      var result = new StringBuilder();

      result.Append("<div class='section-row groove orient-vert'>");
      result.Append("<label class='font-sm'>" + HttpUtility.HtmlEncode(field.FieldLabel) + ":</label>");
      result.Append("<input type='date' class='max rnd' name='" + field.FieldName + "'>" + "</input>");
      result.Append("</div>");

      return result.ToString();
    }
  }
}
