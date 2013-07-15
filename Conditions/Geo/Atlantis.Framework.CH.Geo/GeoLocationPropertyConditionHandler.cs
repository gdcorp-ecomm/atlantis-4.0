using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Geo
{
  public abstract class GeoLocationPropertyConditionHandler : GeoConditionHandler
  {
    protected abstract string GetLocationPropertyValue(IGeoLocation geoLocation);

    protected override bool EvaluateCondition(IList<string> parameters, IGeoProvider geoProvider)
    {
      bool result = false;
      string locationProperyValue = GetLocationPropertyValue(geoProvider.RequestGeoLocation);

      foreach (string paramValue in parameters)
      {
        if ((paramValue != null) && (paramValue.Equals(locationProperyValue, StringComparison.OrdinalIgnoreCase)))
        {
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
