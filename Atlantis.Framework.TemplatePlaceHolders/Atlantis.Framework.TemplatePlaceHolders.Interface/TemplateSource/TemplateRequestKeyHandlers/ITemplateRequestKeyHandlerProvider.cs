using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  public interface ITemplateRequestKeyHandlerProvider
  {
    string GetFormattedTemplateRequestKey(string originalRequestKey, IProviderContainer providerContainer);
  }
}
