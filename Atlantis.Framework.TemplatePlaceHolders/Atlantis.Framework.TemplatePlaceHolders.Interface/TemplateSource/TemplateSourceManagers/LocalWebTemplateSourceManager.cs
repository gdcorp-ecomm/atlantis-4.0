using System;
using System.IO;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class LocalWebTemplateSourceManager : ITemplateSourceManager
  {
    private string GetTemplateFromFile(string filePath)
    {
      string template;

      try
      {
        if(!File.Exists(filePath))
        {
          throw new Exception(string.Format("localweb template does not exist. Path: {0}", filePath));
        }

        using (StreamReader streamReader = new StreamReader(filePath))
        {
          template = streamReader.ReadToEnd();
        } 
      }
      catch (Exception ex)
      {
        // Log silent and return empty?
        throw new Exception(string.Format("Unable to load localweb template. Path: {0}, Exception: {1}", filePath, ex.Message), ex);
      }

      return template;
    }

    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      string template;

      if(HttpContext.Current == null)
      {
        // TODO: Log silent instead and return emtpy?
        throw new Exception("TemplateSource \"source\" value \"localweb\" can only be used in a web context.");
      }

      ITemplateRequestKeyHandlerProvider templateRequestKeyHandlerProvider = TemplateRequestKeyHandlerFactory.GetInstance(providerContainer);
      string filePath = HttpContext.Current.Server.MapPath(templateRequestKeyHandlerProvider.GetFormattedTemplateRequestKey(templateSource.RequestKey, providerContainer));

      template = DataCache.DataCache.GetCustomCacheData(filePath, GetTemplateFromFile);

      return template;
    }
  }
}