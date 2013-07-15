using Atlantis.Framework.Providers.Geo.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Geo
{
  public class ClientCityAny : GeoLocationPropertyConditionHandler
  {
    public override string ConditionName
    {
      get { return "clientCityAny"; }
    }

    protected override string GetLocationPropertyValue(IGeoLocation geoLocation)
    {
      return geoLocation.City;
    }
  }
}
