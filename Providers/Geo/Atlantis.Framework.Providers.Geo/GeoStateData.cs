using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.Providers.Geo
{
  internal class GeoStateData
  {
    readonly List<IGeoState> _geoStates;
    readonly Dictionary<string, IGeoState> _geoStatesByCode;

    internal GeoStateData(IProviderContainer container, int countryId)
    {
      _geoStates = new List<IGeoState>();
      _geoStatesByCode = new Dictionary<string, IGeoState>(StringComparer.OrdinalIgnoreCase);

      var request = new StateRequestData(countryId);
      StateResponseData stateResponse = null;

      try
      {
        stateResponse = (StateResponseData)DataCache.DataCache.GetProcessRequest(request, GeoProviderEngineRequests.States);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(request, "GeoStateData.ctor", ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }

      if (stateResponse != null)
      {
        foreach (var state in stateResponse.States)
        {
          IGeoState geoState = GeoState.FromState(container, state, countryId);
          _geoStates.Add(geoState);
          _geoStatesByCode[geoState.Code] = geoState;
        }
      }

      bool isSorted = false;
      try
      {
        ILocalizationProvider localization;
        if (container.TryResolve<ILocalizationProvider>(out localization) && localization.CurrentCultureInfo != null)
        {
          _geoStates = _geoStates.OrderBy(c => c.Name, StringComparer.Create(localization.CurrentCultureInfo, true)).ToList();
          isSorted = true;
        }
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(request, "GeoStateData.ctor", ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }

      if (!isSorted)
      {
        _geoStates = _geoStates.OrderBy(c => c.Name).ToList();
      }
    }

    public IEnumerable<IGeoState> States
    {
      get { return _geoStates; }
    }

    public bool HasStates
    {
      get { return _geoStates.Count > 0; }
    }

    public bool TryGetGeoStateByCode(string stateCode, out IGeoState state)
    {
      return _geoStatesByCode.TryGetValue(stateCode, out state);
    }
  }
}
