using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class NullTemplateSourceManager : ITemplateSourceManager
  {
    public string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer)
    {
      // TODO: Log silent?
      return string.Empty;
    }
  }
}
