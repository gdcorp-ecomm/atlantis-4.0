using System.Collections.Generic;
using System.Linq;
using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;

namespace Atlantis.Framework.Providers.Personalization
{
  public class PersonalizationDataProvider : ProviderBase, IPersonalizationDataProvider
  {
    protected readonly Lazy<ILocalizationProvider> _localizationProvider;
    protected readonly Lazy<ISiteContext> _siteContext;

    public PersonalizationDataProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _localizationProvider = new Lazy<ILocalizationProvider>(() => Container.Resolve<ILocalizationProvider>());
    }

    #region IPersonalizationDataProvider Members

    public virtual Dictionary<string, string> GetChannelSessionData()
    {
      Dictionary<string, string> result = new Dictionary<string, string>();
      string value;

      if (!String.IsNullOrWhiteSpace(value = GetISC()))
      {
        result["isc"] = value;
      }

      if (!String.IsNullOrWhiteSpace(value = GetCountrySite()))
      {
        result["countrysite"] = value;
      }

      if (!String.IsNullOrWhiteSpace(value = GetFullLanguage()))
      {
        result["market"] = value;
      }

      if (!String.IsNullOrWhiteSpace(value = GetShortLanguage()))
      {
        result["lang"] = value;
      }

      return result;
    }

    #endregion

    protected string GetCountrySite()
    {
      try
      {
        return _localizationProvider.Value.CountrySite;
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("PersonalizationDataProvider.GetChannelSessionData",
          0, ex.ToString(), "Failure retrieving the 'CountrySite' value.");
        Engine.Engine.LogAtlantisException(aex);
        return null;
      }
    }

    protected string GetFullLanguage()
    {
      try
      {
        return _localizationProvider.Value.FullLanguage;
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("PersonalizationDataProvider.GetChannelSessionData",
          0, ex.ToString(), "Failure retrieving the 'Language (Full)' value.");
        Engine.Engine.LogAtlantisException(aex);
        return null;
      }
    }

    protected string GetISC()
    {
      try
      {
        // Add ISC
        return _siteContext.Value.ISC;
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("PersonalizationDataProvider.GetChannelSessionData",
          0, ex.ToString(), "Failure retrieving the 'ISC' value.");
        Engine.Engine.LogAtlantisException(aex);
        return null;
      }
    }

    protected string GetShortLanguage()
    {
      try
      {
        return _localizationProvider.Value.ShortLanguage;
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException("PersonalizationDataProvider.GetChannelSessionData",
          0, ex.ToString(), "Failure retrieving the 'Language (Short)' value.");
        Engine.Engine.LogAtlantisException(aex);
        return null;
      }
    }
  }
}