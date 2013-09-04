using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.Geo
{
  public class GeoState : IGeoState
  {
    internal static IGeoState FromState(IProviderContainer container, State state, int countryId)
    {
      return new GeoState(container, state, countryId);
    }

    private readonly int _countryId;
    private readonly IProviderContainer _container;
    private readonly State _state;
    private string _name;

    private GeoState(IProviderContainer container, State state, int countryId)
    {
      _container = container;
      _state = state;
      _countryId = countryId;
    }

    public int Id
    {
      get { return _state.Id; }
    }

    public string Code
    {
      get { return _state.Code; }
    }

    public string Name
    {
      get { return _name ?? (_name = DetermineLanguageAwareName()); }
    }

    private string DetermineLanguageAwareName()
    {
      var result = _state.Name;

      if (!_container.CanResolve<ILocalizationProvider>())
      {
        return result;
      }

      var localization = _container.Resolve<ILocalizationProvider>();
      if (localization.IsActiveLanguage("en-us"))
      {
        return result;
      }

      try
      {
        var request = new StateNamesRequestData(localization.FullLanguage, _countryId);
        var response = (StateNamesResponseData)DataCache.DataCache.GetProcessRequest(request, GeoProviderEngineRequests.StateNames);
        string languageName;
        if (response.TryGetNameById(_state.Id, out languageName))
        {
          result = languageName;
        }
      }
      catch
      {
        // DataCache logged the exception, keep default english name.
      }

      return result;
    }
  }
}
