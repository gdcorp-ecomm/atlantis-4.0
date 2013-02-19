using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotComDotAg : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40051, 40052, 40053, 40054, 40055, 40056, 40057, 40058, 40059, 40060 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50051, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50070, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50070, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50072, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50074, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50076, 50052, 50053, 50054, 50055, 50056, 50057, 50058, 50059, 50060 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "COM.AG"; }
    }
  }
}
