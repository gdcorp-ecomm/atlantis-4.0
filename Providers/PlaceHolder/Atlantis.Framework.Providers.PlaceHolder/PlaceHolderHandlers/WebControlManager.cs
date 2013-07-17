using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal static class WebControlManager
  {
    private static void SetControlProperties(Control control, Type type, PlaceHolderData placeHolderData)
    {
      if (placeHolderData != null)
      {
        foreach (string parameterKey in placeHolderData.ParametersDictionary.Keys)
        {
          PropertyInfo propertyInfo = type.GetProperty(parameterKey);
          if (propertyInfo != null && propertyInfo.CanWrite)
          {
            propertyInfo.SetValue(control, placeHolderData.ParametersDictionary[parameterKey], null);
          }
        }
      }
    }

    internal static Control LoadControl(Type type, PlaceHolderData placeHolderData)
    {
      Page currentPage = HttpContext.Current.Handler == null ? new Page() : (Page)HttpContext.Current.Handler;
      Control control = currentPage.LoadControl(type, null);

      if (control == null)
      {
        throw new Exception("Unable to load web control: " + type);
      }

      SetControlProperties(control, type, placeHolderData);

      return control;
    }

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
