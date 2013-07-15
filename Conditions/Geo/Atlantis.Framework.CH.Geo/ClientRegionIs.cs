using System.Collections.Generic;

namespace Atlantis.Framework.CH.Geo
{
  public class ClientRegionIs : GeoConditionHandler
  {
    public override string ConditionName
    {
      get { return "clientRegionIs"; }
    }

    protected override bool EvaluateCondition(IList<string> parameters, Providers.Geo.Interface.IGeoProvider geoProvider)
    {
      bool result = false;

      if (parameters.Count > 1)
      {
        int regionTypeId;
        if (int.TryParse(parameters[0], out regionTypeId))
        {
          result = geoProvider.IsUserInRegion(regionTypeId, parameters[1]);
        }
      }

      return result;
    }
  }
}
