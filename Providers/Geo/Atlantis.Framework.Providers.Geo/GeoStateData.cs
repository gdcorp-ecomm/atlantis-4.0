using Atlantis.Framework.Geo.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Geo
{
  internal class GeoStateData
  {
    IProviderContainer _container;
    List<IGeoState> _geoStates;

    Dictionary<string, IGeoState> _geoStatesByCode;

    internal GeoStateData(IProviderContainer container, int countryId)
    {
      _container = container;

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
        AtlantisException aex = new AtlantisException(request, "GeoStateData.ctor", ex.Message + ex.StackTrace, string.Empty);
        Engine.Engine.LogAtlantisException(aex);
      }

      if (stateResponse != null)
      {
        foreach (var state in stateResponse.States)
        {
          IGeoState geoState = GeoState.FromState(_container, state);
          _geoStates.Add(geoState);
          _geoStatesByCode[geoState.Code] = geoState;
        }
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
