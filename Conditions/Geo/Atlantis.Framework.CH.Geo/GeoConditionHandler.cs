using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Geo
{
  public abstract class GeoConditionHandler : IConditionHandler
  {
    public abstract string ConditionName { get; }
    protected abstract bool EvaluateCondition(IList<string> parameters, IGeoProvider geoProvider);

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      IGeoProvider geoProvider = providerContainer.Resolve<IGeoProvider>();
      return EvaluateCondition(parameters, geoProvider);
    }
  }
}
