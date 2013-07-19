using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Atlantis.Framework.Providers.PlaceHolder
{
  internal static class WebControlManager
  {
    private static readonly IDictionary<string, MethodInfo> _eventMethodCache = new Dictionary<string, MethodInfo>(8);

    static WebControlManager()
    {
      InitializeEvents(); 
    }

    private static void InitializeEvents()
    {
      Type controlType = typeof(Control);

      _eventMethodCache[WebControlEvents.Init] = controlType.GetMethod(WebControlEvents.Init, BindingFlags.Instance | BindingFlags.NonPublic);
      _eventMethodCache[WebControlEvents.Load] = controlType.GetMethod(WebControlEvents.Load, BindingFlags.Instance | BindingFlags.NonPublic);
      _eventMethodCache[WebControlEvents.PreRender] = controlType.GetMethod(WebControlEvents.PreRender, BindingFlags.Instance | BindingFlags.NonPublic);
    }

    private static bool TryGetEvent(string eventName, out MethodInfo methodInfo)
    {
      return _eventMethodCache.TryGetValue(eventName, out methodInfo);
    }

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

    internal static Control Contruct(Type type, PlaceHolderData placeHolderData)
    {
      Page currentPage = HttpContext.Current.Handler == null ? new Page() : (Page) HttpContext.Current.Handler;
      
      Control control = currentPage.LoadControl(type, null);

      if (control == null)
      {
        throw new Exception("Unhandled error loading control");
      }

      SetControlProperties(control, type, placeHolderData);

      return control;
    }

    internal static void FireEvent(string eventName, Control control)
    {
      if (control != null)
      {
        MethodInfo methodInfo;
        if (TryGetEvent(eventName, out methodInfo) && methodInfo != null)
        {
          methodInfo.Invoke(control, new object[] { EventArgs.Empty });
        }
      }
    }

    internal static string Render(Control control)
    {
      string html = string.Empty;

      if (control != null && control.Visible)
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