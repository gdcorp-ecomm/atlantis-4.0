using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Conditions.Interface
{
  internal class NullConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "null"; } }

    public bool EvaluateCondition(string conditionName, IList<string> expectedValues, IProviderContainer providerContainer)
    {

      AtlantisException atlantisException = new AtlantisException("NullConditionHandler.EvaluateCondition",
                                                                  "0",
                                                                  string.Format("Condition \"{0}\" does not have a defined IConditionHandler.", conditionName),
                                                                  string.Empty,
                                                                  providerContainer.Resolve<ISiteContext>(),
                                                                  providerContainer.Resolve<IShopperContext>());

      Engine.Engine.LogAtlantisException(atlantisException);

      return false;
    }
  }
}
