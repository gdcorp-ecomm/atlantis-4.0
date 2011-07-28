using System;
using System.Collections.Generic;

using Atlantis.Framework.Interface;
using Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface
{
  public class BonsaiGetPlanOptionsResponseData : IResponseData
  {
    private readonly string _accountXml = String.Empty;

    public List<ProductPlan> ProductPlans { get; private set; }
    public CategoryAddonCollection Addons { get; private set; }
    public List<PrepaidAddon> PrepaidAddons { get; private set; }
    public List<FilteredProductPlan> FilteredProductPlans { get; private set; }
    public AtlantisException AtlantisException { get; private set; }

    public BonsaiGetPlanOptionsResponseData() { }

    public BonsaiGetPlanOptionsResponseData(AtlantisException atlEx)
    {
      AtlantisException = atlEx;
    }

    public BonsaiGetPlanOptionsResponseData(string accountXml, List<ProductPlan> plans, List<FilteredProductPlan> filteredPlans,
                                            CategoryAddonCollection addons, List<PrepaidAddon> prepaids)
    {
      _accountXml = accountXml;
      ProductPlans = plans;
      Addons = addons;
      PrepaidAddons = prepaids;
      FilteredProductPlans = filteredPlans;
    }
    
    #region IResponseData Members

    public string ToXML()
    {
      return _accountXml;
    }

    public AtlantisException GetException()
    {
      return AtlantisException;
    }

    #endregion
  }
}
