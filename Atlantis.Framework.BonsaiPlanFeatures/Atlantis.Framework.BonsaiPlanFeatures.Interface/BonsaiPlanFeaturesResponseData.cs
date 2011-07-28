using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BonsaiPlanFeatures.Interface
{
  public class BonsaiPlanFeaturesResponseData : IResponseData
  {
    public Dictionary<string, string> PlanFeatures { get; private set; }
    public AtlantisException AtlantisException { get; private set; }

    public BonsaiPlanFeaturesResponseData(Dictionary<string, string> planFeatures)
    {
      PlanFeatures = planFeatures;
    }

    public BonsaiPlanFeaturesResponseData(AtlantisException atlEx)
    {
      AtlantisException = atlEx;
    }

    #region IResponseData Members

    public string ToXML()
    {
      throw new NotImplementedException("ToXML not implemented in BonsaiPlanFeaturesResponseData"); 
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }

    #endregion
  }
}
