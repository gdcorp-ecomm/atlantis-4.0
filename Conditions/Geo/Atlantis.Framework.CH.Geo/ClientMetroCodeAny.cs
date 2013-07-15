using Atlantis.Framework.Providers.Geo.Interface;

namespace Atlantis.Framework.CH.Geo
{
  public class ClientMetroCodeAny : GeoLocationPropertyConditionHandler
  {
    protected override string GetLocationPropertyValue(IGeoLocation geoLocation)
    {
      return geoLocation.MetroCode.ToString();
    }

    public override string ConditionName
    {
      get { return "clientMetroCodeAny"; }
    }
  }
}
