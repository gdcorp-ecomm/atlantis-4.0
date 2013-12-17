using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Localization
{
  public class ActiveMarketDisplayProvider : ProviderBase, IActiveMarketDisplayProvider
  {    
    private readonly Lazy<CountrySitesActiveResponseData> _countrySitesActive;
    private readonly Lazy<ILocalizationProvider> _localizationProvider;
    
    public ActiveMarketDisplayProvider(IProviderContainer container) : base(container)
    {
      _countrySitesActive = new Lazy<CountrySitesActiveResponseData>(LoadActiveCountrySites);
      _localizationProvider = new Lazy<ILocalizationProvider>(() => Container.Resolve<ILocalizationProvider>());
    }

    private CountrySitesActiveResponseData LoadActiveCountrySites()
    {
      CountrySitesActiveResponseData result;

      var request = new CountrySitesActiveRequestData();
      try
      {
        result = (CountrySitesActiveResponseData)DataCache.DataCache.GetProcessRequest(request, EngineRequests.CountrySitesActiveRequest);
      }
      catch
      {
        result = CountrySitesActiveResponseData.DefaultCountrySites;
      }

      return result;
    }

    private IList<IActiveMarketDisplay> LoadActiveMarketDisplays(bool includeInternalOnly)
    {
      var activeMarketDisplays = new List<IActiveMarketDisplay>();

      IEnumerable<ICountrySite> activeCountrySites = ActiveCountrySites.CountrySites.Where(cs => includeInternalOnly || !cs.IsInternalOnly).OrderBy(cs => cs.Description);

      foreach (var countrySite in activeCountrySites)
      {
        IEnumerable<IMarket> markets = _localizationProvider.Value.GetMappedMarketsForCountrySite(countrySite.Id, includeInternalOnly);
        foreach (var market in markets.OrderBy(m => m.Description))
        {
          string countryName;
          string language;
          ParseMarketDescription(market.Description, out countryName, out language);

          IActiveMarketDisplay activeMarketDisplay = new ActiveMarketDisplay
          {
            CountrySiteId = countrySite.Id,
            MarketId = market.Id,
            MarketDescription = market.Description,
            CountryName = countryName,
            Language = language
          };
          activeMarketDisplays.Add(activeMarketDisplay);
        }
      }

      return activeMarketDisplays;
    }

    public IList<IActiveMarketDisplay> GetActiveMarketDisplay(bool includeInternalOnly)
    {
      return LoadActiveMarketDisplays(includeInternalOnly);
    }

    private CountrySitesActiveResponseData ActiveCountrySites
    {
      get { return _countrySitesActive.Value; }
    }

    private static void ParseMarketDescription(string marketDescription, out string countryName, out string language)
    {
      string[] parts = marketDescription.Split((new[] { " - " }), StringSplitOptions.RemoveEmptyEntries);
      language = parts.Length > 1 ? parts[1] : string.Empty;
      countryName = parts[0];
    }
  }
}
