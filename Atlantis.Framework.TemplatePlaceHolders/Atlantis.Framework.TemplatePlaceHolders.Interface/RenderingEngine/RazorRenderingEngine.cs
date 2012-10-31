using System;
using System.Reflection;
using RazorEngine;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class RazorRenderingEngine : IRenderingEngine
  {
    public string Render(string rawContent, dynamic model)
    {
      string html;

      try
      {
        html = model == null ? string.Empty : Razor.Parse(rawContent, model);
      }
      catch (Exception ex)
      {
        html = string.Empty;
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to render razor template. Exception: {0}", ex.Message)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return html;
    }
  }
}
