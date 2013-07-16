using System.IO;
using System.Text;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal static class RenderControlManager
  {
    internal static string ToHtml(Control control)
    {
      string html = string.Empty;

      if (control != null)
      {
        StringBuilder htmlStringBuilder = new StringBuilder();

        using (StringWriter stringWriter = new StringWriter(htmlStringBuilder))
        {
          using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter))
          {
            control.RenderControl(htmlTextWriter);
            html = htmlStringBuilder.ToString();
          }
        }
      }

      return html;
    }
  }
}
