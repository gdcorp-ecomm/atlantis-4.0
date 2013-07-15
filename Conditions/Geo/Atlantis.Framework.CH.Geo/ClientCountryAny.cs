using System.Collections.Generic;

namespace Atlantis.Framework.CH.Geo
{
  public class ClientCountryAny : GeoConditionHandler
  {
    public override string ConditionName
    {
      get { return "clientCountryAny"; }
    }

    protected override bool EvaluateCondition(IList<string> parameters, Providers.Geo.Interface.IGeoProvider geoProvider)
    {
      bool result = false;

      foreach (string countryCode in parameters)
      {
        if (geoProvider.IsUserInCountry(countryCode))
        {
          result = true;
          break;
        }
      }

      return result;
    }
  }
}
