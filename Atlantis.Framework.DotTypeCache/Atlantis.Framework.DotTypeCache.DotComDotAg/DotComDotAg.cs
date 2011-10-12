using System;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache.DotComDotAg
{
  public class DotComDotAg : DotTypeStaticBase
  {
    protected override DotTypeProductIds InitializeRegistrationProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Register, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override DotTypeProductIds InitializeTransferProductIds()
    {
      return null;
    }

    protected override DotTypeProductIds InitializeRenewalProductIds()
    {
      DotTypeTier DotTypeTier0 = new DotTypeTier(0, new int[] { 50051, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      DotTypeTier DotTypeTier6to20 = new DotTypeTier(6, new int[] { 50070, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      DotTypeTier DotTypeTier21to49 = new DotTypeTier(21, new int[] { 50070, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      DotTypeTier DotTypeTier50to100 = new DotTypeTier(50, new int[] { 50072, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      DotTypeTier DotTypeTier101to200 = new DotTypeTier(101, new int[] { 50074, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      DotTypeTier DotTypeTier201andup = new DotTypeTier(201, new int[] { 50076, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });

      DotTypeProductIds result = new DotTypeProductIds(DotTypeProductIdTypes.Renewal, new DotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.AG"; }
    }
  }
}
