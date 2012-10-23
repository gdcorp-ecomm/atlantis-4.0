using System;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public class CodeClassTemplateSourceManager : ITemplateSourceManager
  {
    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      // TODO: Cache all of this reflection

      ITemplateContentProvider templateContentProvider;

      ITemplateRequestKeyHandlerProvider templateRequestKeyHandlerProvider = TemplateRequestKeyHandlerFactory.GetInstance(providerContainer);
      string typeKey = templateRequestKeyHandlerProvider.GetFormattedTemplateRequestKey(templateSource.RequestKey, providerContainer);

      Assembly assembly = !string.IsNullOrEmpty(templateSource.SourceAssembly) ? Assembly.Load(templateSource.SourceAssembly) : AssemblyHelper.GetApplicationAssembly();
      Type typeOfTemplateContent = assembly.GetType(typeKey);

      if (typeOfTemplateContent != null)
      {
        if (typeOfTemplateContent.GetInterface("ITemplateContentProvider") != typeof(ITemplateContentProvider))
        {
          throw new Exception(string.Format("Static file type \"{0}\" does not implement \"ITemplateContentProvider\".", typeKey));
        }

        if(!providerContainer.TryResolve(typeOfTemplateContent, out templateContentProvider))
        {
          throw new Exception(string.Format("Unable to resolve ProviderType \"{0}\". Make sure it is registered by your provider container.", typeKey));
        }
      }
      else
      {
        throw new Exception(string.Format("Unable to reflect type of static file \"{0}\".", typeKey));
      }

      return templateContentProvider.Content;
    }
  }
}
