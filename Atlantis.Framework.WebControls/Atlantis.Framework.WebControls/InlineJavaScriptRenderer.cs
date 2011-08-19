using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Atlantis.Framework.WebControls
{
  public class InlineJavaScriptRenderer : LiteralControl
  {
    private const string HTTP_CONTEXT_KEY = "Atlantis.Framework.WebControls.InlineJavaScriptPlaceHolder.HttpContextKey";

    protected override void Render(HtmlTextWriter writer)
    {
      IDictionary<string, string> controlDictionary = HttpContext.Current.Items[HTTP_CONTEXT_KEY] as Dictionary<string, string>;

      if (controlDictionary != null)
      {
        foreach (string controlHtml in controlDictionary.Values)
        {
          writer.Write(controlHtml);
        }
      }

      writer.Flush();
      writer.Close();
      writer.Dispose();
    }
  }
}
