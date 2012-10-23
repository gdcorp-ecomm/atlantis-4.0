using System;
using RazorEngine;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class RazorRenderingEngine : IRenderingEngine
  {
    public string Render<T>(string rawContent, T model)
    {
      string html;

      try
      {
        html = Razor.Parse(rawContent, model);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Unable to render razor template. Exception: {0}", ex.Message), ex);
      }

      return html;
    }
  }
}
