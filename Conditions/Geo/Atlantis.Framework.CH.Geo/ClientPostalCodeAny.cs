using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.CH.Geo
{
  public class ClientPostalCodeAny : GeoLocationPropertyConditionHandler
  {
    protected override string GetLocationPropertyValue(IGeoLocation geoLocation)
    {
      return geoLocation.PostalCode;
    }

    public override string ConditionName
    {
      get { return "clientPostalCodeAny"; }
    }
  }
}
