using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface.Tests
{
  public class TestTemplateRequestKeyHandlerProvider : ProviderBase, ITemplateRequestKeyHandlerProvider
  {
    public TestTemplateRequestKeyHandlerProvider(IProviderContainer container) : base(container)
    {
    }

    public string GetFormattedTemplateRequestKey(string originalRequestKey, IProviderContainer providerContainer)
    {
      string language = "en"; // Normally this would come from the shopper preferences provider
      ISiteContext siteContext = providerContainer.Resolve<ISiteContext>();
      int contextId = siteContext.ContextId;

      return string.Format(originalRequestKey, language, "sales", contextId);
    }
  }
}
