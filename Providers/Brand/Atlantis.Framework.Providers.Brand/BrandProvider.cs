using System;
using Atlantis.Framework.Brand.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;
using Atlantis.Framework.Providers.Language.Interface;

namespace Atlantis.Framework.Providers.Brand
{
  public class BrandProvider : ProviderBase, IBrandProvider
  {

    private const string _DICTIONARY = "cds.atlantis/providers/brand";

    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<ILanguageProvider> _languageProvider;

    public BrandProvider(IProviderContainer container) : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _languageProvider = new Lazy<ILanguageProvider>(() => Container.Resolve<ILanguageProvider>());
    }

    private bool IsDomainsByProxy(string companyPropertyKey)
    {
      return companyPropertyKey == BrandKeyConstants.NAME_DBP || 
             companyPropertyKey == BrandKeyConstants.NAME_DBP_DOT_COM || 
             companyPropertyKey == BrandKeyConstants.NAME_DBP_LEGAL ||
             companyPropertyKey == BrandKeyConstants.NAME_DBP_PARENT_COMPANY_LEGAL;
    }

    public string GetCompanyName(string companyPropertyKey)
    {
      string companyValue;

      if (_siteContext.Value.ContextId == 6 && !IsDomainsByProxy(companyPropertyKey))
      {
        companyValue = DataCache.DataCache.GetPLData(_siteContext.Value.PrivateLabelId, 0);
      }
      else
      {
        var companyNameRequestData = new CompanyNameRequestData(_siteContext.Value.ContextId);
        var companyNameResponseData = (CompanyNameResponseData)DataCache.DataCache.GetProcessRequest(companyNameRequestData, BrandEngineRequests.CompanyNameRequestId);

        companyValue = companyNameResponseData.GetName(companyPropertyKey);
      }

      return companyValue ?? String.Empty;
    }

    public string GetProductLineName(string productLineKey, int contextId = 0)
    {
      string key = productLineKey.ToLowerInvariant();
      return _languageProvider.Value.GetLanguagePhrase(_DICTIONARY, key);
    }
  }
}
