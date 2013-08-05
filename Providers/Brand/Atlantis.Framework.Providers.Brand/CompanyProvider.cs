using System;
using System.Collections.Generic;
using Atlantis.Framework.Brand.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Brand.Interface;

namespace Atlantis.Framework.Providers.Brand
{
  public class CompanyProvider : ProviderBase, ICompanyProvider
  {

    #region Providers

    private readonly Lazy<ISiteContext> _siteContext;

    #endregion


    public CompanyProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
    }

    private Dictionary<string, string> _companyDict; 
    public Dictionary<string, string> CompanyDict
    {
      get
      {
        if (_companyDict == null)
        {
          var companyNameRequestData = new CompanyNameRequestData(_siteContext.Value.ContextId, _siteContext.Value.PrivateLabelId);
          var companyNameResponseData = (CompanyNameResponseData)Engine.Engine.ProcessRequest(companyNameRequestData, BrandEngineRequests.CompanyNameRequestId);
          _companyDict = companyNameResponseData.CompanyDict;
        }

        return _companyDict;
      }

    }

    public string GetCompanyPropertyValue(string companyPropertyKey)
    {
      string companyValue;

      CompanyDict.TryGetValue(companyPropertyKey, out companyValue);

      return companyValue ?? String.Empty;
    }
  }
}
