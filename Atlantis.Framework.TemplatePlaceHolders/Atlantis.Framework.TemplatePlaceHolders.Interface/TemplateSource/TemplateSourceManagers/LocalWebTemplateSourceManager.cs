using System;
using System.IO;
using System.Reflection;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class LocalWebTemplateSourceManager : ITemplateSourceManager
  {
    private string GetTemplateFromFile(string filePath)
    {
      string template = string.Empty;

      try
      {
        if(!File.Exists(filePath))
        {
          ErrorLogHelper.LogError(new Exception(string.Format("localweb template does not exist. Path: {0}", filePath)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        using (StreamReader streamReader = new StreamReader(filePath))
        {
          template = streamReader.ReadToEnd();
        } 
      }
      catch (Exception ex)
      {
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to load localweb template. Path: {0}, Exception: {1}", filePath, ex.Message)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return template;
    }

    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      string template;

      if (HttpContext.Current == null)
      {
        template = string.Empty;
        ErrorLogHelper.LogError(new Exception("TemplateSource \"source\" value \"localweb\" can only be used in a web context."), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }
      else
      {
        ITemplateRequestKeyHandlerProvider templateRequestKeyHandlerProvider = TemplateRequestKeyHandlerFactory.GetInstance(providerContainer);
        string filePath = HttpContext.Current.Server.MapPath(templateRequestKeyHandlerProvider.GetFormattedTemplateRequestKey(templateSource.RequestKey, providerContainer));

        template = DataCache.DataCache.GetCustomCacheData(filePath, GetTemplateFromFile);
      }

      return template;
    }
  }
}