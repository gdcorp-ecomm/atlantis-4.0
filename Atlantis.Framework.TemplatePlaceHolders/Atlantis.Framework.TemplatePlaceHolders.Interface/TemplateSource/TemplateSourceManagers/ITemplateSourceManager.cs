using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal interface ITemplateSourceManager
  {
    string GetTemplateSource(ITemplateSource templateSource, IProviderContainer providerContainer);
  }
}
