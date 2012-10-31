using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public class CodeClassTemplateSourceManager : ITemplateSourceManager
  {
    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      string templateContent;
      
      ITemplateRequestKeyHandlerProvider templateRequestKeyHandlerProvider = TemplateRequestKeyHandlerFactory.GetInstance(providerContainer);
      string typeKey = templateRequestKeyHandlerProvider.GetFormattedTemplateRequestKey(templateSource.RequestKey, providerContainer);

      ITemplateContentProvider templateContentProvider;
      if (ProviderTypeCacheManager.GetProvider(templateSource.SourceAssembly, typeKey, providerContainer, out templateContentProvider))
      {
        templateContent = templateContentProvider.Content;
      }
      else
      {
        templateContent = string.Empty;
        ErrorLogHelper.LogError(new Exception(string.Format("Unable to reflect type of static file \"{0}\".", typeKey)), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      }

      return templateContent;
    }
  }
}
